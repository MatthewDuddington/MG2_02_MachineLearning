﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PacmanMovement : TileMove
{
    public enum PowerUp { NONE, GHOST };
    public int score = 0;
    public Vector2 moveVec2;
    Vector2 actualVec2;
    Rigidbody2D rb;
    Animator anim;
    float destTimer;

    public Transform destTransform;

    public Text moveVecText;
    public Text actualVecText;

    public float speed = 0.4f;
    // destination varaible, where pacman is going
    Vector3 dest = Vector3.zero;

    void Start()
    {
        dest = transform.position;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        print("ASDF");

        if (col.gameObject.tag == "Dot")
        {
            Destroy(col.gameObject);
            score += 10;
            transform.GetChild(0).GetComponent<AudioSource>().Play();
        }
    }

    void Update()
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

        if (transform.position.x < -15.5f)
        {
            transform.position = new Vector2(transform.position.x + 31f, transform.position.y);
            dest = new Vector3(Mathf.Round(transform.position.x) - 0.5f, Mathf.Round(transform.position.y), 0);
        }
        if (transform.position.x > 15.5f)
        {
            transform.position = new Vector2(transform.position.x - 31f, transform.position.y);
            dest = new Vector3(Mathf.Round(transform.position.x) + 0.5f, Mathf.Round(transform.position.y), 0);
        }
        //
        //visual guide for dest position -- debug only.
        destTransform.position = dest;


        //Input
        //raw input
        actualVec2 = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        actualVecText.text = "Actual: " + actualVec2;
        moveVecText.text = "MoveVec: " + moveVec2;

        //bug fix
        Vector2 dir = dest - this.transform.position;

        // recheck the keys for a new movement
        if (transform.position == dest)
        {

            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && isValidMove(Vector2.right))
            {
                moveVec2 = new Vector2(1, 0);
            }
            else if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && isValidMove(-Vector2.right))
            {
                moveVec2 = new Vector2(-1, 0);
            }
            else if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && isValidMove(Vector2.up))
            {
                moveVec2 = new Vector2(0, 1);
            }
            else if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && isValidMove(-Vector2.up))
            {
                moveVec2 = new Vector2(0, -1);
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