using System;
using MocopiDistinction;
using UnityEngine;

public class Punch : MotionStateBase
{
    public Action OnPunch;
    public override void OnEnter()
    {
        Debug.Log("Punch");
        OnPunch?.Invoke();
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
        
    }
}
