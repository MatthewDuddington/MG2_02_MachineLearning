using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionInfo : MonoBehaviour {

  static public PerceptionInfo Get;  // Easy access to the perception info class instance (no singleton check)

  private Vector2[] ValidDestinationTiles;  // [x,y] positions of all possible tiles PacMan can choose to travel into


  // INPUT VECTORS
  // x    -1 Down to +1 Up      (Crunching between 25 and 1?)
  // y    -1 Left to +1 Right   (Crunching between 28 and 1?)
  // We could use inverse square to emphasise importance of closer proximities?
  private Vector2 closestDot;
  private Vector2 ghostA;
  private Vector2 ghostB;
  private Vector2 ghostC;
  private Vector2 ghostD;

  private bool[] WallPerceptionWindow;


  // ERROR INFO POINTS
  public int totalTilesToDestination;
  public int totalTilesSurvivedOnWayToDestination;

  public int totalDotsThatWereRemaining;
  public int totalDotsEatenOnWayToDestination;


	void Start () {
    if (Get != null) { Debug.LogError ("More than one PerceptionInfo in scene"); }
    else { Get = this; }

    /*     // Switch for the two variations (try adding a second '/' at the start of this line)
    ValidDestinationTiles = new Vector2[298];  // 298 for whole map. Acts like a pure map destination.
    /*/
    ValidDestinationTiles = new Vector2[48];  // 48 if constrained to only destinations within the 7x7 perception window. Acts more like a compass heading.
    //*/
	}
	

	public static void UpdatePerception () {
    // Refresh closest dot

    // Refresh ghosts positions

    // Refresh wall perception window bool array
  }


  public void UpdateErrorCheckInfo() {
    // Refresh number of tiles to destination
    //SetTilesToDestination ( LukesAwesomeFunction() );

    // Refresh number of dots remaining
    //SetDotsRemaining ( LukesOtherAwesomeFunction() );
  }
  
  private void SetTilesToDestination(int tilesToDestination) {
    totalTilesToDestination = tilesToDestination;
  }

  // Call from some external class to increase tiles entered counter
  public void TileSurvived() {
    totalTilesSurvivedOnWayToDestination++;
  }
  
  private void SetDotsRemaining(int dotsRemaining) {
    totalDotsThatWereRemaining = dotsRemaining;
  }

  // Call from some external class to increase dots eaten counter
  public void DotEaten() {
    totalDotsEatenOnWayToDestination++;
  }

  public double[] Walls {
    get { 
      double[] returnData = new double[WallPerceptionWindow.Length];
      for (int i = 0; i < WallPerceptionWindow.Length; i++) {
        if (WallPerceptionWindow [i]) { returnData [i] = 1.0; }
        else { returnData [i] = 0.0; }
      }
      return returnData;
    }
  }

  public double[] Dot {
    get {
      double[] returnData = new double[2];
      returnData [0] = closestDot.x;
      returnData [1] = closestDot.y;
      return returnData;
    }
  }

  public double[] Ghosts {
    get {
      double[] returnData = new double[8];
      returnData [0] = ghostA.x;
      returnData [1] = ghostA.y;
      returnData [2] = ghostB.x;
      returnData [3] = ghostB.y;
      returnData [4] = ghostC.x;
      returnData [5] = ghostC.y;
      returnData [6] = ghostD.x;
      returnData [7] = ghostD.y;
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
