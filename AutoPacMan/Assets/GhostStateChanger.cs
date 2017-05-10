using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostStateChanger : MonoBehaviour {

    public Ghost2[] ghosts;
    public Text text;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            foreach (Ghost2 g in ghosts)
            {
                if (g.myState == Ghost2.Statey.Chase)
                {
                    g.myState = Ghost2.Statey.Corner;
                    text.text = "Corner";
                }
                else
                {
                    g.myState = Ghost2.Statey.Chase;
                    text.text = "Chase";
                }
            }
        }

    }
}
