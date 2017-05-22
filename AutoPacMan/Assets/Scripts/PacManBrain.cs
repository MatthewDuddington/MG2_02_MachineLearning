//
//  Code and comments (c) Matthew Duddington 2017
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class PacManBrain : MonoBehaviour {

  public enum BrainMode { UnsupervisedTraining, SupervisedTraining, PlayLikeMe }
  public BrainMode activeBrainmode { get; private set; }

  public BrainMode brainMode = BrainMode.UnsupervisedTraining;

  static public PacManBrain Get;  // Easy access to the brain class instance (no singleton check)

  public PacMasterControl pacMasterControl;

  public bool networkIsEnabled { get; private set; }

  private NeuralNetwork nn;

  private int predictedDirectionIndex;  // Stores in case we need to choose a sub prediction

  public int seed = 25;
  public int generationToLoad;
  public string userIdForTrainingData = "guestUserName";

  public int displayIterations;
  
  public double learnRate = 0.05;
  public double momentum = 0.01;
  public double weightDecay = 0.0001;
  
  private int numInput;
  private int numHidden;
  private int numOutput;
  
  private double[] inputArray;
  private bool firstInputBuild = true;

  private static string dataPath = string.Empty;
  public float delayBetweenSaves = 1.0f;

  // Set the saved data location
  void Awake() {
    if (Get != null) { Debug.LogError ("More than one PacManBrain in scene"); }
    else { Get = this; }

    activeBrainmode = brainMode;

    // Setup network with values for correct mode
    if (activeBrainmode == BrainMode.UnsupervisedTraining) {
      dataPath = System.IO.Path.Combine (Application.dataPath, "Resources/unsupervisedLearning_generations.xml");
      numInput = 58;
      numHidden = 58;
      numOutput = 48;
    }
    else if (activeBrainmode == BrainMode.SupervisedTraining || activeBrainmode == BrainMode.PlayLikeMe) {
      dataPath = System.IO.Path.Combine (Application.dataPath, "Resources/supervisedLearning_generations_" + userIdForTrainingData + ".xml");
      numInput = 58;
      numHidden = 58;
      numOutput = 4;
    }

    inputArray = new double[numInput];
  }

  void Start() {
    if (activeBrainmode == BrainMode.PlayLikeMe) {
      LoadNetwork();
    }
    else if (activeBrainmode == BrainMode.SupervisedTraining) {
      LoadNetwork();
      // In supervised training mode only save the network values to xml file every once in a while
      StartCoroutine(PeriodicalylSaveNetwork());
    }
    else if (activeBrainmode == BrainMode.UnsupervisedTraining) {
//      GameObject.FindObjectOfType<PacMasterControl>().SwitchMode();  // Start AI mode
      LoadNetwork();
    }
  }

  void Update() {
    // If ungergoing supervised training learn from every frame
    if (activeBrainmode == BrainMode.SupervisedTraining && nn != null) {

      // Get the current player input (as a network-output comparison-ready double array)
      double[] playerTrainingInput = PerceptionInfo.Get.GetPlayerTrainingInput();

      // If the player isn't pressing any key just ignore and dont update learning
      if (playerTrainingInput != null) {
//        print("Training input: " + playerTrainingInput[0] + ", " + playerTrainingInput[1] + ", " + playerTrainingInput[2] + ", " + playerTrainingInput[3]);
        nn.MakePrediction(BuildInputs());
        nn.UpdateWeights(playerTrainingInput, learnRate, momentum, weightDecay);
        nn.SaveLearnedValues();

        displayIterations = nn.savedData.iterations;

        // Start a new generation only every n data points to prevent file from ballooning
//        if (nn.savedData.iterations > 300) {
//          nn.savedData.generation += 1;
//          nn.savedData.iterations = 0;
//          //DataHandler.LoadGenerationData(dataPath, 0);
//          SaveNetwork();
//        }
//        else {
//          //SaveNetwork();
//        }
      }
    }
  }

  // If playing using learned network from supervised training then pass predicted direction to pacman
  public Vector2 GetDirectionForPacMan() {
    print("get direction");
    predictedDirectionIndex = nn.MakePrediction(BuildInputs());

    switch (predictedDirectionIndex) {
    case 0:
      return Vector2.left;
    case 1:
      return Vector2.right;
    case 2:
      return Vector2.up;
    case 3:
      return Vector2.down;
    }

    // Should never reach this
    return Vector2.zero;
  }

  public Vector2 GetNextBestDirectionForPacMan() {
    predictedDirectionIndex = nn.NextBiggestIndex(predictedDirectionIndex);

    switch (predictedDirectionIndex) {
    case 0:
      return Vector2.left;
    case 1:
      return Vector2.right;
    case 2:
      return Vector2.up;
    case 3:
      return Vector2.down;
    case -1:
      // If we run out of posibilities report the error as (0,0)
      return Vector2.zero;
    }

    // Should never reach this
    return Vector2.zero;
  }


  // Build new network from scratch - should only be done once when first starting the experiment
  public void CreateNewNetwork() {
    Debug.Log ("Creating brand new network from scratch");
    nn = new NeuralNetwork(numInput, numHidden, numOutput, seed);

    Debug.Log ("Populating network with random weights");
    nn.InitializeWeights();

    nn.SaveLearnedValues();
    SaveNetwork();
  }

  // Recreate network from saved data - done on each new loading of the level
  // (Luke, can we have a more efficent way of resetting the level than having to re-load each time?)
  public void LoadNetwork() {
    Debug.Log ("Loading data for generation: " + generationToLoad + " into container from path: " + dataPath + " ...");
    NetworkData dataToLoad = DataHandler.LoadGenerationData (dataPath, generationToLoad);

    Debug.Log ("Populating network with loaded data...");
    nn = new NeuralNetwork(dataToLoad);

    // This just keeps the UI values consistant
    numInput = nn.savedData.numInput;
    numHidden = nn.savedData.numHidden;
    numOutput = nn.savedData.numOutput;
  }

  // Copy all the weights. biases and detas to xml data file
  public void SaveNetwork() {
//    Debug.Log ("Learning values...");
//    nn.SaveLearnedValues ();

//    Debug.Log ("Adding generation to data list...");
    DataHandler.AddGenerationToSavedData (dataPath, nn.savedData);
  }

  // Returns the [x,y] position for the next destination the neural network has determined for PacMan
  public Vector2 ChooseDestination() {
    int desinationIndex = nn.MakePrediction(BuildInputs());
    Vector2 nextDestination = PerceptionInfo.Get.GetValidDestination (desinationIndex);
//    print(nextDestination);
    return nextDestination;
  }

  // Recalculates the weights, biases and deltas in the network via back propagation
  public void LearnFromDecision(bool didPacManDie) {
    Debug.Log ("Updating weights...");
    nn.UpdateWeightsSingle(CalculateError(), learnRate, momentum, weightDecay); // find better weights

    Debug.Log ("Storing values into network data...");
    nn.SaveLearnedValues(didPacManDie);

    Debug.Log ("Saving network...");
    SaveNetwork();
  }

  // Uses the perception data of the current world state to populate the network input values
  private double[] BuildInputs() {
    int inputIndex = 0;
    
    double[] Dot = PerceptionInfo.Get.Dot;
    foreach (double value in Dot) {
      inputArray [inputIndex++] = value;
    }
    
    double[] Ghosts = PerceptionInfo.Get.Ghosts;
    foreach (double value in Ghosts) {
      inputArray [inputIndex++] = value;
    }

    if (firstInputBuild) {
      double[] Walls = PerceptionInfo.Get.Walls;
      foreach (double value in Walls) {
        inputArray [inputIndex++] = value;
      }

      firstInputBuild = false;
    }

    return inputArray;
  }

  // Calculates our error as a function of distance survived and dots eaten
  private double CalculateError() {
    PerceptionInfo data = PerceptionInfo.Get;
    double survivalError = ((data.totalTilesSurvivedOnWayToDestination / data.totalTilesToDestination) - 1) * 2;
    double effectivenessError = (data.totalDotsEatenOnWayToDestination / data.totalTilesToDestination);
    double lateGameEffectivenessBias = (data.totalDotsEatenOnWayToDestination * 2 / data.totalDotsThatWereRemaining);
    return survivalError + (effectivenessError * lateGameEffectivenessBias);
  }

  public void StartNetwork() {
    networkIsEnabled = true;
    // TODO start the network
  }

  public void StopNetwork() {
    // TODO stop the network
    networkIsEnabled = false;
  }

  private IEnumerator PeriodicalylSaveNetwork() {
    while (true) {
      yield return new WaitForSeconds(delayBetweenSaves);
      if (nn != null) {
        SaveNetwork();
      }
    }
  }
}
  