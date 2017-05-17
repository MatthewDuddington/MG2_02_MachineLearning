using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanAI : TileMove
{
    public int tilesToGoThrough;
    public GameObject magicPacman;
    [Header("Animator")]
    Animator anim;
    public PacmanMovement Pacman;

    [Header("Colour / States")]
    bool canTurnBool = true;

    [Header("Movement / Targets")]
    public Transform pacDest;
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
    // Use this for initialization

    void OnEnable()
    {
        PerceptionInfo.Get.UpdatePerception();

        moveCheckerGraphic.transform.position = transform.position;
        moveChecker = moveCheckerGraphic.position;
    }
    void Start()
    {
        Pacman = GetComponent<PacmanMovement>();
        anim = GetComponent<Animator>();
        startPosition = transform.position;
        moveChecker = transform.position;
        intersections = GameObject.FindGameObjectsWithTag("Intersection");
        for (int i = 0; i < intersections.Length; i++)
        {
            intersectionsPos[i] = intersections[i].transform;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Dot")
        {
            col.gameObject.SetActive(false);
            //  Destroy(col.gameObject);
            score += 10;
            transform.GetChild(0).GetComponent<AudioSource>().Play();
            PerceptionInfo.Get.DotEaten();
        }
        if (col.gameObject.tag == "PowerPellet")
        {
            //  Destroy(col.gameObject);
            col.gameObject.SetActive(false);

            score += 100;
            gsc.Scare();
            transform.GetChild(0).GetComponent<AudioSource>().Play();
        }
        if (col.gameObject.tag == "Ghost")
        {
            if (col.GetComponent<Ghost2>().myState == Ghost2.Statey.Scared)
            {
                score += 200;
                transform.GetChild(0).GetComponent<AudioSource>().Play();
                col.GetComponent<Ghost2>().Eaten();
            }
            if (col.GetComponent<Ghost2>().myState == Ghost2.Statey.Eaten)
            {
                //e
            }
            else
            {
                PacManBrain.Get.LearnFromDesision();
                Application.LoadLevel(Application.loadedLevel);
               // Pacman.isAlive = false;
            }
        }
    }


    void LateUpdate()
    {
        if (transform.position == targetTileGraphic.position)
        {
            //hit end final tile!!!
            print("HIT THAT BAD BOY");

            magicPacman.SetActive(true);

            PacManBrain.Get.LearnFromDesision();

            PerceptionInfo.Get.UpdatePerception();

        }

    }
    // Update is called once per frame
    void Update()
    {
        if (Pacman.isAlive)
        {
            anim.Play("Pac_Move");

            moveCheckerGraphic.position = moveChecker;

            targetTile = targetTileGraphic.transform.position;

            upDistance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y + 1), targetTile);
            downDistance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y - 1), targetTile);
            leftDistance = Vector2.Distance(new Vector2(transform.position.x - 1, transform.position.y), targetTile);
            rightDistance = Vector2.Distance(new Vector2(transform.position.x + 1, transform.position.y), targetTile);

            //warp through tunnels
            if (transform.position.x < -15.5f)
            {
                transform.position = new Vector2(transform.position.x + 31f, transform.position.y);
                moveChecker = new Vector3(Mathf.Round(transform.position.x) - 0.5f, Mathf.Round(transform.position.y), 0);
            }
            if (transform.position.x > 15.5f)
            {
                transform.position = new Vector2(transform.position.x - 31f, transform.position.y);
                moveChecker = new Vector3(Mathf.Round(transform.position.x) + 0.5f, Mathf.Round(transform.position.y), 0);
            }

            //if he hits an intersection
            for (int i = 0; i < intersections.Length; i++)
            {
                if (transform.position == intersectionsPos[i].transform.position)
                {
                    print("HIT AN INTERSECTION");

                    if (isValidMove(Vector2.up) && upDistance < rightDistance && upDistance < downDistance && upDistance < leftDistance)
                    {
                        //if (moveVec != Vector2.down)
                        {
                            moveVec = Vector2.up;
                        }
                        

                    }
                    if (isValidMove(Vector2.down) && downDistance < rightDistance && downDistance < upDistance && downDistance < leftDistance)
                    {
                        //if (moveVec != Vector2.up)
                        {
                            moveVec = Vector2.down;         //all good.
                        }
                        
                    }
                    if (isValidMove(Vector2.right) && rightDistance < upDistance && rightDistance < downDistance && rightDistance < leftDistance)
                    {

                        //if (moveVec != Vector2.left)
                        {
                            moveVec = Vector2.right;
                        }
                        
                    }
                    if (isValidMove(Vector2.left) && leftDistance < rightDistance && leftDistance < downDistance && leftDistance < upDistance)
                    {
                        //if (moveVec != Vector2.right)
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
            //if you can, move forward
            if (isValidMove(moveVec))
            {
                moveChecker += (moveVec);
            }
            else                //hit a corner
            {
                print("HIT CORNER");
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

