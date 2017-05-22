using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPacman : TileMove
{
    int dotsRemaining;
    public float offTimer = 0;
    public int tilesPassedThrough = 0;
    [Header("Animator")]
    Animator anim;
    public PacmanMovement Pacman;
    public PacmanAI PacAI;

    [Header("Colour / States")]
//    bool canTurnBool = true;  // Never used?

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

    public GameObject[] dots;
    // Use this for initialization

    void OnEnable()
    {
        transform.position = PacAI.transform.position;
        moveChecker = moveCheckerGraphic.position = (Vector2)PacAI.transform.position;
    }

    void Start()
    {
        dots = GameObject.FindGameObjectsWithTag("Dot");
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

    void LateUpdate()
    {
        offTimer += Time.deltaTime;

        if (transform.position == targetTileGraphic.position && offTimer > 0.1f)        //if hit final tile!!!
        {
            dotsRemaining = 0;

            foreach (GameObject d in dots)
            {
                if (d.activeSelf)
                {
                    dotsRemaining++;
                }
            }

            PerceptionInfo.Get.SetDotsRemaining(dotsRemaining);

            PacAI.tilesToGoThrough = tilesPassedThrough;
            PerceptionInfo.Get.SetTilesToDestination(tilesPassedThrough);
            tilesPassedThrough = 0;
            gameObject.SetActive(false);
            offTimer = 0;

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
    // Update is called once per frame
    void Update()
    {
        if (true)
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
                transform.position = new Vector2(transform.position.x + 31f, transform.position.y);
                moveChecker = new Vector3(Mathf.Round(transform.position.x) - 0.5f, Mathf.Round(transform.position.y), 0);
            }
            if (transform.position.x >= 15.5f)
            {
                transform.position = new Vector2(transform.position.x - 31f, transform.position.y);
                moveChecker = new Vector3(Mathf.Round(transform.position.x) + 0.5f, Mathf.Round(transform.position.y), 0);
            }

            //if he hits an intersection
            for (int i = 0; i < intersections.Length; i++)
            {
                if (transform.position == intersectionsPos[i].transform.position)
                {
//                    print("HIT AN INTERSECTION");

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
//        else
//        {
//            transform.position = startPosition;
//            moveChecker = transform.position;
//        }
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
            tilesPassedThrough++;

            PerceptionInfo.Get.TileSurvived();
            //if you can, move forward
            if (isValidMove(moveVec))
            {
                moveChecker += (moveVec);
            }
            else                //hit a corner
            {
//                print("HIT CORNER");
                //right
                //
                //
                //
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

                /*
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
                 * */

            }

        }
    }
}

