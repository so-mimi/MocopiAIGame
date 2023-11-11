using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MocopiDistinction;

public class PKThunder : MotionStateBase
{
    public Action OnPKThunder;
    public override void OnEnter()
    {
        Debug.Log("PKThunder");
        OnPKThunder?.Invoke();
    }

    public override void OnUpdate()
    {
    }

    public override void OnExit()
    {
    }
}
