using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanAI : TileMove
{
    public int tilesToGoThrough;
    public int currentTilesGoneThrough;
    public int howManyDotsIAte = 0;
    public MagicPacman magicPacman;
    [Header("Animator")]
    Animator anim;
    public PacmanMovement Pacman;
    public PacMasterControl pacMasterControl;

    [Header("Colour / States")]
//    bool canTurnBool = true;  // Never used?

    [Header("Movement / Targets")]
//    public Transform pacDest;  // Never used?
    public Vector2 targetTile;
    public Transform targetTileGraphic;
    public Vector2 moveChecker;
    public Transform moveCheckerGraphic;
    Vector2 startPosition;

    public GhostStateChanger gsc;

    public Vector2 moveVec = new Vector2(1, 0); //start right.
    public float speed = 0.18f;
    int score;

    public GameObject[] intersections;
    public Transform[] intersectionsPos;

    float upDistance, downDistance, leftDistance, rightDistance;

    void OnEnable()
    {
        if (PerceptionInfo.Get != null) {
            PerceptionInfo.Get.UpdatePerception();
        }

        moveCheckerGraphic.transform.position = transform.position;
        moveChecker = moveCheckerGraphic.position;
    }

    void Start()
    {
        Pacman = GetComponent<PacmanMovement>();
        anim = GetComponent<Animator>();
//        magicPacman = GameObject.FindObjectOfType<MagicPacman>();
        startPosition = transform.position;
        moveChecker = transform.position;
        intersections = GameObject.FindGameObjectsWithTag("Intersection");
        for (int i = 0; i < intersections.Length; i++)
        {
            intersectionsPos[i] = intersections[i].transform;
        }

        // COPYED FROM PACMANMOVE (SEE UPDATE COPY BELOW)
        dest = transform.position;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Dot")
        {
            col.gameObject.SetActive(false);
//            Destroy(col.gameObject);
            score += 10;

            int asdf = Random.Range(0, 3);
            transform.GetChild(asdf).GetComponent<AudioSource>().Play();
            howManyDotsIAte++;
            PerceptionInfo.Get.DotEaten();
        }
        if (col.gameObject.tag == "PowerPellet")
        {
//            Destroy(col.gameObject);
            col.gameObject.SetActive(false);

            score += 100;
            gsc.Scare();
            int asdf = Random.Range(0, 3);
            transform.GetChild(asdf).GetComponent<AudioSource>().Play();
        }
        if (col.gameObject.tag == "Ghost")
        {
            if (col.GetComponent<Ghost2>().myState == Ghost2.Statey.Scared)
            {
                score += 200;
                int asdf = Random.Range(0, 3);
                transform.GetChild(asdf).GetComponent<AudioSource>().Play();

                col.GetComponent<Ghost2>().Eaten();
            }
            if (col.GetComponent<Ghost2>().myState == Ghost2.Statey.Eaten)
            {
                //e
            }
            else
            {
                if (PacManBrain.Get.activeBrainmode == PacManBrain.BrainMode.UnsupervisedTraining) {
//                    print("AISFHASIFHASIFHASIFHAISFHASIFHASIFHASIFH");
                    Debug.Log("Decision failed. Learning...");
                    PacManBrain.Get.LearnFromDecision(true);

                    Debug.Log ("Restarting level...");
//                    Application.LoadLevel(Application.loadedLevel);
                    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);  // Stop Unity complaining about obsolete functions

//                    Pacman.isAlive = false;
                }
            }
        }
    }

    void LateUpdate()
    {
        if ( PacManBrain.Get.activeBrainmode == PacManBrain.BrainMode.UnsupervisedTraining
          && transform.position == targetTileGraphic.position)
        {
            //hit end final tile!!!
            if (pacMasterControl.myPlayerState == PacMasterControl.playerState.AI) {
                Debug.Log ("Reached destination tile");

                PacManBrain.Get.LearnFromDecision (false);  // Learning when not dead

                PerceptionInfo.Get.UpdatePerception ();

                currentTilesGoneThrough = 0;
                howManyDotsIAte = 0;

                // Update destination
                Vector2 newDestination = PacManBrain.Get.ChooseDestination ();
                Vector3 newDestinationV3 = new Vector3(newDestination.x, newDestination.y, 0);
                targetTileGraphic.position = newDestinationV3;

                // Give magicPacMan the new destination to scout out the route to
                magicPacman.targetTileGraphic.position = newDestination;
                magicPacman.gameObject.SetActive (true);

                if (upDistance < leftDistance && upDistance < rightDistance && upDistance < downDistance && isValidMove(Vector2.up))
                {
                    moveVec = Vector2.up;
                }
                else if (rightDistance < leftDistance && rightDistance < upDistance && rightDistance < downDistance && isValidMove(Vector2.right))
                {
                    moveVec = Vector2.right;
                }
                else if (leftDistance < upDistance && leftDistance < rightDistance && leftDistance < downDistance && isValidMove(Vector2.left))
                {
                    moveVec = Vector2.left;
                }
                else if (downDistance < leftDistance && downDistance < rightDistance && downDistance < upDistance && isValidMove(Vector2.down))
                {
                    moveVec = Vector2.down;
                }
            }
        }
    }

    public Vector2 moveVec2;
    Vector2 actualVec2;
    Rigidbody2D rb;
    float destTimer;
    public Transform destTransform;
    Vector3 dest = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if ( Pacman.isAlive
          && PacManBrain.Get.activeBrainmode == PacManBrain.BrainMode.UnsupervisedTraining)
        {
            anim.Play("Pac_Move");

            moveCheckerGraphic.position = moveChecker;

            targetTile = targetTileGraphic.transform.position;

            upDistance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y + 1), targetTile);
            downDistance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y - 1), targetTile);
            leftDistance = Vector2.Distance(new Vector2(transform.position.x - 1, transform.position.y), targetTile);
            rightDistance = Vector2.Distance(new Vector2(transform.position.x + 1, transform.position.y), targetTile);

            //warp through tunnels
            if (transform.position.x <= -15.5f)
            {
                transform.position = new Vector2(transform.position.x + 30f, transform.position.y);
                moveChecker = new Vector3(Mathf.Round(transform.position.x) - 0.5f, Mathf.Round(transform.position.y), 0);
            }
            if (transform.position.x >= 15.5f)
            {
                transform.position = new Vector2(transform.position.x - 30f, transform.position.y);
                moveChecker = new Vector3(Mathf.Round(transform.position.x) + 0.5f, Mathf.Round(transform.position.y), 0);
            }
        
            //if he hits an intersection
            if (PacManBrain.Get.activeBrainmode == PacManBrain.BrainMode.UnsupervisedTraining)
            {
                for (int i = 0; i < intersections.Length; i++)
                {
                    if (transform.position == intersectionsPos[i].transform.position)
                    {
//                      print("HIT AN INTERSECTION");

                        if (isValidMove(Vector2.up) && upDistance < rightDistance && upDistance < downDistance && upDistance < leftDistance)
                        {
                                moveVec = Vector2.up;                       
                        }
                        if (isValidMove(Vector2.down) && downDistance < rightDistance && downDistance < upDistance && downDistance < leftDistance)
                        {
                                moveVec = Vector2.down;         //all good.
                        }
                        if (isValidMove(Vector2.right) && rightDistance < upDistance && rightDistance < downDistance && rightDistance < leftDistance)
                        {
                                moveVec = Vector2.right;
                        }
                        if (isValidMove(Vector2.left) && leftDistance < rightDistance && leftDistance < downDistance && leftDistance < upDistance)
                        {
                                moveVec = Vector2.left;
                        }
                    }
                }
            }
        
            MoveChecker();

            //actual movement
            moveTo(moveChecker, speed);
        }
        else if ( Pacman.isAlive
               && PacManBrain.Get.activeBrainmode == PacManBrain.BrainMode.PlayLikeMe)
////////---START OF COPY FROM PACMANMOVE---////////
        {
          //rotations and animation setting
          if (transform.position != dest)
          {
            if (moveVec2 == Vector2.right)
            {
              rb.rotation = 0;
            }
            if (moveVec2 == -Vector2.up)
            {
              rb.rotation = -90;
            }
            if (moveVec2 == -Vector2.right)
            {
              rb.rotation = 180;
            }
            if (moveVec2 == Vector2.up)
            {
              rb.rotation = 90;
            }
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Pac_Idle"))
            {
              anim.Play("Pac_Move");
              destTimer = 0;
            }
          }
          else
          {
            destTimer += Time.deltaTime;
          }

          if (destTimer >= 0.061f)
          {
            // print("stopped!");
            anim.Play("Pac_Idle");
          }


          //warp

          if (transform.position.x <= -15.5f)
          {
            transform.position = new Vector2(transform.position.x + 30f, transform.position.y);
            dest = new Vector3(Mathf.Round(transform.position.x) - 0.5f, Mathf.Round(transform.position.y), 0);
          }
          if (transform.position.x >= 15.5f)
          {
            transform.position = new Vector2(transform.position.x - 30f, transform.position.y);
            dest = new Vector3(Mathf.Round(transform.position.x) + 0.5f, Mathf.Round(transform.position.y), 0);
          }
          //
          //visual guide for dest position -- debug only.
          destTransform.position = dest;

          //Input
          //raw input
//          actualVec2 = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
//          actualVecText.text = "Actual: " + actualVec2;
//          moveVecText.text = "MoveVec: " + moveVec2;

          //bug fix
          Vector2 dir = dest - this.transform.position;

      // THIS IS WHERE THE OUTPUT DIRECTION IS TAKEN FROM THE NETWORK
      moveVec2 = PacManBrain.Get.GetDirectionForPacMan();
      print("Network thinks: " + moveVec2);

          // recheck the keys for a new movement
          if (transform.position == dest)
          {
            //does the sexy 2d array of bools;
//            pacChecker.WallCheck();

            if (isValidMove(moveVec2 + dir))
            {
              dest = this.transform.position + (Vector3)moveVec2;
            }
          }
          
          // Gets the offset from the destination to the current postion (change in direction)
          moveTo(dest, speed);
        }
////////---END COPY FROM FROM PACMANMOVE---////////
        else
        {
            transform.position = startPosition;
            moveChecker = transform.position;
        }

    }

    public void Restart()
    {
        transform.position = startPosition;
        moveChecker = transform.position;
    }

    void MoveChecker()                 
    {
        if (moveChecker == (Vector2)transform.position)         //every time he hits a tile
        {
            PerceptionInfo.Get.TileSurvived();
            currentTilesGoneThrough++;
            //if you can, move forward
            if (isValidMove(moveVec))
            {
                moveChecker += (moveVec);
            }
            else                //hit a corner
            {
//                print("HIT CORNER");
                //right

                if (moveVec == Vector2.right && isValidMove(Vector2.down)) //if down is free?
                {
                    if (downDistance < upDistance && isValidMove(Vector2.down))
                        moveVec = Vector2.down;
                    else if (isValidMove(Vector2.up))
                        moveVec = Vector2.up;
                    else
                        moveVec = Vector2.down;

                    moveChecker += moveVec;
                }
                else if (moveVec == Vector2.right && isValidMove(Vector2.up)) //if down is free?
                {
                    if (upDistance < downDistance && isValidMove(Vector2.up))
                        moveVec = Vector2.up;
                    else if (isValidMove(Vector2.down))
                        moveVec = Vector2.down;
                    else
                        moveVec = Vector2.up;

                    moveChecker += moveVec;
                }

                //

                //left
                else if (moveVec == Vector2.left && isValidMove(Vector2.down)) //if down is free?
                {
                    if (downDistance < upDistance && isValidMove(Vector2.down))
                        moveVec = Vector2.down;
                    else if (isValidMove(Vector2.up))
                        moveVec = Vector2.up;
                    else
                        moveVec = Vector2.down;

                    moveChecker += moveVec;
                }
                else if (moveVec == Vector2.left && isValidMove(Vector2.up)) //if down is free?
                {
                    if (upDistance < downDistance && isValidMove(Vector2.up))
                        moveVec = Vector2.up;
                    else if (isValidMove(Vector2.down))
                        moveVec = Vector2.down;
                    else
                        moveVec = Vector2.up;

                    moveChecker += moveVec;
                }

                //

                //up
                else if (moveVec == Vector2.up && isValidMove(Vector2.right)) //if down is free?
                {
                    if (rightDistance < leftDistance && isValidMove(Vector2.right))
                        moveVec = Vector2.right;
                    else if (isValidMove(Vector2.left))
                        moveVec = Vector2.left;
                    else
                        moveVec = Vector2.right;

                    moveChecker += moveVec;
                }
                else if (moveVec == Vector2.up && isValidMove(Vector2.left)) //if down is free?
                {
                    if (leftDistance < rightDistance && isValidMove(Vector2.left))
                        moveVec = Vector2.left;
                    else if (isValidMove(Vector2.right))
                        moveVec = Vector2.right;
                    else
                        moveVec = Vector2.left;

                    moveChecker += moveVec;
                }

                //

                //down
                else if (moveVec == Vector2.down && isValidMove(Vector2.right)) //if down is free?
                {
                    if (rightDistance < leftDistance && isValidMove(Vector2.right))
                        moveVec = Vector2.right;
                    else if (isValidMove(Vector2.left))
                        moveVec = Vector2.left;
                    else
                        moveVec = Vector2.right;

                    moveChecker += moveVec;
                }
                else if (moveVec == Vector2.down && isValidMove(Vector2.left)) //if down is free?
                {
                    if (leftDistance < rightDistance && isValidMove(Vector2.left))
                        moveVec = Vector2.left;
                    else if (isValidMove(Vector2.right))
                        moveVec = Vector2.right;
                    else
                        moveVec = Vector2.left;

                    moveChecker += moveVec;
                }
            }
        }
    }
}


//
//

/*if (moveVec == Vector2.right && isValidMove(Vector2.down)) //if down is free?
                {
                    if (downDistance < upDistance && isValidMove(Vector2.down))
                        moveVec = Vector2.down;
                    else if (isValidMove(Vector2.up))
                        moveVec = Vector2.up;
                    else
                        moveVec = Vector2.down;

                    moveChecker += moveVec;
                }
                else if (moveVec == Vector2.right && isValidMove(Vector2.up)) //if down is free?
                {
                    if (upDistance < downDistance && isValidMove(Vector2.up))
                        moveVec = Vector2.up;
                    else if (isValidMove(Vector2.down))
                        moveVec = Vector2.down;
                    else
                        moveVec = Vector2.up;

                    moveChecker += moveVec;
                }

                //

                //left
                else if (moveVec == Vector2.left && isValidMove(Vector2.down)) //if down is free?
                {
                    if (downDistance < upDistance && isValidMove(Vector2.down))
                        moveVec = Vector2.down;
                    else if (isValidMove(Vector2.up))
                        moveVec = Vector2.up;
                    else
                        moveVec = Vector2.down;

                    moveChecker += moveVec;
                }
                else if (moveVec == Vector2.left && isValidMove(Vector2.up)) //if down is free?
                {
                    if (upDistance < downDistance && isValidMove(Vector2.up))
                        moveVec = Vector2.up;
                    else if (isValidMove(Vector2.down))
                        moveVec = Vector2.down;
                    else
                        moveVec = Vector2.up;

                    moveChecker += moveVec;
                }

                //

                //up
                else if (moveVec == Vector2.up && isValidMove(Vector2.right)) //if down is free?
                {
                    if (rightDistance < leftDistance && isValidMove(Vector2.right))
                        moveVec = Vector2.right;
                    else if (isValidMove(Vector2.left))
                        moveVec = Vector2.left;
                    else
                        moveVec = Vector2.right;

                    moveChecker += moveVec;
                }
                else if (moveVec == Vector2.up && isValidMove(Vector2.left)) //if down is free?
                {
                    if (leftDistance < rightDistance && isValidMove(Vector2.left))
                        moveVec = Vector2.left;
                    else if (isValidMove(Vector2.right))
                        moveVec = Vector2.right;
                    else
                        moveVec = Vector2.left;

                    moveChecker += moveVec;
                }

                //

                //down
                else if (moveVec == Vector2.down && isValidMove(Vector2.right)) //if down is free?
                {
                    if (rightDistance < leftDistance && isValidMove(Vector2.right))
                        moveVec = Vector2.right;
                    else if (isValidMove(Vector2.left))
                        moveVec = Vector2.left;
                    else
                        moveVec = Vector2.right;

                    moveChecker += moveVec;
                }
                else if (moveVec == Vector2.down && isValidMove(Vector2.left)) //if down is free?
                {
                    if (leftDistance < rightDistance && isValidMove(Vector2.left))
                        moveVec = Vector2.left;
                    else if (isValidMove(Vector2.right))
                        moveVec = Vector2.right;
                    else
                        moveVec = Vector2.left;

                    moveChecker += moveVec;
                }
* */
