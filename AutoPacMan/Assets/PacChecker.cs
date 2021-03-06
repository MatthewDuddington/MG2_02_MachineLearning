﻿//
//  Code and comments (c) Luke Sanderson & Matthew Duddington 2017
//
////////////////////////////////////////////////////////////////////////////////

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

    private Vector2 sizeOfMap = new Vector2 (28, 31);

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
            if (t.gameObject.activeSelf)
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
  
    public double DetermineNormalisedHorizontal(Transform otherTile) {
        Vector2 otherPos = otherTile.transform.position;
        Vector2 pacPos = transform.position;
        
        double differenceX = pacPos.x - otherPos.x;
        // If diff is -ve then the other must be right of pacman
        
        // Return as proportion of max possible horizontal offset - no minus 2 here because of the extra tunnel exit tiles on each side 
        return differenceX / (sizeOfMap.x);
    }

    public double DetermineNormalisedVertical(Transform otherTile) {
        Vector2 otherPos = otherTile.transform.position;
        Vector2 pacPos = transform.position;

        double differenceY = pacPos.y - otherPos.y;
        // If diff is -ve then the other must be above pacman

        // Return as proportion of max possible vertical offset i.e. 2 less than the height of the map
        return differenceY / (sizeOfMap.y - 2);
    }

    public void ClosestDotCheck()
    {
        closestDot = GetClosestEnemy(dots);
        
        // Directly update perception values - ugh sorry not encapsulated :(
        PerceptionInfo.Get.closestDotHorizontal = DetermineNormalisedHorizontal (closestDot);
        PerceptionInfo.Get.closestDotVertical = DetermineNormalisedVertical (closestDot);
    }
  
    public void GhostsCheck()
    {
        PerceptionInfo.Get.ghostAHorizontal = DetermineNormalisedHorizontal (redGhost);
        PerceptionInfo.Get.ghostAVertical = DetermineNormalisedVertical (redGhost);
        PerceptionInfo.Get.ghostBHorizontal = DetermineNormalisedHorizontal (pinkGhost);
        PerceptionInfo.Get.ghostBVertical = DetermineNormalisedVertical (pinkGhost);
        PerceptionInfo.Get.ghostCHorizontal = DetermineNormalisedHorizontal (orangeGhost);
        PerceptionInfo.Get.ghostCVertical = DetermineNormalisedVertical (orangeGhost);
        PerceptionInfo.Get.ghostDHorizontal = DetermineNormalisedHorizontal (blueGhost);
        PerceptionInfo.Get.ghostDVertical = DetermineNormalisedVertical (blueGhost);
    }

    public void WallCheck() //called by pacMovement;
    {
        float middleValue = windowSize * windowSize * 0.5f;
        int checkerIndex = 0;

        bool[] wallPerceptionClippedForInputs = new bool[(windowSize * windowSize) - 1];

        for (int y = 0; y < windowSize; y++) {
            for (int x = 0; x < windowSize; x++) {
                // Get a reference to the next wall checker
                IndividualWallChecker checker = checkers [checkerIndex].GetComponent<IndividualWallChecker>();
                
                // Update the wall presence or lack of
                wallPerception [x, y] = checker.hitting;

                // Ignoring the pacman tile
                //
                if (checkerIndex < middleValue - 1)
                {
                    wallPerceptionClippedForInputs[checkerIndex] = wallPerception[x,y];
//                    PerceptionInfo.Get.UpdateSurroundingDestinationTileList(checkerIndex, checker.positionOfTileUnderMe);
                }
                else if (checkerIndex > middleValue)
                {
                    wallPerceptionClippedForInputs[checkerIndex - 1] = wallPerception[x,y];
//                    PerceptionInfo.Get.UpdateSurroundingDestinationTileList(checkerIndex - 1, checker.positionOfTileUnderMe);
                }

                checkerIndex++;        
            }
        }
        
        PerceptionInfo.Get.WallPerceptionWindow = wallPerceptionClippedForInputs;
    }

    
    //EVERYTHING FOR AI CHECKING GOES IN HERE!!
}
