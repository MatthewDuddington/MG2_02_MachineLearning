using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallColourChanger : MonoBehaviour {

    public Color color;
    void Start()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().color = color;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Color zing = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
            foreach (Transform child in transform)
            {
                //makes a lightshow!!
              //  child.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f),1);

                //sets to 1 colour.
                child.GetComponent<SpriteRenderer>().color = zing;
            }
        }
    }
}
