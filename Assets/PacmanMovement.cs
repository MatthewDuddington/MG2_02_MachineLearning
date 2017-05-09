using UnityEngine;
using System.Collections;

public class PacmanMovement : TileMove
{

    public enum PowerUp { NONE, GHOST };
    //public enum Direction { left, right, up, down, none };
    Vector2 moveVec2;
    Vector2 actualVec2;

    // speed variable to contorl how fast pacman moves
    public float speed = 0.4f;
    // destination varaible, where pacman is going
    Vector3 dest = Vector3.zero;
    //  private PowerUp powerup = Pacman.PowerUp.NONE;
    private int score = 0;

    void Start()
    {
        // set the destination to the current position (starting position) of pacman
        dest = transform.position;

    }

    void Update() 
    {
        //Input

        actualVec2 = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        moveTo(dest, speed);

        // recheck the keys for a new movement
        if (transform.position == dest)
        {
            if (this.isValidMove(moveVec2))
            {
                dest = this.transform.position + (Vector3)moveVec2;
            }
        }
        // Gets the offset from the destination to the current postion (change in direction)
        Vector2 dir = dest - this.transform.position;



        if (Input.GetKey(KeyCode.RightArrow) && isValidMove(Vector2.right))
        {
            moveVec2 = new Vector2(1, 0);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && isValidMove(-Vector2.right))
        {
            moveVec2 = new Vector2(-1, 0);

        }
        else if (Input.GetKey(KeyCode.UpArrow) && isValidMove(Vector2.up))
        {
            moveVec2 = new Vector2(0, 1);

        }
        else if (Input.GetKey(KeyCode.DownArrow)&& isValidMove(-Vector2.up))
        {
            moveVec2 = new Vector2(0, -1);

        }

    }
}
