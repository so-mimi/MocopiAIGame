using MocopiDistinction;
using UnityEngine;

namespace MotionDistinction
{
    internal class MotionState_Glide: MotionStateBase
    {
        [SerializeField] private MotionText motionText;
        public override void OnEnter()
        {
            Debug.Log("Glide");
            motionText.SetText("Glide");
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnExit()
        {
            
        }
    }
}