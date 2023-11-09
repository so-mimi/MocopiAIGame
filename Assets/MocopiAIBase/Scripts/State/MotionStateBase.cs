using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MocopiDistinction
{
    public abstract class MotionStateBase : MonoBehaviour
    {
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();
    }
}
