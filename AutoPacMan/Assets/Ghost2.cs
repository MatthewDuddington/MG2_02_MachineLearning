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

    public enum Statey { Chase, Corner, Scared };
    public Statey myState;


    [Header("Movement / Targets")]
    public Transform pacDest;
    public Vector2 targetTile;
    public Transform targetTileGraphic;
    public Vector2 moveChecker;
    public Transform moveCheckerGraphic;


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

            if (myState != Statey.Scared)
            {
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
            else
            {
                anim.Play("Red_Scared_Up");
            }
        }
        if (myGhostColour == GhostColour.Pink)
        {
            if (myState == Statey.Chase)
            {
                targetTileGraphic.position = targetTile = (Vector2)pacDest.position + (Pacman.moveVec2*4);
            }
            if (myState == Statey.Corner)
            {
                targetTileGraphic.position = targetTile = new Vector2(-12.5f, 20f);
            }

            if (myState != Statey.Scared)
            {
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
            else
            {
                anim.Play("Red_Scared_Up");
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
                targetTileGraphic.position = targetTile = -disty+midPoint;
            }
            if (myState == Statey.Corner)
            {
                targetTileGraphic.position = targetTile = new Vector2(12.5f, -12f);
            }

            if (myState != Statey.Scared)
            {
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
            else
            {
                anim.Play("Red_Scared_Up");
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

            if (myState != Statey.Scared)
            {
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
            else
            {
                anim.Play("Red_Scared_Up");
            }
        }
        if (myState == Statey.Scared)
        {
            targetTileGraphic.position = targetTile = new Vector2(Random.Range(-12f,12f),Random.Range(-10f,18f));
        }

        moveCheckerGraphic.position = moveChecker;

        upDistance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y + 1), targetTile);
        downDistance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y - 1), targetTile);
        leftDistance = Vector2.Distance(new Vector2(transform.position.x - 1, transform.position.y), targetTile);
        rightDistance = Vector2.Distance(new Vector2(transform.position.x + 1, transform.position.y), targetTile);

        for (int i = 0; i < intersections.Length; i++)
        {
            if (transform.position == intersectionsPos[i].transform.position)
            {
                print("HIT AN INTERSECTION");

                if (isValidMove(Vector2.up) && upDistance < rightDistance && upDistance < downDistance && upDistance < leftDistance)
                {
                    print("go up!");
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
                    print("go down!");
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
                    print("go right!");
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
                    print("go left!");
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

        if (myState != Statey.Scared)
            moveTo(moveChecker, speed);
        else
            moveTo(moveChecker, scaredSpeed);
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
