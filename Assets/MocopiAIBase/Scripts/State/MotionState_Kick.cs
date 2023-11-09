using MocopiDistinction;
using UnityEngine;

namespace MotionDistinction
{
    internal class MotionState_Kick : MotionStateBase
    {
        [SerializeField] private MotionText motionText;
        
        public override void OnEnter()
        {
            Debug.Log("Kick");
            motionText.SetText("Kick");
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnExit()
        {
        }
    }
}

