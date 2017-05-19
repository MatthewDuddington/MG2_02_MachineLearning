using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost2 : TileMove
{
    [Header("Animator")]
    Animator anim;
    public PacmanMovement Pacman;
    public Transform redGhost;
    public LineRenderer blueLine;

    [Header("Colour / States")]
    public float stateTimer;
    public enum GhostColour { Red, Orange, Blue, Pink };
    public GhostColour myGhostColour;
    bool canTurnBool = true;

    public enum Statey { Chase, Corner, Scared, Eaten };
    public Statey myState;


    [Header("Movement / Targets")]
    public Transform pacDest;
    public Vector2 targetTile;
    public Transform targetTileGraphic;
    public Vector2 moveChecker;
    public Transform moveCheckerGraphic;
    Vector2 startPosition;


    public Vector2 moveVec = new Vector2(1, 0); //start right.
    public float speed = 0.2f;
    public float scaredSpeed;


    public GameObject[] intersections;
    public Transform[] intersectionsPos;

    float upDistance, downDistance, leftDistance, rightDistance;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        startPosition = transform.position;
        moveChecker = transform.position;
        intersections = GameObject.FindGameObjectsWithTag("Intersection");
        for (int i = 0; i < intersections.Length; i++)
        {
            intersectionsPos[i] = intersections[i].transform;
        }
        if (myGhostColour == GhostColour.Blue)
        {
            blueLine = GetComponent<LineRenderer>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Pacman.isAlive)
        {
            if (myGhostColour == GhostColour.Red)
            {
                if (myState == Statey.Chase)
                {
                    targetTileGraphic.position = targetTile = pacDest.position;
                }
                if (myState == Statey.Corner)
                {
                    targetTileGraphic.position = targetTile = new Vector2(12.5f, 20f);
                }
                if (myState != Statey.Scared && myState != Statey.Eaten)
                {
                    canTurnBool = true;
                    if (moveVec == Vector2.right)
                    {
                        anim.Play("Red_Right");
                    }
                    if (moveVec == Vector2.up)
                    {
                        anim.Play("Red_Up");
                    }
                    if (moveVec == Vector2.left)
                    {
                        anim.Play("Red_Left");
                    }
                    if (moveVec == Vector2.down)
                    {
                        anim.Play("Red_Down");
                    }
                }
                else if(myState == Statey.Scared && canTurnBool)
                {
                    anim.Play("Red_Scared_Up");
                    if (isValidMove(-moveVec))
                    {
                        moveVec = -moveVec;
                    }
                    canTurnBool = false;
                }
                else if (myState == Statey.Eaten)
                {
                    anim.Play("Red_Eaten");
                }
            }
            if (myGhostColour == GhostColour.Pink)
            {
                if (myState == Statey.Chase)
                {
                    targetTileGraphic.position = targetTile = (Vector2)pacDest.position + (Pacman.moveVec2 * 4);
                }
                if (myState == Statey.Corner)
                {
                    targetTileGraphic.position = targetTile = new Vector2(-12.5f, 20f);
                }
                if (myState != Statey.Scared && myState != Statey.Eaten)
                {
                    canTurnBool = true;

                    if (moveVec == Vector2.right)
                    {
                        anim.Play("Pink_Right");
                    }
                    if (moveVec == Vector2.up)
                    {
                        anim.Play("Pink_Up");
                    }
                    if (moveVec == Vector2.left)
                    {
                        anim.Play("Pink_Left");
                    }
                    if (moveVec == Vector2.down)
                    {
                        anim.Play("Pink_Down");
                    }
                }
                else if (myState == Statey.Scared && canTurnBool)
                {
                    anim.Play("Red_Scared_Up");
                    if (isValidMove(-moveVec))
                    {
                        moveVec = -moveVec;
                    }
                    canTurnBool = false;
                }
                else if (myState == Statey.Eaten)
                {
                    anim.Play("Red_Eaten");
                }
            }
            if (myGhostColour == GhostColour.Blue)
            {
                Vector3 startPoint = new Vector3(redGhost.position.x, redGhost.position.y, -5f);
                Vector3 midPoint = (Vector2)Pacman.transform.position + Pacman.moveVec2 * 2;
                Vector3 disty = new Vector3(startPoint.x - midPoint.x, startPoint.y - midPoint.y, 5f);
                // Vector3 endPoint = new Vector3(,,-5f);
                //print(midPoint);

                blueLine.SetPosition(0, startPoint);
                blueLine.SetPosition(1, -disty + midPoint);
                if (myState == Statey.Chase)
                {
                    targetTileGraphic.position = targetTile = -disty + midPoint;
                }
                if (myState == Statey.Corner)
                {
                    targetTileGraphic.position = targetTile = new Vector2(12.5f, -12f);
                }
                if (myState != Statey.Scared && myState != Statey.Eaten)
                {
                    canTurnBool = true;

                    if (moveVec == Vector2.right)
                    {
                        anim.Play("Blue_Right");
                    }
                    if (moveVec == Vector2.up)
                    {
                        anim.Play("Blue_Up");
                    }
                    if (moveVec == Vector2.left)
                    {
                        anim.Play("Blue_Left");
                    }
                    if (moveVec == Vector2.down)
                    {
                        anim.Play("Blue_Down");
                    }
                }
                else if (myState == Statey.Scared && canTurnBool)
                {
                    anim.Play("Red_Scared_Up");
                    if (isValidMove(-moveVec))
                    {
                        moveVec = -moveVec;
                    }
                    canTurnBool = false;
                }
                else if (myState == Statey.Eaten)
                {
                    anim.Play("Red_Eaten");
                }
            }
            if (myGhostColour == GhostColour.Orange)
            {
                if (myState == Statey.Chase)
                {
                    if (Vector2.Distance(transform.position, pacDest.position) >= 8)
                    {
                        targetTileGraphic.position = targetTile = pacDest.position;
                    }
                    else
                    {
                        targetTileGraphic.position = targetTile = new Vector2(-12.5f, -12f);
                    }
                }
                if (myState == Statey.Corner)
                {
                    targetTileGraphic.position = targetTile = new Vector2(-12.5f, -12f);
                }
                if (myState != Statey.Scared && myState != Statey.Eaten)
                {
                    canTurnBool = true;

                    if (moveVec == Vector2.right)
                    {
                        anim.Play("Orange_Right");
                    }
                    if (moveVec == Vector2.up)
                    {
                        anim.Play("Orange_Up");
                    }
                    if (moveVec == Vector2.left)
                    {
                        anim.Play("Orange_Left");
                    }
                    if (moveVec == Vector2.down)
                    {
                        anim.Play("Orange_Down");
                    }
                }
                else if (myState == Statey.Scared && canTurnBool)
                {
                    anim.Play("Red_Scared_Up");
                    if (isValidMove(-moveVec))
                    {
                        moveVec = -moveVec;
                    }
                    canTurnBool = false;
                }
                else if (myState == Statey.Eaten)
                {
                    anim.Play("Red_Eaten");
                }
            }
            if (myState == Statey.Scared && transform.position == (Vector3)moveChecker)
            {
                targetTileGraphic.position = targetTile = new Vector2(Random.Range(-12f, 12f), Random.Range(-10f, 18f));
            }
            if (myState == Statey.Eaten)
            {
                targetTileGraphic.position = targetTile = new Vector2(0.5f,8f);

                if(transform.position == new Vector3(0.5f, 8f,0))
                {
                    myState = Statey.Chase;
                }
            }

            moveCheckerGraphic.position = moveChecker;

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

            for (int i = 0; i < intersections.Length; i++)
            {
                if (transform.position == intersectionsPos[i].transform.position)
                {
                    print("HIT AN INTERSECTION");

                    if (isValidMove(Vector2.up) && upDistance < rightDistance && upDistance < downDistance && upDistance < leftDistance)
                    {
                        if (moveVec != Vector2.down)
                        {
                            moveVec = Vector2.up;
                        }
                        else
                        {
                            if (leftDistance <= downDistance && leftDistance <= rightDistance && isValidMove(Vector2.left))
                            {
                                moveVec = Vector2.left;
                            }
                            if (rightDistance <= upDistance && rightDistance <= leftDistance && isValidMove(Vector2.right))
                            {
                                moveVec = Vector2.right;
                            }
                        }

                    }
                    if (isValidMove(Vector2.down) && downDistance < rightDistance && downDistance < upDistance && downDistance < leftDistance)
                    {
                        if (moveVec != Vector2.up)
                        {
                            moveVec = Vector2.down;         //all good.
                        }
                        else        //uh oh
                        {
                            if (leftDistance <= upDistance && leftDistance <= rightDistance && isValidMove(Vector2.left))
                            {
                                moveVec = Vector2.left;
                            }
                            if (rightDistance <= upDistance && rightDistance <= leftDistance && isValidMove(Vector2.right))
                            {
                                moveVec = Vector2.right;
                            }
                        }
                    }
                    if (isValidMove(Vector2.right) && rightDistance < upDistance && rightDistance < downDistance && rightDistance < leftDistance)
                    {

                        if (moveVec != Vector2.left)
                        {
                            moveVec = Vector2.right;
                        }
                        else
                        {
                            if (upDistance <= downDistance && upDistance <= leftDistance && isValidMove(Vector2.up))
                            {
                                moveVec = Vector2.up;
                            }
                            if (downDistance <= upDistance && downDistance <= leftDistance && isValidMove(Vector2.down))
                            {
                                moveVec = Vector2.down;
                            }
                        }
                    }
                    if (isValidMove(Vector2.left) && leftDistance < rightDistance && leftDistance < downDistance && leftDistance < upDistance)
                    {
                        if (moveVec != Vector2.right)
                        {
                            moveVec = Vector2.left;
                        }
                        else
                        {
                            if (upDistance <= downDistance && upDistance <= rightDistance && isValidMove(Vector2.up))
                            {
                                moveVec = Vector2.up;
                            }
                            if (downDistance <= upDistance && downDistance <= rightDistance && isValidMove(Vector2.down))
                            {
                                moveVec = Vector2.down;
                            }
                        }
                    }
                }
            }


            MoveChecker();

            if (myState == Statey.Chase || myState == Statey.Corner)
                moveTo(moveChecker, speed);
            else if (myState == Statey.Scared)
                moveTo(moveChecker, scaredSpeed);
            else if (myState == Statey.Eaten)
                moveTo(moveChecker, speed*2);
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

    public void Eaten()
    {
        print("i got eaten!");
        myState = Statey.Eaten;

        if(isValidMove(-moveVec))       //turn around when you get eaten -- looks nice.
        {
            moveVec = -moveVec;
        }
    }

    void MoveChecker()
    {
        if (moveChecker == (Vector2)transform.position)
        {
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
