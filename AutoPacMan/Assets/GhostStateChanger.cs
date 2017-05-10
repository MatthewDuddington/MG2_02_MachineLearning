using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostStateChanger : MonoBehaviour
{

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
            scaredTimer = 0;
        }
    }
    void Update()
    {

        //scared timer...
        if (ghosts[0].myState != Ghost2.Statey.Scared &&
            ghosts[1].myState != Ghost2.Statey.Scared &&
            ghosts[2].myState != Ghost2.Statey.Scared &&
            ghosts[3].myState != Ghost2.Statey.Scared)
        {
            stateTimer += Time.deltaTime;
            scaredTimer = 0;
        }
        else
        {
            scaredTimer += Time.deltaTime;
        }

        if (scaredTimer >= 6.5f)                        //LIMIT FOR POWER PELLET!!!
        {
            foreach (Ghost2 g in ghosts)
            {
                g.myState = Ghost2.Statey.Chase;
                text.text = "Chase";
            }
        }

        //waves of chase / corner
        for (int i = 0; i < ghosts.Length; i++)
        {
            if (stateTimer < 7)
            {
                if (ghosts[i].myState != Ghost2.Statey.Eaten && ghosts[i].myState != Ghost2.Statey.Scared)
                {
                    ghosts[i].myState = Ghost2.Statey.Corner;
                    text.text = "Corner";
                }
            }
            if (stateTimer > 7 && stateTimer < 27)
            {
                if (ghosts[i].myState != Ghost2.Statey.Eaten && ghosts[i].myState != Ghost2.Statey.Scared)
                {
                    ghosts[i].myState = Ghost2.Statey.Chase;
                    text.text = "Chase";
                }
            }
            if (stateTimer > 27 && stateTimer < 34)
            {
                if (ghosts[i].myState != Ghost2.Statey.Eaten && ghosts[i].myState != Ghost2.Statey.Scared)
                {
                    ghosts[i].myState = Ghost2.Statey.Corner;
                    text.text = "Corner";
                }
            }
            if (stateTimer > 34 && stateTimer < 54)
            {
                if (ghosts[i].myState != Ghost2.Statey.Eaten && ghosts[i].myState != Ghost2.Statey.Scared)
                {
                    ghosts[i].myState = Ghost2.Statey.Chase;
                    text.text = "Chase";
                }
            }
            if (stateTimer > 54 && stateTimer < 59)
            {
                if (ghosts[i].myState != Ghost2.Statey.Eaten && ghosts[i].myState != Ghost2.Statey.Scared)
                {
                    ghosts[i].myState = Ghost2.Statey.Corner;
                    text.text = "Corner";
                }
            }
            if (stateTimer > 59 && stateTimer < 79)
            {
                if (ghosts[i].myState != Ghost2.Statey.Eaten && ghosts[i].myState != Ghost2.Statey.Scared)
                {
                    ghosts[i].myState = Ghost2.Statey.Chase;
                    text.text = "Chase";
                }
            }
            if (stateTimer > 79 && stateTimer < 84)
            {
                if (ghosts[i].myState != Ghost2.Statey.Eaten && ghosts[i].myState != Ghost2.Statey.Scared)
                {
                    ghosts[i].myState = Ghost2.Statey.Corner;
                    text.text = "Corner";
                }
            }
            if (stateTimer > 84)
            {
                if (ghosts[i].myState != Ghost2.Statey.Eaten && ghosts[i].myState != Ghost2.Statey.Scared)
                {
                    ghosts[i].myState = Ghost2.Statey.Chase;
                    text.text = "Chase";
                }
            }
        }
    }
}
