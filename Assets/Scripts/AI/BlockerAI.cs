﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine;

public class BlockerAI : BaseAI {

    public Transform StartPosition;
    public Transform EndPosition;
    public Transform IdleLookAt;

    public GameObject InvisibleColission;

    public AIVision Vision;
    public IdleBehaviour Idle;

    private Vector3 Destination;
    public float MinDistance = 0.1f;

    public float speed;

    public Animator Animator;
    private void Start()
    {
        Animator = GetComponent<Animator>();
        InvisibleColission.transform.localScale = new Vector3((EndPosition.position - StartPosition.position).magnitude, 5, 0);
    }

    // Update is called once per frame
    [Server]
    void Update()
    {
        Vector3 Target;
        Vector3 LookAt;

        Vector3 Path = EndPosition.position - StartPosition.position;

        if (Vision.SeeSomething(out Target))
        {
            Rpc_SetActiveWall(true);
            float Magnitude = Path.magnitude;
            Path.Normalize();

            float dot = Vector3.Dot((Target - StartPosition.position), Path);

            if (dot > Magnitude)
                dot = Magnitude;

            if (dot > 0)
            {
                Destination = StartPosition.position + (dot * Path);
            }
            else if (dot < 0)
            {
                Destination = StartPosition.position;
            }

            LookAt = Target;
        }
        else
        {
            if (CurrentStimuli != null)
            {
                Rpc_SetActiveWall(true);
                LookAt = CurrentStimuli.GetPosition();
            }
            else
            {
                Rpc_SetActiveWall(false);
                LookAt = IdleLookAt.position;
            }

            Destination = Idle.Process();
        }

        Vector3 Direction = Destination - transform.position;
        float signedDistance = Vector3.Dot(Direction, Path);
        if ((Direction).magnitude > MinDistance)
        {
            Direction.Normalize();

            transform.position += (Direction * speed * Time.deltaTime);
        }

        Animator.SetFloat("Speed", Direction.magnitude > MinDistance? Mathf.Sign(signedDistance): 0);
        transform.LookAt(LookAt);
    }

    [ClientRpc]
    public void Rpc_SetActiveWall(bool val)
    {
        InvisibleColission.SetActive(val);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(StartPosition.position, EndPosition.position);

        Gizmos.color = Color.blue;
        Gizmos.DrawCube(StartPosition.position, new Vector3(0.5f, 0.5f, 0.5f));
        Gizmos.DrawCube(EndPosition.position, new Vector3(0.5f, 0.5f, 0.5f));
    }
}
