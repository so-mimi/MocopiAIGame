using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MocopiDistinction;
using UniRx;

public class GameOutput : MonoBehaviour
{
    
    [SerializeField] private InputJointAndGetResult inputJointAndGetResult = null;
    [SerializeField] private MotionText motionText;
    public int MotionIndex = 0;

    private void Start()
    {
        if (inputJointAndGetResult == null)
        {
            Debug.LogError("inputJointAndGetResultを設定してください");
        }

        //inputJointAndGetResultのresultsを監視
        inputJointAndGetResult.ObserveEveryValueChanged(x => x.results).Subscribe(_ =>
        {
            //AIの結果を取得
            float[] results = inputJointAndGetResult.results;
            MotionIndex = MostLikelyData(results);
            switch (MotionIndex)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        });
    }

    private int MostLikelyData(float[] results)
    {
        float max = 0;
        int index = 0;
        for (int i = 0; i < results.Length; i++)
        {
            //感度調整
            if (i == 0)
            {
                results[i] *= 5f;
            }
            if (i == 1)
            {
                results[i] *= 92f;
            }
            if (i == 2)
            {
                results[i] *= 0.02f;
            }
            
            if (max < results[i])
            {
                max = results[i];
                index = i;
            }
        }

        return index;
    }
}
