﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPushable : MonoBehaviour {

    private HintUI hintUI;

    private void Start()
    {
        hintUI = FindObjectOfType<HintUI>();
    }

    public void OnTriggerEnter(Collider other)
    {
        ObjectSync os = other.GetComponentInParent<ObjectSync>();

        if (os != null && os.hasAuthority && os.CompareTag("Fille"))
        {
            hintUI.Display(Controls.B, "Push the box");
            this.transform.parent.GetComponentInChildren<HighlightObject>().ToggleHighlight(true);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        TriggerFeet triggerFeet = other.GetComponent<TriggerFeet>();

        if (!(triggerFeet == null))
        {
            Player player = triggerFeet.GetComponentInParent<PlayerGirl>();

            if (player != null && player.State != player.States[StateEnum.CLIMBING])
            {
                if (StaticInput.GetButtonDown("B"))
                {
                    
                    Vector3 pushDirection = this.transform.parent.position - this.transform.position;
                    pushDirection.y = 0;
                    player.transform.forward = pushDirection;

                    player.ChangeState(StateEnum.PUSHING, this.GetComponentInParent<Rigidbody>());
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        ObjectSync os = other.GetComponentInParent<ObjectSync>();

        if (os != null && os.hasAuthority && os.CompareTag("Fille"))
        {
            hintUI.Hide();
        }
        this.transform.parent.GetComponentInChildren<HighlightObject>().ToggleHighlight(false);
    }
}
