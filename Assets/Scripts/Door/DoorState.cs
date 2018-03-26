﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class DoorState : NetworkBehaviour {

    public List<Collider> RelatedKey;
    public List<GameObject> Locks;

    public Animator Anim;
    public DoorTrigger dt;

    private bool Opened = false;
    private bool Idle = false;
    
    private bool IsGirlInRange = false;

    private ObjectSync Os;

    private void Update()
    {
        if (IsGirlInRange && Input.GetKeyDown(KeyCode.Q) && Locks.Count == 0)
        {
            OpenDoor();
            CloseDoor();
        }

    }

    void Start()
    {
        dt.GirlTriggerState += GirlInRange;

        if (!isServer)
        { 
            Destroy(Anim);
            Destroy(this);
        }
    }
    
    public void StopAnimating()
    {
        Idle = true;
    }

    public void OpenDoor()
    {
        if (!Idle || Opened)
            return;

        Anim.Play("Door_Open");
        Idle = false;
        Opened = true;
    }

    public void CloseDoor()
    {
        if (!Idle || !Opened)
            return;

        Anim.Play("Door_Close");
        Idle = false;
        Opened = false;
    }
    
    void GirlInRange(bool state)
    {
        IsGirlInRange = state;
    }  

    [Server]
    void OnCollisionEnter(Collision collision)
    {
        foreach(Collider c in RelatedKey)
        {
            // Bonne clef
            if (collision.collider == c)
            {
                GameObject.FindGameObjectWithTag("Fille").GetComponent<PickupObject>().InsertKeyInDoor();

                GameObject lck = Locks[Locks.Count];

                Locks.Remove(lck);

                lck.GetComponent<FadeMaterial>().Rpc_Kill();
                c.gameObject.GetComponent<FadeMaterial>().Rpc_Kill();

                break;
            }
        }
    }
}
