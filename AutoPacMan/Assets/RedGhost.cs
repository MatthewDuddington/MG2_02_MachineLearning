using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGhost : TileMove
{

    public Vector3 pacDest;

    public Vector3 myDest;
    public Vector3 movementDest;
    public Vector2 moveVec2 = new Vector2(0, 1);


    public Transform pacDestTransform;
    public Transform myDestTransform;
    public Transform myMovementDestTransform;

    public Transform cherry;

    Vector2 upCheck;
    Vector2 downCheck;
    Vector2 rightCheck;
    Vector2 leftCheck;

    float upCheckDistance;
    float rightCheckDistance;
    float downCheckDistance;
    float leftCheckDistance;



    public float speed = 0.18f;

    public enum Statey { Chase, Corner, Scared };
    public Statey myState;
    // Use this for initialization
    void Start()
    {
        myDest = transform.position;
        movementDest = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        cherry.position = leftCheck;
        upCheck = Vector2.up;//new Vector2(myMovementDestTransform.position.x, myMovementDestTransform.position.y + 1);
        downCheck = Vector2.down;//new Vector2(myMovementDestTransform.position.x, myMovementDestTransform.position.y - 1);
        rightCheck = Vector2.right;//new Vector2(myMovementDestTransform.position.x + 1, myMovementDestTransform.position.y);
        leftCheck = Vector2.left;//new Vector2(myMovementDestTransform.position.x - 1, myMovementDestTransform.position.y);

        upCheckDistance = Vector2.Distance(upCheck, myDestTransform.position);
        rightCheckDistance = Vector2.Distance(rightCheck, myDestTransform.position);
        downCheckDistance = Vector2.Distance(downCheck, myDestTransform.position);
        leftCheckDistance = Vector2.Distance(leftCheck, myDestTransform.position);

        print("UP: " + isValidMove(upCheck) + ", DOWN: " + isValidMove(downCheck) + ", LEFT: " + isValidMove(leftCheck) + ", RIGHT: " + isValidMove(rightCheck));

        if (transform.position == movementDest)// && isValidMove(moveVec2))
        {
            print("checking next move...");
            {
                //if at intersection...
                if (isValidMove(upCheck) && isValidMove(downCheck) && isValidMove(rightCheck) && isValidMove(leftCheck))
                {
                    print("at intersection");
                    if ((upCheckDistance <= rightCheckDistance) && (upCheckDistance <= leftCheckDistance))
                    {
                        moveVec2 = new Vector2(0, 1);
                        movementDest = this.transform.position + new Vector3(0, 1, 0);
                    }
                    if ((downCheckDistance <= rightCheckDistance) && (downCheckDistance <= leftCheckDistance))
                    {
                        moveVec2 = new Vector2(0, -1);
                        movementDest = this.transform.position + new Vector3(0, -1, 0);
                    }
                    if ((rightCheckDistance <= upCheckDistance) && (rightCheckDistance <= downCheckDistance))
                    {
                        moveVec2 = new Vector2(1, 0);
                        movementDest = this.transform.position + new Vector3(1, 0, 0);
                    }
                    if ((leftCheckDistance <= upCheckDistance) && (leftCheckDistance <= downCheckDistance))
                    {
                        moveVec2 = new Vector2(-1, 0);
                        movementDest = this.transform.position + new Vector3(-1, 0, 0);
                    }

                }
                if ((upCheckDistance <= rightCheckDistance) &&
                    (upCheckDistance <= leftCheckDistance)

                     &&

                    !isValidMove(leftCheck) && !isValidMove(rightCheck))
                {
                    {
                        print("UP");
                        if (isValidMove(upCheck))
                        {
                            moveVec2 = new Vector2(0, 1);
                            movementDest = this.transform.position + new Vector3(0, 1, 0);
                        }
                    }
                }

                else if ((downCheckDistance <= rightCheckDistance) &&
                     (downCheckDistance <= leftCheckDistance)

                    &&

                    !isValidMove(leftCheck) && !isValidMove(rightCheck))
                {
                    {
                        print("DOWN");
                        if (isValidMove(downCheck))
                        {
                            moveVec2 = new Vector2(0, -1);
                            movementDest = this.transform.position + new Vector3(0, -1, 0);
                        }
                    }
                }

                else if ((rightCheckDistance <= upCheckDistance) &&
                    (rightCheckDistance <= downCheckDistance)

                     &&

                    !isValidMove(upCheck) && !isValidMove(downCheck))
                {
                    {
                        print("RIGHT");
                        if (isValidMove(rightCheck))
                        {
                            moveVec2 = new Vector2(1, 0);
                            movementDest = this.transform.position + new Vector3(1, 0, 0);
                        }
                    }
                }

                else if ((leftCheckDistance <= upCheckDistance) &&
                    (leftCheckDistance <= downCheckDistance)

                     &&

                    !isValidMove(upCheck) && !isValidMove(downCheck))
                {
                    {
                        print("LEFT");
                        if (isValidMove(leftCheck))
                        {
                            moveVec2 = new Vector2(-1, 0);
                            movementDest = this.transform.position + new Vector3(-1, 0, 0);
                        }
                    }
                }
            }
        }


        pacDest = pacDestTransform.position;

        myMovementDestTransform.position = movementDest;


        if (myState == Statey.Chase)
        {
            myDest = pacDest;
            //  moveTo(myDest, speed);
        }
        if (myState == Statey.Corner)
        {
            myDest = myDestTransform.position; //puts my dest in the UP - RIGHT corner
        }

        moveTo(movementDest, speed);


    }
}
