using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheckerRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.rotation = Quaternion.Euler(0, 0, 0);
	}

    public void StartCheck()
    {
       // foreach (Transform child in transform)
        {
       //     child.GetComponent<IndividualWallChecker>().Check();
        }
    }
}
