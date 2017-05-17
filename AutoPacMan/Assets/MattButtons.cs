using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MattButtons : MonoBehaviour {

  void Update() {
    if (Input.GetKeyDown (KeyCode.K))
      Save ();
    else if (Input.GetKeyDown (KeyCode.L))
      Load ();
    else if (Input.GetKey (KeyCode.P))
    {
      if (Input.GetKeyDown(KeyCode.Equals))
        PacManBrain.Get.CreateNewNetwork ();
    }
  }

    public void Save()
    {
        PacManBrain.Get.SaveNetwork();
    }

    public void Load()
    {
        PacManBrain.Get.LoadNetwork();
    }

}
