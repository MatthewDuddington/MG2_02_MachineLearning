using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionInfo : MonoBehaviour {

  static PerceptionInfo Get;  // Easy access to the perception info class instance (no singleton check)

  Vector2[] ValidDestinationTiles;  // [x,y] positions of all possible tiles PacMan can choose to travel into


  // INPUT VECTORS
  // x    -1 Down to +1 Up      (Crunching between 25 and 1?)
  // y    -1 Left to +1 Right   (Crunching between 28 and 1?)
  // We could use inverse square to emphasise importance of closer proximities?
  Vector2 closestDot;
  Vector2 ghostA;
  Vector2 ghostB;
  Vector2 ghostC;
  Vector2 ghostD;

  bool[] WallPerceptionWindow;


  // ERROR INFO POINTS
  int totalTilesToDestination;
  int totalTilesEnteredOnWayToDestination;

  int totalDotsThatWereRemaining;
  int totalDotsEatenOnWayToDestination;


	void Start () {
    if (Get != null) { Debug.LogError ("More than one PerceptionInfo in scene"); }
    else { Get = this; }

    /*     // Switch for the two variations (try adding a second '/' at the start of this line)
    ValidDestinationTiles = new Vector2[298];  // 298 for whole map. Acts like a pure map destination.
    /*/
    ValidDestinationTiles = new Vector2[48];  // 48 if constrained to only destinations within the 7x7 perception window. Acts more like a compass heading.
    //*/
	}
	

	static void UpdatePerception () {
    // Refresh closest dot

    // Refresh ghosts positions

    // Refresh wall perception window bool array
  }


  void UpdateErrorCheckInfo() {
    // Refresh number of tiles to destination
    //SetTilesToDestination ( LukesAwesomeFunction() );

    // Refresh number of dots remaining
    //SetDotsRemaining ( LukesOtherAwesomeFunction() );
  }
  
  void SetTilesToDestination(int tilesToDestination) {
    totalTilesToDestination = tilesToDestination;
  }

  // Call from some external class to increase tiles entered counter
  void TileEntered() {
    totalTilesEnteredOnWayToDestination++;
  }
  
  void SetDotsRemaining(int dotsRemaining) {
    totalDotsThatWereRemaining = dotsRemaining;
  }

  // Call from some external class to increase dots eaten counter
  void DotEaten() {
    totalDotsEatenOnWayToDestination++;
  }

}
