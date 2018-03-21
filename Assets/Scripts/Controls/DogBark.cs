﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class DogBark : NetworkBehaviour {
    public float BarkRange;
    private DogBarkEcho Echo;

    public Action<Vector3> HasBarked;
    
	// Update is called once per frame
	void Update () {
        if (!hasAuthority)
            return;

		if(Input.GetButton("Fire1"))
        {
            Cmd_StartBark(Color.green);
            HasBarked.Invoke(transform.position);
        }

        if (Input.GetButton("Fire2"))
        {
            Cmd_StartBark(Color.red);
            HasBarked.Invoke(transform.position);
        }
    }


    [Command]
    public void Cmd_StartBark(Color color)
    {
        if (!Echo)
            Echo = GameObject.FindGameObjectWithTag("Fille").GetComponent<DogBarkEcho>();

        Echo.StartBark(color);
        HasBarked.Invoke(transform.position);
    }
}
