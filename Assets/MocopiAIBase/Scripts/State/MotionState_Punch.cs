using MocopiDistinction;
using UnityEngine;

namespace MotionDistinction
{
    internal class MotionState_Punch : MotionStateBase
    {
        [SerializeField] private MotionText motionText;
        
        public override void OnEnter()
        {
            Debug.Log("Punch");
            motionText.SetText("Punch");
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnExit()
        {
            
        }
    }
}