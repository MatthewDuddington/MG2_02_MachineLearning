using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class DataHandler {

  public static GenerationsContainer generationsContainer = new GenerationsContainer();

  //public delegate void SerializeAction ();
  //public static event SerializeAction OnLoaded;
  //public static event SerializeAction OnBeforeSave;
  
  public static NetworkData LoadGenerationData(string path, int generationIndex) {
    generationsContainer = LoadGenerations (path);

    return generationsContainer.generations[0];  //TODO make container actually store a progressive series of generations
  }

  public static void AddGenerationToSavedData(string path, NetworkData data) {
    generationsContainer.generations.Add (data);

//    Debug.Log ("Saving generations to container...");
    SaveGenerations (path, generationsContainer);

    ClearGenerations ();
  }

  public static GenerationsContainer LoadGenerations(string dataPath) {
    XmlSerializer serializer = new XmlSerializer (typeof(GenerationsContainer));

    FileStream stream = new FileStream (dataPath, FileMode.Open);  // Erase existing data and override
    //FileStream stream = new FileStream (dataPath, FileMode.Append);  // Add to end of existing data

    GenerationsContainer generations = serializer.Deserialize (stream) as GenerationsContainer;

    stream.Close();

    return generations;
  }

  public static void SaveGenerations(string dataPath, GenerationsContainer generationsContainer) {
    XmlSerializer serializer = new XmlSerializer (typeof(GenerationsContainer));

    FileStream stream = new FileStream (dataPath, FileMode.Truncate);  // Erase existing data and override
    //FileStream stream = new FileStream (dataPath, FileMode.Append);  // Add to end of existing data

    serializer.Serialize (stream, generationsContainer);

    stream.Close();
  }

  public static void ClearGenerations() {
    generationsContainer.generations.Clear ();
  }

}

public class NetworkData {

  // Generations track the individual saved versions of the network
  [XmlAttribute("Generation")]
  public int generation;

  // Iterations track the number of times a particular generation has been restarted (without defining a new generation)
  [XmlElement("Iterations")]
  public int iterations;


  [XmlElement("seed")]
  public int seed;
  
  [XmlElement("numInput")]
  public int numInput;

  [XmlElement("numHidden")]
  public int numHidden;

  [XmlElement("numOutput")]
  public int numOutput;


  [XmlElement("inputToHiddenWeights")]
  public double[][] inputToHiddenWeights;


  [XmlElement("hiddenBiases")]
  public double[] hiddenBiases;

  [XmlElement("hiddenOutputs")]
  public double[] hiddenOutputs;

  [XmlElement("hiddenToOutputWeights")]
  public double[][] hiddenToOutputWeights;


  [XmlElement("outputBiases")]
  public double[] outputBiases;


  [XmlElement("inputToHiddenPreviousWeightsDelta")]
  public double[][] inputToHiddenPreviousWeightsDelta;

  [XmlElement("hiddenPreviousBiasesDelta")]
  public double[] hiddenPreviousBiasesDelta;

  [XmlElement("hiddenToOutputPreviousWeightsDelta")]
  public double[][] hiddenToOutputPreviousWeightsDelta;

  [XmlElement("outputPreviousBiasesDelta")]
  public double[] outputPreviousBiasesDelta;

}
