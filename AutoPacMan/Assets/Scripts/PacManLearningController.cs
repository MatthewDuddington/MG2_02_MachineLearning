using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;

public class PacManLearningController : UnitController {
  bool IsRunning;
  IBlackBox box;

  public PerceptionMap myPerceptionMap;
  public int windowWidth = 7;
  public int windowHeight = 7;

  // Use FixedUpdate to enable time jumping
  void FixedUpdate() {
    if (IsRunning) {
      Vector2 myGridPosition = new Vector2 (0, 0);  // TODO actually get correct position

      ISignalArray inputArray = box.InputSignalArray;

      // Check a window of tiles around PacMan and set the percieved object into the inputs
      for (int x = 0; x < windowWidth; x++) {
        for (int y = 0; y < windowHeight; y++) {
          int gridPositionX = (int)(x + myGridPosition.x - ((windowWidth - 1) * 0.5f));
          int gridPositionY = (int)(y + myGridPosition.y - ((windowHeight - 1) * 0.5f));
          inputArray[x + (y * windowWidth)] = myPerceptionMap.ReadMap(gridPositionX, gridPositionY);
        }
      }

      box.Activate();

      ISignalArray outputArray = box.OutputSignalArray;
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
