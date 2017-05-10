using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;

public class PacManLearningController : UnitController {
  bool IsRunning;
  IBlackBox box;

  public PerceptionMap myPerceptionMap;
  public PacmanMovement myPacmanMovement;
  public int windowWidth = 7;
  public int windowHeight = 7;

  // Use FixedUpdate to enable time jumping
  void FixedUpdate() {
    if (IsRunning) {
      ISignalArray inputArray = box.InputSignalArray;  // Number of inputs is defined in the Optimizer class

      Vector2 myGridPosition = new Vector2 (0, 0);  // TODO actually get correct position
      
      // Check a window of tiles around PacMan and set the percieved object into the inputs
      for (int x = 0; x < windowWidth; x++) {
        for (int y = 0; y < windowHeight; y++) {
          int gridPositionX = (int)(x + myGridPosition.x - ((windowWidth - 1) * 0.5f));
          int gridPositionY = (int)(y + myGridPosition.y - ((windowHeight - 1) * 0.5f));
          inputArray [x + (y * windowWidth)] = myPerceptionMap.ReadMap (gridPositionX, gridPositionY);
        }
      }

      box.Activate ();

      ISignalArray outputArray = box.OutputSignalArray;  // Number of outputs is defined in the Optimizer class

      if (outputArray [4] > outputArray [0] && outputArray [4] > outputArray [1] && outputArray [4] > outputArray [2] && outputArray [4] > outputArray [3]) {
        // myPacmanMovement.SetAiInputVector(new Vector4 (0,0,0,0);  // Don't need to turn this on, as it always resets anyway?
      } else if (outputArray [0] > outputArray [1] && outputArray [0] > outputArray [2] && outputArray [0] > outputArray [3]) {
        myPacmanMovement.SetAiInputVector (new Vector4 (1, 0, 0, 0));  // Signal right
      } else if (outputArray [1] > outputArray [2] && outputArray [1] > outputArray [3]) {
        myPacmanMovement.SetAiInputVector (new Vector4 (0, 1, 0, 0));  // Signal left
      } else if (outputArray [2] > outputArray [3]) {
        myPacmanMovement.SetAiInputVector (new Vector4 (0, 0, 1, 0));  // Signal up
      } else {
        myPacmanMovement.SetAiInputVector (new Vector4 (0, 0, 0, 1));  // Signal down
      }
    }
  }

  public override void Stop()
  {
    this.IsRunning = false;
  }

  public override void Activate(IBlackBox box)
  {
    this.box = box;
    this.IsRunning = true;
  }

  public override float GetFitness()
  {
    // Implement a meaningful fitness function here, for each unit.
    return 0;
  }
}
