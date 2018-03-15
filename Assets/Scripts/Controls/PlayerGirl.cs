﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGirl : Player {

    public float MovementSpeed = 10f;
    public float RotationSpeed = 45f;
    public float PushingMovementSpeed = 5f;

    public float ClimbingSpeed = 10f;

    public bool shaderActivated = true;

    new void Start()
    {
        base.Start();
        //Initialize currentstate and possible states
        State = new PStateGrounded(this, MovementSpeed, RotationSpeed);
        PreviousState = State;
        States = new Dictionary<StateEnum, PlayerState>
        {
            { StateEnum.GROUNDED, State},
            { StateEnum.CLIMBING, new PStateClimbing(this, ClimbingSpeed, RotationSpeed) },
            { StateEnum.GETTING_OFF_LADDER, new PStateScriptedLadder(this) },
            { StateEnum.PUSHING, new PStatePushing(this, PushingMovementSpeed) }
        };
    }

    new void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Camera cam= GetComponent<UpdateEcho>().Cam;
            shaderActivated = !shaderActivated;
            cam.GetComponent<ReplacementShaderCam>().enabled = shaderActivated;
        }
    }
}
