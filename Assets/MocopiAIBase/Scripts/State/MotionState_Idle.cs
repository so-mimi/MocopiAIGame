using MocopiDistinction;
using UnityEngine;

namespace MotionDistinction
{
    internal class MotionState_Idle : MotionStateBase
    {
        [SerializeField] private MotionText motionText;
        public override void OnEnter()
        {
            Debug.Log("Idle");
            motionText.SetText("Idle");
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnExit()
        {
            
        }
    }
}