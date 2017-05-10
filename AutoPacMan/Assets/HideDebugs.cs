using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideDebugs : MonoBehaviour {

    public LineRenderer lr;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<SpriteRenderer>().enabled = !child.GetComponent<SpriteRenderer>().enabled;
                lr.enabled = !lr.enabled;
            }

        }
		
	}
}
