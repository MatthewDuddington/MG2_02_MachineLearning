using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionInfo : MonoBehaviour {

  static public PerceptionInfo Get;  // Easy access to the perception info class instance (no singleton check)

  private PacChecker pacChecker;

  private Vector2[] ValidDestinationTiles;  // [x,y] positions of all possible tiles PacMan can choose to travel into


  // INPUT VECTORS
  // x    -1 Down to +1 Up      (Crunching between 25 and 1?)
  // y    -1 Left to +1 Right   (Crunching between 28 and 1?)
  // We could use inverse square to emphasise importance of closer proximities?
  public double closestDotHorizontal;
  public double closestDotVertical;
  public double ghostAHorizontal;
  public double ghostAVertical;
  public double ghostBHorizontal;
  public double ghostBVertical;
  public double ghostCHorizontal;
  public double ghostCVertical;
  public double ghostDHorizontal;
  public double ghostDVertical;

  public bool[,] WallPerceptionWindow;


  // ERROR INFO POINTS
  public int totalTilesToDestination;
  public int totalTilesSurvivedOnWayToDestination;

  public int totalDotsThatWereRemaining;
  public int totalDotsEatenOnWayToDestination;


	void Start () {
    if (Get != null) { Debug.LogError ("More than one PerceptionInfo in scene"); }
    else { Get = this; }

    pacChecker = GameObject.FindObjectOfType<PacChecker> ();

    /*     // Switch for the two variations (try adding a second '/' at the start of this line)
    ValidDestinationTiles = new Vector2[298];  // 298 for whole map. Acts like a pure map destination.
    /*/
    ValidDestinationTiles = new Vector2[48];  // 48 if constrained to only destinations within the 7x7 perception window. Acts more like a compass heading.
    //*/
	}
	

	public void UpdatePerception () {
    // Refresh closest dot
    pacChecker.ClosestDotCheck();

    // Refresh ghosts positions
    pacChecker.GhostsCheck();

    // Refresh wall perception window bool array
    pacChecker.WallCheck();
  }


//  public void UpdateErrorCheckInfo() {
//    // Refresh number of tiles to destination
//    //SetTilesToDestination ( LukesAwesomeFunction() );
//
//    // Refresh number of dots remaining
//    //SetDotsRemaining ( LukesOtherAwesomeFunction() );
//  }

  // Set from MagicPac class
  public void SetTilesToDestination(int tilesToDestination) {
    totalTilesToDestination = tilesToDestination;
  }

  // Call from some external class to increase tiles entered counter
  public void TileSurvived() {
    totalTilesSurvivedOnWayToDestination++;
  }

  // Set from MagicPac class
  public void SetDotsRemaining(int dotsRemaining) {
    totalDotsThatWereRemaining = dotsRemaining;
  }

  // Call from some external class to increase dots eaten counter
  public void DotEaten() {
    totalDotsEatenOnWayToDestination++;
  }

  public double[] Walls {
    get { 
      double[] returnData = new double[WallPerceptionWindow.Length];
      int returnDataIndex = 0;
      foreach (bool isWall in WallPerceptionWindow) {
        if (isWall) { returnData [returnDataIndex++] = 1.0; }
        else { returnData [returnDataIndex++] = 0.0; }
      }
      return returnData;
    }
  }

  public double[] Dot {
    get {
      double[] returnData = new double[2];
      returnData [0] = closestDotHorizontal;
      returnData [1] = closestDotVertical;
      return returnData;
    }
  }

  public double[] Ghosts {
    get {
      double[] returnData = new double[8];
      returnData [0] = ghostAHorizontal;
      returnData [1] = ghostAVertical;
      returnData [2] = ghostBHorizontal;
      returnData [3] = ghostBVertical;
      returnData [4] = ghostCHorizontal;
      returnData [5] = ghostCVertical;
      returnData [6] = ghostDHorizontal;
      returnData [7] = ghostDVertical;
      return returnData;
    }
  }

  public Vector2 GetValidDestination(int destinationIndex) {
    return ValidDestinationTiles [destinationIndex];
  }

  public void UpdateSurroundingDestinationTileList(int tileIndex, Vector2 tilePosition) {
    ValidDestinationTiles[tileIndex] = tilePosition;
  }

}
