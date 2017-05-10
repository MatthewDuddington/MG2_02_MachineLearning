using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostStateChanger : MonoBehaviour {

    public Ghost2[] ghosts;
    int asdf = 0;
    public Text text;
    public float stateTimer;

    void Start()
    {
        Ting();
    }
    void Update()
    {
        if (ghosts[0].myState != Ghost2.Statey.Scared)
        {
            stateTimer += Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (asdf != 2)
            {
                asdf++;
                Ting();
            }
            else
            {
                asdf = 0;
                Ting();
            }
        }

    }

    void Ting()
    {
        foreach (Ghost2 g in ghosts)
        {
            if (asdf == 0)
            {
                g.myState = Ghost2.Statey.Chase;
                text.text = "Chase";
            }
            if (asdf == 1)
            {
                g.myState = Ghost2.Statey.Corner;
                text.text = "Corner";
            }
            if (asdf == 2)
            {
                g.myState = Ghost2.Statey.Scared;
                text.text = "Scared";
            }
        }
    }
}
