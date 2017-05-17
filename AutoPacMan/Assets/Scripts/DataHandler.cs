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

    return generationsContainer.generations [generationIndex];
  }

  public static void AddGenerationToSavedData(string path, NetworkData data) {
    generationsContainer.generations.Add (data);

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
