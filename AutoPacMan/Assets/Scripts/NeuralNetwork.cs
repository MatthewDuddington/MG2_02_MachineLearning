//
//  Neural Network code and comments adapted from James McCaffrey's Build 2013 talk:
//  "Developing neural networks using Visual Studio"
//  (c) James McCaffrey 2013
//  www.quaetrix.com/Build2013.html
//  youtube.com/watch?v=-zT1Zi_ukSk
//
//  Additional code and comments by Matthew Duddington 2017
//
////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class NeuralNetwork {
  
  private static System.Random rnd;

  private int numInput;
  private int numHidden;
  private int numOutput;

  private double[] inputs;

  private double[][] inputToHiddenWeights; // input-hidden
  private double[]   hiddenBiases;
  private double[]   hiddenOutputs;

  private double[][] hiddenToOutputWeights; // hidden-output
  private double[]   outputBiases;

  private double[] outputs;

  // back-prop specific arrays (these could be local to method UpdateWeights)
  private double[] outputGradients; // output gradients for back-propagation
  private double   outputGradient;
  private double[] hiddenGradients; // hidden gradients for back-propagation

  // back-prop momentum specific arrays (could be local to method Train)
  private double[][] inputToHiddenPreviousWeightsDelta;  // for momentum with back-propagation
  private double[]   hiddenPreviousBiasesDelta;
  private double[][] hiddenToOutputPreviousWeightsDelta;
  private double[]   outputPreviousBiasesDelta;

  int predictionIndex;
  double predictionValue;

  int generation;

  public NetworkData savedData = new NetworkData();

  // Used when building a brand new network for the very first time
  public NeuralNetwork(int numInput,
    int numHidden,
    int numOutput,
    int seed)
  {
    rnd = new System.Random(seed); // for InitializeWeights() and Shuffle()

    this.numInput = numInput;
    this.numHidden = numHidden;
    this.numOutput = numOutput;

    this.inputs = new double[numInput];

    this.inputToHiddenWeights = MakeMatrix(numInput, numHidden);
    this.hiddenBiases = new double[numHidden];
    this.hiddenOutputs = new double[numHidden];

    this.hiddenToOutputWeights = MakeMatrix(numHidden, numOutput);
    this.outputBiases = new double[numOutput];

    this.outputs = new double[numOutput];

    // back-prop related arrays below
    this.hiddenGradients = new double[numHidden];
    this.outputGradients = new double[numOutput];

    this.inputToHiddenPreviousWeightsDelta = MakeMatrix(numInput, numHidden);
    this.hiddenPreviousBiasesDelta = new double[numHidden];
    this.hiddenToOutputPreviousWeightsDelta = MakeMatrix(numHidden, numOutput);
    this.outputPreviousBiasesDelta = new double[numOutput];

    this.generation = 0;

    this.savedData.iterations = 0;

    // Save the data that doesnt change during learning
    this.savedData.seed = seed;
    this.savedData.numInput = numInput;
    this.savedData.numHidden = numHidden;
    this.savedData.numOutput = numOutput;
  } // constructor

  // Used when recreating network from loaded data
  public NeuralNetwork(NetworkData dataToLoad)
  {
    rnd = new System.Random(dataToLoad.seed); // for InitializeWeights() and Shuffle()

    this.numInput = dataToLoad.numInput;
    this.numHidden = dataToLoad.numHidden;
    this.numOutput = dataToLoad.numOutput;

    this.inputs = new double[numInput];

    this.inputToHiddenWeights = dataToLoad.inputToHiddenWeights;
    this.hiddenBiases = dataToLoad.hiddenBiases;
    this.hiddenOutputs = dataToLoad.hiddenOutputs;

    this.hiddenToOutputWeights = dataToLoad.hiddenToOutputWeights;
    this.outputBiases = dataToLoad.outputBiases;

    this.outputs = new double[numOutput];

    // back-prop related arrays below
    this.hiddenGradients = new double[numHidden];
    this.outputGradients = new double[numOutput];

    this.inputToHiddenPreviousWeightsDelta = dataToLoad.inputToHiddenPreviousWeightsDelta;
    this.hiddenPreviousBiasesDelta = dataToLoad.hiddenPreviousBiasesDelta;
    this.hiddenToOutputPreviousWeightsDelta = dataToLoad.hiddenToOutputPreviousWeightsDelta;
    this.outputPreviousBiasesDelta = dataToLoad.outputPreviousBiasesDelta;

    this.generation = dataToLoad.generation;

    this.savedData = dataToLoad;
  } // constructor

  private static double[][] MakeMatrix(int rows, int cols) // helper for ctor
  {
    double[][] result = new double[rows][];
    for (int r = 0; r < result.Length; ++r)
      result[r] = new double[cols];
    return result;
  }

  // For use with Unsupervised Training mode
  public void SaveLearnedValues(bool didDecisionFail) {
    savedData.iterations += 1;

    // Save a new generation after it fails (i.e. dies from a ghost), but only after attempting a few iterations (i.e. decisions) 
    if (didDecisionFail && savedData.iterations > 10) {
      generation += 1;
    }

    savedData.generation = generation;

    savedData.inputToHiddenWeights = inputToHiddenWeights;

    savedData.hiddenBiases = hiddenBiases;
    savedData.hiddenOutputs = hiddenOutputs;
    savedData.hiddenToOutputWeights = hiddenToOutputWeights;

    savedData.outputBiases = outputBiases;

    savedData.inputToHiddenPreviousWeightsDelta = inputToHiddenPreviousWeightsDelta;
    savedData.hiddenPreviousBiasesDelta = hiddenPreviousBiasesDelta;
    savedData.hiddenToOutputPreviousWeightsDelta = hiddenToOutputPreviousWeightsDelta;
    savedData.outputPreviousBiasesDelta = outputPreviousBiasesDelta;
  }

  // For use with Supervised Training mode
  public void SaveLearnedValues() {
    savedData.iterations += 1;

    savedData.inputToHiddenWeights = inputToHiddenWeights;

    savedData.hiddenBiases = hiddenBiases;
    savedData.hiddenOutputs = hiddenOutputs;
    savedData.hiddenToOutputWeights = hiddenToOutputWeights;

    savedData.outputBiases = outputBiases;

    savedData.inputToHiddenPreviousWeightsDelta = inputToHiddenPreviousWeightsDelta;
    savedData.hiddenPreviousBiasesDelta = hiddenPreviousBiasesDelta;
    savedData.hiddenToOutputPreviousWeightsDelta = hiddenToOutputPreviousWeightsDelta;
    savedData.outputPreviousBiasesDelta = outputPreviousBiasesDelta;
  }

  // ----------------------------------------------------------------------------------------

  public void SetWeights(double[] weights)
  {
    // copy weights and biases in weights[] array to i-h weights, i-h biases, h-o weights, h-o biases
    int numWeights = (numInput * numHidden) + (numHidden * numOutput) + numHidden + numOutput;
    if (weights.Length != numWeights)
      Debug.LogError("Bad weights array length: ");

    int k = 0; // points into weights param

    for (int i = 0; i < numInput; ++i)
      for (int j = 0; j < numHidden; ++j)
        inputToHiddenWeights[i][j] = weights[k++];
    for (int i = 0; i < numHidden; ++i)
      hiddenBiases[i] = weights[k++];
    for (int i = 0; i < numHidden; ++i)
      for (int j = 0; j < numOutput; ++j)
        hiddenToOutputWeights[i][j] = weights[k++];
    for (int i = 0; i < numOutput; ++i)
      outputBiases[i] = weights[k++];
  }

  public void InitializeWeights()
  {
    // initialize weights and biases to small random values
    int numWeights = (numInput * numHidden) + (numHidden * numOutput) + numHidden + numOutput;
    double[] initialWeights = new double[numWeights];
    double lo = -0.01;
    double hi = 0.01;
    for (int i = 0; i < initialWeights.Length; ++i)
      initialWeights[i] = (hi - lo) * rnd.NextDouble() + lo;
    this.SetWeights(initialWeights);
  }

  // ----------------------------------------------------------------------------------------

  private double[] ComputeOutputs(double[] xValues)
  {
    if (xValues.Length != numInput)
      Debug.LogError("Bad xValues array length");

    double[] hiddenSums = new double[numHidden]; // hidden nodes sums scratch array
    double[] outputSums = new double[numOutput]; // output nodes sums

    for (int i = 0; i < xValues.Length; ++i) // copy x-values to inputs
      this.inputs[i] = xValues[i];

    for (int hiddenIndex = 0; hiddenIndex < numHidden; ++hiddenIndex)  // compute i-h sum of weights * inputs
      for (int inputIndex = 0; inputIndex < numInput; ++inputIndex)
        hiddenSums[hiddenIndex] += this.inputs[inputIndex] * this.inputToHiddenWeights[inputIndex][hiddenIndex]; // note +=

    for (int i = 0; i < numHidden; ++i)  // add biases to input-to-hidden sums
      hiddenSums[i] += this.hiddenBiases[i];

    for (int i = 0; i < numHidden; ++i)   // apply activation
      this.hiddenOutputs[i] = HyperTanFunction(hiddenSums[i]); // hard-coded

    for (int outputIndex = 0; outputIndex < numOutput; ++outputIndex)   // compute h-o sum of weights * hOutputs
      for (int hiddenIndex = 0;  hiddenIndex< numHidden; hiddenIndex++)
        outputSums[outputIndex] += hiddenOutputs[hiddenIndex] * hiddenToOutputWeights[hiddenIndex][outputIndex];

    for (int i = 0; i < numOutput; ++i)  // add biases to input-to-hidden sums
      outputSums[i] += outputBiases[i];

    double[] softOut = Softmax(outputSums); // softmax activation does all outputs at once for efficiency
    System.Array.Copy(softOut, outputs, softOut.Length);

    double[] retResult = new double[numOutput]; // could define a GetOutputs method instead
    System.Array.Copy(this.outputs, retResult, retResult.Length);
    return retResult;
  } // ComputeOutputs

  private static double HyperTanFunction(double x)
  {
    if (x < -20.0) return -1.0; // approximation is correct to 30 decimals
    else if (x > 20.0) return 1.0;
    else return Math.Tanh(x);
  }

  private static double[] Softmax(double[] outputSums) 
  {
    // determine max output sum
    // does all output nodes at once so scale doesn't have to be re-computed each time
    double max = outputSums[0];
    for (int i = 0; i < outputSums.Length; ++i)
      if (outputSums[i] > max) max = outputSums[i];

    // determine scaling factor -- sum of exp(each val - max)
    double scale = 0.0;
    for (int i = 0; i < outputSums.Length; ++i)
      scale += Math.Exp(outputSums[i] - max);

    double[] result = new double[outputSums.Length];
    for (int i = 0; i < outputSums.Length; ++i)
      result[i] = Math.Exp(outputSums[i] - max) / scale;

    return result; // now scaled so that xi sum to 1.0
  }

  // ----------------------------------------------------------------------------------------

  public void UpdateWeights(double[] trainingValues, double learnRate, double momentum, double weightDecay)
  {
    // update the weights and biases using back-propagation, with target values, eta (learning rate),
    // alpha (momentum).
    // assumes that SetWeights and ComputeOutputs have been called and so all the internal arrays
    // and matrices have values (other than 0.0)
    if (trainingValues.Length != numOutput)
      Debug.LogError("target values not same Length as output in UpdateWeights");

    // 1. compute output gradients
    for (int i = 0; i < outputGradients.Length; ++i)
    {
      // derivative of softmax = (1 - y) * y (same as log-sigmoid)
      double derivative = (1 - outputs[i]) * outputs[i]; 
      // 'mean squared error version' includes (1-y)(y) derivative
      outputGradients[i] = derivative * (trainingValues[i] - outputs[i]); 
    }

    // 2. compute hidden gradients
    for (int i = 0; i < hiddenGradients.Length; ++i)
    {
      // derivative of tanh = (1 - y) * (1 + y)
      double derivative = (1 - hiddenOutputs[i]) * (1 + hiddenOutputs[i]); 
      double sum = 0.0;
      for (int j = 0; j < numOutput; ++j) // each hidden delta is the sum of numOutput terms
      {
        double x = outputGradients[j] * hiddenToOutputWeights[i][j];
        sum += x;
      }
      hiddenGradients[i] = derivative * sum;
    }

    // 3a. update hidden weights (gradients must be computed right-to-left but weights
    // can be updated in any order)
    for (int i = 0; i < inputToHiddenWeights.Length; ++i) // 0..2 (3)
    {
      for (int j = 0; j < inputToHiddenWeights[0].Length; ++j) // 0..3 (4)
      {
        double delta = learnRate * hiddenGradients[j] * inputs[i]; // compute the new delta
        inputToHiddenWeights[i][j] += delta; // update. note we use '+' instead of '-'. this can be very tricky.
        // now add momentum using previous delta. on first pass old value will be 0.0 but that's OK.
        inputToHiddenWeights[i][j] += momentum * inputToHiddenPreviousWeightsDelta[i][j]; 
        inputToHiddenWeights[i][j] -= (weightDecay * inputToHiddenWeights[i][j]); // weight decay
        inputToHiddenPreviousWeightsDelta[i][j] = delta; // don't forget to save the delta for momentum 
      }
    }

    // 3b. update hidden biases
    for (int i = 0; i < hiddenBiases.Length; ++i)
    {
      double delta = learnRate * hiddenGradients[i] * 1.0; // t1.0 is constant input for bias; could leave out
      hiddenBiases[i] += delta;
      hiddenBiases[i] += momentum * hiddenPreviousBiasesDelta[i]; // momentum
      hiddenBiases[i] -= (weightDecay * hiddenBiases[i]); // weight decay
      hiddenPreviousBiasesDelta[i] = delta; // don't forget to save the delta
    }

    // 4. update hidden-output weights
    for (int i = 0; i < hiddenToOutputWeights.Length; ++i)
    {
      for (int j = 0; j < hiddenToOutputWeights[0].Length; ++j)
      {
        // see above: hOutputs are inputs to the nn outputs
        double delta = learnRate * outputGradients[j] * hiddenOutputs[i];  
        hiddenToOutputWeights[i][j] += delta;
        hiddenToOutputWeights[i][j] += momentum * hiddenToOutputPreviousWeightsDelta[i][j]; // momentum
        hiddenToOutputWeights[i][j] -= (weightDecay * hiddenToOutputWeights[i][j]); // weight decay
        hiddenToOutputPreviousWeightsDelta[i][j] = delta; // save
      }
    }

    // 4b. update output biases
    for (int i = 0; i < outputBiases.Length; ++i)
    {
      double delta = learnRate * outputGradients[i] * 1.0;
      outputBiases[i] += delta;
      outputBiases[i] += momentum * outputPreviousBiasesDelta[i]; // momentum
      outputBiases[i] -= (weightDecay * outputBiases[i]); // weight decay
      outputPreviousBiasesDelta[i] = delta; // save
    }
  } // UpdateWeights

  // ----------------------------------------------------------------------------------------

  public void UpdateWeightsSingle(double errorValue_, double learnRate, double momentum, double weightDecay)
  {
    // update the weights and biases using back-propagation, with target values, eta (learning rate),
    // alpha (momentum).
    // assumes that SetWeights and ComputeOutputs have been called and so all the internal arrays
    // and matrices have values (other than 0.0)

    double errorValue = errorValue_ - predictionValue;

    // 1. compute output gradient
    {
      // derivative of softmax = (1 - y) * y (same as log-sigmoid)
      double derivative = (1 - outputs [predictionIndex]) * outputs [predictionIndex]; 
      // 'mean squared error version' includes (1-y)(y) derivative
      outputGradient = derivative * (errorValue - outputs [predictionIndex]); 
    }

    // 2. compute hidden gradients
    for (int i = 0; i < hiddenGradients.Length; ++i)
    {
      // derivative of tanh = (1 - y) * (1 + y)
      double derivative = (1 - hiddenOutputs[i]) * (1 + hiddenOutputs[i]); 
      double x = outputGradient * hiddenToOutputWeights[i][predictionIndex];
      hiddenGradients[i] = derivative * x;
    }

    // 3a. update hidden weights (gradients must be computed right-to-left but weights
    // can be updated in any order)
    for (int i = 0; i < inputToHiddenWeights.Length; ++i) // 0..2 (3)
    {
      for (int j = 0; j < inputToHiddenWeights[0].Length; ++j) // 0..3 (4)
      {
        double delta = learnRate * hiddenGradients[j] * inputs[i]; // compute the new delta
        inputToHiddenWeights[i][j] += delta; // update. note we use '+' instead of '-'. this can be very tricky.
        // now add momentum using previous delta. on first pass old value will be 0.0 but that's OK.
        inputToHiddenWeights[i][j] += momentum * inputToHiddenPreviousWeightsDelta[i][j]; 
        inputToHiddenWeights[i][j] -= (weightDecay * inputToHiddenWeights[i][j]); // weight decay
        inputToHiddenPreviousWeightsDelta[i][j] = delta; // don't forget to save the delta for momentum 
      }
    }

    // 3b. update hidden biases
    for (int i = 0; i < hiddenBiases.Length; ++i)
    {
      double delta = learnRate * hiddenGradients[i] * 1.0; // t1.0 is constant input for bias; could leave out
      hiddenBiases[i] += delta;
      hiddenBiases[i] += momentum * hiddenPreviousBiasesDelta[i]; // momentum
      hiddenBiases[i] -= (weightDecay * hiddenBiases[i]); // weight decay
      hiddenPreviousBiasesDelta[i] = delta; // don't forget to save the delta
    }

    // 4. update hidden-output weights
    for (int i = 0; i < hiddenToOutputWeights.Length; ++i)
    {
      // see above: hOutputs are inputs to the nn outputs
      double delta = learnRate * outputGradient * hiddenOutputs[i];  
      hiddenToOutputWeights[i][predictionIndex] += delta;
      hiddenToOutputWeights[i][predictionIndex] += momentum * hiddenToOutputPreviousWeightsDelta[i][predictionIndex]; // momentum
      hiddenToOutputWeights[i][predictionIndex] -= (weightDecay * hiddenToOutputWeights[i][predictionIndex]); // weight decay
      hiddenToOutputPreviousWeightsDelta[i][predictionIndex] = delta; // save
    }

    // 4b. update output biases
    {
      double delta = learnRate * outputGradient * 1.0;
      outputBiases[predictionIndex] += delta;
      outputBiases[predictionIndex] += momentum * outputPreviousBiasesDelta[predictionIndex]; // momentum
      outputBiases[predictionIndex] -= (weightDecay * outputBiases[predictionIndex]); // weight decay
      outputPreviousBiasesDelta[predictionIndex] = delta; // save
    }
  } // UpdateWeightsSingle

  // ----------------------------------------------------------------------------------------

  public int MakePrediction(double[] inputData)
  {
    // train a back-prop style NN classifier using learning rate and momentum
    // weight decay reduces the magnitude of a weight value over time unless that value
    // is constantly increased

    if (inputData.Length != numInput)
      Debug.LogError ("Wrong number of inputs being passed into training");

    double[] xValues = inputData; // inputs
    double[] yValues;

    yValues = this.ComputeOutputs(xValues); // copy xValues in, compute outputs (store them internally)

    this.predictionIndex = MaxIndex (yValues);
    this.predictionValue = yValues [this.predictionIndex];
    return this.predictionIndex;
  } // Train

  private static int MaxIndex(double[] vector) // helper for Accuracy()
  {
    // index of largest value
    int bigIndex = 0;
    double biggestVal = vector[0];
    for (int i = 0; i < vector.Length; ++i)
    {
      if (vector[i] > biggestVal)
      {
        biggestVal = vector[i];
        bigIndex = i;
      }
    }
    return bigIndex;
  }

  public int NextBiggestIndex(int previous) {
    // index of next best value
    int nextIndex = 0;
    double bigVal = this.outputs[previous];
    double nextVal = 0.0;
    for (int i = 0; i < outputs.Length; ++i)
    {
      if (outputs[i] < bigVal && outputs[i] > nextVal)
      {
        nextVal = outputs[i];
        nextIndex = i;
      }
    }
    return nextIndex;
  }

}
