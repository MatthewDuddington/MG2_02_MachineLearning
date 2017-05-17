using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MattButtons : MonoBehaviour {

    public void Save()
    {
        PacManBrain.Get.SaveNetwork();
    }

    public void Load()
    {
        PacManBrain.Get.LoadNetwork();
    }

}
