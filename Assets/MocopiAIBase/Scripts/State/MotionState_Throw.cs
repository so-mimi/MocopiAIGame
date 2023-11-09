using MocopiDistinction;
using UnityEngine;

namespace MotionDistinction
{
    internal class MotionState_Throw : MotionStateBase
    {
        
        [SerializeField] private MotionText motionText;
        public override void OnEnter()
        {
            Debug.Log("Throw");
            motionText.SetText("Throw");
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnExit()
        {
            
        }
    }
}