using MocopiDistinction;
using UnityEngine;

namespace MotionDistinction
{
    internal class MotionState_OpenBox : MotionStateBase
    {
        [SerializeField] private MotionText motionText;
        public override void OnEnter()
        {
            Debug.Log("OpenBox");
            motionText.SetText("OpenBox");
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnExit()
        {
            
        }
    }
}