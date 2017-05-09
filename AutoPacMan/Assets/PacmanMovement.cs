using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PacmanMovement : TileMove
{

    public enum PowerUp { NONE, GHOST };
    //public enum Direction { left, right, up, down, none };
    Vector2 moveVec2;
    Vector2 actualVec2;
    Rigidbody2D rb;
    Animator anim;

    public Transform destTransform;

    public Text moveVecText;
    public Text actualVecText;

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
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    void Update() 
    {
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
            anim.Play("Pac_Move");
        }
        else
        {
            anim.Play("Pac_Idle");
        }
        destTransform.position = dest;
        //Input

        //raw input
        actualVec2 = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        actualVecText.text = "Actual: "+actualVec2;


        moveVecText.text = "MoveVec: "+moveVec2;

        Vector2 dir = dest - this.transform.position;

        // recheck the keys for a new movement
        if (transform.position == dest)
        {
            if (true)
            {
                print("work");
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
                else if (Input.GetKey(KeyCode.DownArrow) && isValidMove(-Vector2.up))
                {
                    moveVec2 = new Vector2(0, -1);
                }

            }
            if (isValidMove(moveVec2 + dir))
            {
                dest = this.transform.position + (Vector3)moveVec2;

            }
        }
        // Gets the offset from the destination to the current postion (change in direction)


        
        moveTo(dest, speed);

    }
}
