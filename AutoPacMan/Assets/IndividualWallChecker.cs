using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualWallChecker : MonoBehaviour {

    public Sprite good;
    public Sprite bad;
    public bool hitting;
    SpriteRenderer spr;
    public Transform[] walls;

    public Vector2 positionOfTileUnderMe;

    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    /*public void Check()
    {
        foreach (Transform w in walls)
        {
            if (transform.position == w.position)
            {
                print("ASDASDDSAD");
                hitting = true;
                spr.sprite = good;

            }
            else
            {
                print("ASDASDDSAD");
                hitting = false;
                spr.sprite = bad;
            }
        }
    }
    void OnEnable()
    {
        foreach (Transform w in walls)
        {
            if (transform.position == w.position)
            {
                print("hit");
                hitting = true;
                spr.sprite = good;

            }
            else
            {
               // print("no hit");
                hitting = false;
                spr.sprite = bad;
            }
        }
    }
     * */
    void OnTriggerExit2D(Collider2D col)
    {

        if (col.gameObject.tag == "Wall")
        {
            hitting = false;
            spr.sprite = bad;
        }

        outsideMap ();

    }
    void OnTriggerStay2D(Collider2D col)
    {

        if (col.gameObject.tag == "Wall")
        {
            hitting = true;
            spr.sprite = good;
        }

        positionOfTileUnderMe = col.transform.position;

    }

    void outsideMap() {
        positionOfTileUnderMe = Vector2.one * - 1;
    }
}
