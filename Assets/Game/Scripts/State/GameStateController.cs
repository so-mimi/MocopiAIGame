using System.Collections.Generic;
using MocopiDistinction;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

internal class GameStateController : MonoBehaviour
{
    [SerializeField] private List<MotionStateBase> motionStates = new List<MotionStateBase>();
    private int _currentStateIndex = 0;
    [SerializeField] private GameOutput output;
    
    
    
    private void Start()
    {
        motionStates[_currentStateIndex].OnEnter();
        output.ObserveEveryValueChanged(x => x.MotionIndex).Subscribe(_ =>
        {
            int nextIndex = output.MotionIndex;
            AutoStateTransitionSequence(nextIndex);
        });
    }
    
    private void Update()
    {
        motionStates[_currentStateIndex].OnUpdate();
    }
    
    // ステートの自動遷移
    protected void AutoStateTransitionSequence(int nextIndex)
    {
        if(nextIndex == _currentStateIndex) return;
        
        // 現在のステートの終了処理
        motionStates[_currentStateIndex].OnExit();
        // 現在のステートを更新
        _currentStateIndex = nextIndex;
        // 次のステートの開始処理
        motionStates[_currentStateIndex].OnEnter();
    }
}
