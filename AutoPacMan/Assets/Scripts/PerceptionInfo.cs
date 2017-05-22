//
//  Code and comments (c) Matthew Duddington 2017
//
////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionInfo : MonoBehaviour {

  static public PerceptionInfo Get;  // Easy access to the perception info class instance (no singleton check)

  private PacChecker pacChecker;

  private Vector2[] TilesSurroundingPacMan;  // [x,y] positions of all possible tiles PacMan can choose to travel into


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

  public bool[] WallPerceptionWindow;

  public double[] playerTrainingInput;


  // ERROR INFO POINTS
  public int totalTilesToDestination;
  public int totalTilesSurvivedOnWayToDestination;

  public int totalDotsThatWereRemaining;
  public int totalDotsEatenOnWayToDestination;


	void Start () {
    if (Get != null) { Debug.LogError ("More than one PerceptionInfo in scene"); }
    else { Get = this; }

    pacChecker = GameObject.FindObjectOfType<PacChecker> ();

    TilesSurroundingPacMan = new Vector2[48];  // 48 if constrained to only destinations within the 7x7 perception window. Acts more like a compass heading.

    if (PacManBrain.Get.activeBrainmode == PacManBrain.BrainMode.SupervisedTraining) {
      playerTrainingInput = new double[4];
    }
	}
	

	public void UpdatePerception () {
    // Refresh closest dot
    pacChecker.ClosestDotCheck();

    // Refresh ghosts positions
    pacChecker.GhostsCheck();

    // Refresh wall perception window bool array
    pacChecker.WallCheck();

    // Refresh position record of tiles surrounding pacman
    UpdateSurroundingTiles();
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
    Vector2 newDestination = Vector2.zero;

    // If the proposed destination is a tile with a wall in it, search for the nearest tile that is empty
    if (WallPerceptionWindow[destinationIndex]) {
//      Debug.Log("Proposed tile " + destinationIndex + " had wall. Searching for next best tile...");

      Vector2 testDestination = TilesSurroundingPacMan [destinationIndex];
      Vector2 nextNearestPos = new Vector2(-100, - 100);  // Instantiate with obvious wrong value to check for bugs if not overwritten
      int nextNearestIndex = 0;
      float minDist = Mathf.Infinity;

      for (int i = 0; i < TilesSurroundingPacMan.Length; i++)  // For each possible destination tile surrounding PacMan...
      {
        float dist = Vector2.Distance(TilesSurroundingPacMan[i], testDestination);  // Get the distance between that tile and the original suggested destination
//        Debug.Log("Tile index " + i + "and pos " + TilesSurroundingPacMan[i] + " with distance of " + dist + " is there a wall..." + WallPerceptionWindow[i]);
        if ( dist < minDist  // If this is the smallest distance yet...
          && WallPerceptionWindow[i] != true)  // ...and the tile is not a wall...
        {
//          Debug.Log("Proposing tile: " + i);
          nextNearestPos = TilesSurroundingPacMan[i];  // Propose this as the next best tile
          nextNearestIndex = i;
          minDist = dist;  // Update the smallest distance found
        }
      }

      // Return the closest tile position which didnt have a wall
//      Debug.Log("Position aiming for is index " + nextNearestIndex + " pos " + nextNearestPos);
      newDestination = nextNearestPos;
    }
    else {
      // Otherwise the original proposed tile is fine, so just return it
//      Debug.Log("Proposed tile is valid...");
      newDestination = TilesSurroundingPacMan [destinationIndex];
    }

    if (newDestination.x > 14) {
      newDestination.x -= 28;
    }
    else if (newDestination.x < -14) {
      newDestination.x += 28;
    }

    return newDestination;
  }

  public void UpdateSurroundingDestinationTileList(int tileIndex, Vector2 tilePosition) {
//    TilesSurroundingPacMan[tileIndex] = tilePosition;
  }

  private void UpdateSurroundingTiles() {
    int tileListIndex = 0;

    for (int y = 3; y > -4; y--) {
      for (int x = -3; x < 4; x++) {
        bool isMiddle = false;
        if (x == 0 && y == 0) { 
          isMiddle = true;
        }
        if (!isMiddle) {
          float xPos = pacChecker.transform.position.x + x;
          float yPos = pacChecker.transform.position.y + y;
          TilesSurroundingPacMan[tileListIndex] = new Vector2(xPos, yPos);
//          Debug.Log(TilesSurroundingPacMan[tileListIndex]);
          tileListIndex++;
        }
      }
    }
  }

  public double[] GetPlayerTrainingInput() {
//    Vector2 inputVector;

    if (Input.GetAxisRaw("Horizontal") < 0) {
//      inputVector = Vector2.left;
      return new double[] { 1,0,0,0 };
    }
    else if (Input.GetAxisRaw("Horizontal") > 0) {
//      inputVector = Vector2.right;
      return new double[] { 0,1,0,0 };
    }
    else if (Input.GetAxisRaw("Vertical") > 0) {
//      inputVector = Vector2.up;
      return new double[] { 0,0,1,0 };
    }
    else if (Input.GetAxisRaw("Vertical") < 0) {
//      inputVector = Vector2.down;
      return new double[] { 0,0,0,1 };
    }

//    if (inputVector.y == 0) {
//      if (inputVector.x == Vector2.left.x) {
//        return new double[] { 1,0,0,0 };
//      }
//      else if (inputVector.x == Vector2.right.x) {
//        return new double[] { 0,1,0,0 };
//      }
//    }
//    else if(inputVector.x == 0) {
//      if (inputVector.y == Vector2.up.y) {
//        return new double[] { 0,0,1,0 };
//      }
//      else if(inputVector.y == Vector2.down.y) {
//        return new double[] { 0,0,0,1 };
//      }
//    }
    else {
      return null;
    }
  }

}
