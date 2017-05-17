using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[XmlRoot("GenerationCollection")]
public class GenerationsContainer {

  [XmlArray("Generations")]
  [XmlArrayItem("Generation")]
  public List<NetworkData> Generations = new List<NetworkData> ();

}
