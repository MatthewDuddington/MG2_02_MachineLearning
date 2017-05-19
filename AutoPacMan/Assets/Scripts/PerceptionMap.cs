/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionMap : MonoBehaviour {

  public enum perceptableObject {
    Empty = 0,
    Wall = -1,
    PacMan = 0,
    Dot = 1,
    Power = 2,
    Ghost = -5
  };

  public int mapSizeX = 28;
  public int mapSizeY = 31;
  public GameObject mapParent;

  private float tileHalfSize = 0.5f;

  private float mapOffsetX;
  private float mapOffsetY;
  
  private int[,] map;

  void Start() {
    map = new int[mapSizeX, mapSizeY];  // Setup map memory space to fit given size

    // Create offsets which take into account non 0,0 position of top left tile of map
    mapOffsetX = -(mapSizeX * 0.5f) + tileHalfSize + mapParent.transform.position.x;
    mapOffsetY = (mapSizeY * 0.5f) + tileHalfSize + mapParent.transform.position.y;

    BuildMap ();
  }

  private void BuildMap() {
    // Collision check each grid reference and record the tile type found there
    for (int x = 0; x < mapSizeX; x++) {
      for (int y = 0; y < mapSizeY; y++) {
        Vector2 gridPoint = new Vector2 (x + mapOffsetX, mapOffsetY - y);  // Checking top left to bottom right
        Collider2D tileCollider = Physics2D.OverlapPoint(gridPoint);
        if (tileCollider != null) {
          // Set map space to type of tile found by tag
          switch (tileCollider.tag) {
          case "Wall":
            map [x, y] = (int)perceptableObject.Wall;
            break;
          case "PacMan":
            map [x, y] = (int)perceptableObject.PacMan;
            break;
          case "Dot":
            map [x, y] = (int)perceptableObject.Dot;
            break;
          case "Power":
            map [x, y] = (int)perceptableObject.Power;
            break;
          case "Ghost":
            map [x, y] = (int)perceptableObject.Ghost;
            break;
          }
        }
        else {
          // If no collider hit, then assume empty map space
          map[x,y] = (int)perceptableObject.Empty;
        }

        print(map[x,y]);
      }
    }
  }

  // Function for the Neural Net to read in input data from in the form of an int representation
  public int ReadMap(int x, int y) {
    if (x > mapSizeX - 1 || y > mapSizeY - 1) { Debug.LogError ("Map reference attempting to be read is out of bounds"); }
    return map [x, y];
  }

//  public perceptableObject ReadMapTileType(int x, int y) {
//    if (x > mapSizeX - 1 || y > mapSizeY - 1) { Debug.LogError ("Map reference attempting to be read is out of bounds"); }
//    return 
//  }

  public void DeclarePosition(Vector2 objectPosition, perceptableObject objectType) {
    //TODO Calculate the grid position of the object based on its world position
    // Can the PacMan and Ghosts provide a grid tile centre position to this to negate their movement offsets?
    int gridReferenceX = 0;
    int gridReferenceY = 0;

    map [gridReferenceX, gridReferenceY] = (int)objectType;
  }

}

*/
