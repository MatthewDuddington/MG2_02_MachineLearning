﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMasterControl : MonoBehaviour {

    public enum playerState { PLAYER, AI };
    public playerState myPlayerState;
    PacmanMovement pacMovement;
    PacmanAI pacAI;
    bool canPress = true;

    void Start()
    {
        pacMovement = GetComponent<PacmanMovement>();
        pacAI = GetComponent<PacmanAI>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && canPress && transform.position.x % 0.5f == 0 && transform.position.y % 0.5f == 0)
        {
            if (myPlayerState == playerState.AI)
            {
                myPlayerState = playerState.PLAYER;
                pacAI.enabled = false;
                pacMovement.enabled = true;
            }
            else
            {
                myPlayerState = playerState.AI;


                pacAI.enabled = true;
                pacMovement.enabled = false;

            }
            canPress = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            canPress = true;
        }
    }
}