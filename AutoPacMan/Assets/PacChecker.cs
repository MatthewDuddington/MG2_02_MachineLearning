using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacChecker : MonoBehaviour {

    bool[,] wallPerception;

    void Start()
    {
        wallPerception = new bool[5, 5];
    }

    public void WallCheck() //called by pacMovement;
    {

    }
    //EVERYTHING FOR AI CHECKING GOES IN HERE!!
}
