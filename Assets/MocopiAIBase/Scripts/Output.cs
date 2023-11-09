using UnityEngine;
using UniRx;

namespace MocopiDistinction
{
    internal class Output : MonoBehaviour
    {
        [SerializeField] private InputJointAndGetResult inputJointAndGetResult = null;

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
            });
        }
        
        private int MostLikelyData(float[] results)
        {
            float max = 0;
            int index = 0;
            for (int i = 0; i < results.Length; i++)
            {
                if (max < results[i])
                {
                    max = results[i];
                    index = i;
                }
            }

            return index;
        }
    }
}
