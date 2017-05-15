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


    void Start()
    {
        wallPerception = new bool[5, 5];


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
        // padre forgive me, for i have sinned.

        wallPerception[0, 0] = checkers[0].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[1, 0] = checkers[1].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[2, 0] = checkers[2].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[3, 0] = checkers[3].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[4, 0] = checkers[4].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[0, 1] = checkers[5].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[1, 1] = checkers[6].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[2, 1] = checkers[7].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[3, 1] = checkers[8].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[4, 1] = checkers[9].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[0, 2] = checkers[10].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[1, 2] = checkers[11].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[2, 2] = checkers[12].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[3, 2] = checkers[13].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[4, 2] = checkers[14].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[0, 3] = checkers[15].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[1, 3] = checkers[16].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[2, 3] = checkers[17].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[3, 3] = checkers[18].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[4, 3] = checkers[19].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[0, 4] = checkers[20].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[1, 4] = checkers[21].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[2, 4] = checkers[22].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[3, 4] = checkers[23].GetComponent<IndividualWallChecker>().hitting;
        wallPerception[4, 4] = checkers[24].GetComponent<IndividualWallChecker>().hitting;

        foreach (bool b in wallPerception)
        {
           // print(b);             heavy on pc!
        }
    }

    
    //EVERYTHING FOR AI CHECKING GOES IN HERE!!
}
