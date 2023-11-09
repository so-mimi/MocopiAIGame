using MocopiDistinction;
using UnityEngine;

namespace MotionDistinction
{
    internal class MotionState_Jump : MotionStateBase
    {
        [SerializeField] private MotionText motionText;
        public override void OnEnter()
        {
            Debug.Log("Jump");
            motionText.SetText("Jump");
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnExit()
        {
            
        }
    }
}