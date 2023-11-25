using System;
using System.Collections;
using System.Collections.Generic;
using MocopiDistinction;
using UnityEngine;

public class PKFire : MotionStateBase
{
    public Action OnPKFire;
    public override void OnEnter()
    {
        OnPKFire?.Invoke();
    }

    public override void OnUpdate()
    {
    }

    public override void OnExit()
    {
    }
}
