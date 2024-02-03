using MocopiDistinction;
using UnityEngine;
using System;

public class Kick : MotionStateBase
{
    public Action OnKick;
    public override void OnEnter()
    {
        Debug.Log("Kick");
        OnKick?.Invoke();
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
        
    }
}
