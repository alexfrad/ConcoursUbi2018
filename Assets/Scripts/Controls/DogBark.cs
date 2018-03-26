﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class DogBark : NetworkBehaviour {
    private DogBarkEcho Echo;
    public Action<Vector3> HasBarked;

    private Color green;
    private Color yellow;
    private Color red;

    private void Start()
    {
        green.r = 66.0f / 255;
        green.g = 142f / 255;
        green.b = 64f / 255;
        green.a = 1;

        yellow.r = 196.0f / 255;
        yellow.g = 178.0f / 255;
        yellow.b = 59.0f / 255;
        yellow.a = 1;

        red.r = 164.0f / 255;
        red.g = 25.0f / 255;
        red.b = 15.0f / 255;
        red.a = 1;
    }

    // Update is called once per frame
    void Update () {
        if (!hasAuthority)
            return;

		if(Input.GetButtonDown("A"))
        {
            Cmd_StartBark(green);
            GameEssentials.PlayerDog.ChangeState(StateEnum.BARKING);
        }

        if (Input.GetButtonDown("Y"))
        {
            Cmd_StartBark(yellow);
            GameEssentials.PlayerDog.ChangeState(StateEnum.BARKING);
        }

        if (Input.GetButtonDown("B"))
        {
            Cmd_StartBark(red);
            GameEssentials.PlayerDog.ChangeState(StateEnum.BARKING);

            if (HasBarked != null)
                HasBarked.Invoke(transform.position);
        }
    }

    [Command]
    public void Cmd_StartBark(Color color)
    {
        if (!Echo)
            Echo = GameObject.FindGameObjectWithTag("Fille").GetComponent<DogBarkEcho>();

        Echo.StartBark(color);

        if(HasBarked != null)
            HasBarked.Invoke(transform.position);
    }
}
