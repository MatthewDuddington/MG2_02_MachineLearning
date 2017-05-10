using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostStateChanger : MonoBehaviour {

    public Ghost2[] ghosts;
    int asdf = 0;
    public Text text;
    public float stateTimer;
    public float scaredTimer;

    public void Scare()
    {
        //Ting();
        foreach (Ghost2 g in ghosts)
        {
            g.myState = Ghost2.Statey.Scared;
            text.text = "Scared";

        }
    }
    void Update()
    {
        if (ghosts[0].myState != Ghost2.Statey.Scared)
        {
            stateTimer += Time.deltaTime;
            scaredTimer = 0;


            if (stateTimer < 7)
            {
                foreach (Ghost2 g in ghosts)
                {
                    if (g.myState != Ghost2.Statey.Eaten)
                    {
                        g.myState = Ghost2.Statey.Corner;
                        text.text = "Corner";
                    }
                }
            }
            if (stateTimer > 7 && stateTimer < 27)
            {
                foreach (Ghost2 g in ghosts)
                {
                    if (g.myState != Ghost2.Statey.Eaten)
                    {
                        g.myState = Ghost2.Statey.Chase;
                        text.text = "Chase";
                    }
                }
            }
            if (stateTimer > 27 && stateTimer < 34)
            {
                foreach (Ghost2 g in ghosts)
                {
                    if (g.myState != Ghost2.Statey.Eaten)
                    {
                        g.myState = Ghost2.Statey.Corner;
                        text.text = "Corner";
                    }
                }
            }
            if (stateTimer > 34 && stateTimer < 54)
            {
                foreach (Ghost2 g in ghosts)
                {
                    if (g.myState != Ghost2.Statey.Eaten)
                    {
                        g.myState = Ghost2.Statey.Chase;
                        text.text = "Chase";
                    }
                }
            }
            if (stateTimer > 54 && stateTimer < 59)
            {
                foreach (Ghost2 g in ghosts)
                {
                    if (g.myState != Ghost2.Statey.Eaten)
                    {
                        g.myState = Ghost2.Statey.Corner;
                        text.text = "Corner";
                    }
                }
            }
            if (stateTimer > 59 && stateTimer < 79)
            {
                foreach (Ghost2 g in ghosts)
                {
                    if (g.myState != Ghost2.Statey.Eaten)
                    {
                        g.myState = Ghost2.Statey.Chase;
                        text.text = "Chase";
                    }
                }
            }
            if (stateTimer > 79 && stateTimer < 84)
            {
                foreach (Ghost2 g in ghosts)
                {
                    if (g.myState != Ghost2.Statey.Eaten)
                    {
                        g.myState = Ghost2.Statey.Corner;
                        text.text = "Corner";
                    }
                }
            }
            if (stateTimer > 84)
            {
                foreach (Ghost2 g in ghosts)
                {
                    if (g.myState != Ghost2.Statey.Eaten)
                    {
                        g.myState = Ghost2.Statey.Chase;
                        text.text = "Chase";
                    }
                }
            }
        }

            //spooked

        else
        {
            scaredTimer += Time.deltaTime;
        }

        if (scaredTimer >= 5)
        {
            foreach (Ghost2 g in ghosts)
            {
                g.myState = Ghost2.Statey.Chase;
                text.text = "Chase";

            }
        }
    }
}
