using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManBrain : MonoBehaviour {

  static public PacManBrain Get;  // Easy access to the brain class instance (no singleton check)

  void Start () {
    if (Get != null) { Debug.LogError ("More than one PacManBrain in scene"); }
    else { Get = this; }
  }

  // Returns the [x,y] position for the next destination the neural network has determined for PacMan
  public Vector2 ChooseDestination() {
    // TODO return result from neural network prediction
    return new Vector2 (0, 0);
  }

}
