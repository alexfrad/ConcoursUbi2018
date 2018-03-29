﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateTalking : PlayerState
{
    private const string AnimatorAction = "Talking";
    private float DialogueTime;

    private float _time;

    public PStateTalking(Player player, float dialogueTime = 1f) : base(player)
    {
        DialogueTime = dialogueTime;
    }

    public override void InterpretInput()
    {
        _time += Time.deltaTime;
        if (_time > DialogueTime)
        {
            _player.ChangeState(StateEnum.GROUNDED);
        }
    }

    public override void OnEnter(object o)
    {
        _player.Animator.SetBool(AnimatorAction, true);
        _time = 0f;
    }

    public override void OnExit()
    {
        _player.Animator.SetBool(AnimatorAction, false);
    }
}
