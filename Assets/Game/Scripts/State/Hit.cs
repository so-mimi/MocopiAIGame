using MocopiDistinction;
using UnityEngine;
using System;

public class Hit : MotionStateBase
{
    public Action OnHit;

    public override void OnEnter()
    {
        Debug.Log("Hit");
        OnHit?.Invoke();
    }

    public override void OnUpdate()
    {
    }

    public override void OnExit()
    {
    }
}
