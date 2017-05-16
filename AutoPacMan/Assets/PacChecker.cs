using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacChecker : MonoBehaviour {

    public bool[,] wallPerception;
    public GameObject[] checkers;
    public Transform[] emptyTiles;

    public Transform[] dots;
    public Transform closestDot;

    public Transform redGhost, pinkGhost, orangeGhost, blueGhost;

    public int windowSize = 7;

    void Start()
    {
        wallPerception = new bool[windowSize, windowSize];


    }
    Transform GetClosestEnemy(Transform[] enemies)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform t in enemies)
        {
            if (t.gameObject.active)
            {
                float dist = Vector3.Distance(t.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
        }
        return tMin;
    }

    public void GetNextClosestDot()
    {
        closestDot = GetClosestEnemy(dots);
    }

    public void WallCheck() //called by pacMovement;
    {
    
    int checkerIndex = 0;
    for (int y = 0; y < windowSize; y++) {
        for (int x = 0; x < windowSize; x++) {
            IndividualWallChecker checker = checkers [checkerIndex++].GetComponent<IndividualWallChecker>();
            
            // Update the wall presence bool array
            wallPerception [x, y] = checker.hitting;

            if (checkerIndex != windowSize * windowSize)
            {
               // print("poasfjaspodfjaspodfjzds");

                // Update the PerceptionInfo list of the 48 tiles surrounding PacMan
                int tileListIndex = checkerIndex - 1;
                if (checkerIndex > (windowSize * windowSize * 0.5) - 1) { checker = checkers[checkerIndex].GetComponent<IndividualWallChecker>(); }
                PerceptionInfo.Get.UpdateSurroundingDestinationTileList(tileListIndex, checker.positionOfTileUnderMe);
            }
        }
    }

        foreach (bool b in wallPerception)
        {
           // print(b);             heavy on pc!
        }
    }

    
    //EVERYTHING FOR AI CHECKING GOES IN HERE!!
}
