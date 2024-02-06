using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UniRx;

namespace MocopiDistinction
{
    internal class InputJointAndGetResult : MonoBehaviour
    {
        [SerializeField] private MocopiDistinctionAI mocopiDistinctionAI = null;
        
        [SerializeField] private Transform rootTransform = null;
        [SerializeField] private Transform LUpLegTransform = null;
        [SerializeField] private Transform RUpLegTransform = null;
        [SerializeField] private Transform LLowLegTransform = null;
        [SerializeField] private Transform RLowLegTransform = null;
        [SerializeField] private Transform LFootTransform = null;
        [SerializeField] private Transform RFootTransform = null;
        [SerializeField] private Transform firstTorsoTransform = null;
        [SerializeField] private Transform thirdTorsoTransform = null;
        [SerializeField] private Transform fifthTorsoTransform = null;
        [SerializeField] private Transform seventhTorsoTransform = null;
        [SerializeField] private Transform RShoulderTransform = null;
        [SerializeField] private Transform LShoulderTransform = null;
        [SerializeField] private Transform firstNeckTransform = null;
        [SerializeField] private Transform secondNeckTransform = null;
        [SerializeField] private Transform RUpArmTransform = null;
        [SerializeField] private Transform LUpArmTransform = null;
        [SerializeField] private Transform RLowArmTransform = null;
        [SerializeField] private Transform LLowArmTransform = null;
        [SerializeField] private Transform RHandTransform = null;
        [SerializeField] private Transform LHandTransform = null;
        
        [System.Serializable]
        public class MotionData
        {
            public float[] data;
        }
        
        private List<float> _data = new();
        [SerializeField]
        private int serverPort = 6789; // FastAPIサーバーのURL
        private float timer = 0.0f;
        private float interval = 0.1f;
        
        private Thread receiveThread;
        private UdpClient client;
        private int listenPort = 6790; // Pythonサーバーが応答を送信するポート


        public InputJointAndGetResult(float[] results)
        {
            this.results = results;
        }

        public float[] results;
        
        public void BiginToAI()
        {
            // UniRxを用いて0.1秒ごとに処理を行う
            Observable.EveryUpdate()
                .Where(_ => timer > interval)
                .Subscribe(_ =>
                {
                    timer = 0f;  // タイマーリセット
                    SendMotionDataToServer();
                })
                .AddTo(this);
        }
        
        void Update()
        {
            timer += Time.deltaTime;
        }

        private float[] GetMotionData()
        {
            if (_data == null)
            {
                _data = new List<float>();
            }
            _data.Add(rootTransform.localPosition.x);
            _data.Add(rootTransform.localPosition.y);
            _data.Add(rootTransform.localPosition.z);
            _data.Add(rootTransform.localRotation.x);
            _data.Add(rootTransform.localRotation.y);
            _data.Add(rootTransform.localRotation.z);
            _data.Add(rootTransform.localRotation.w);
            _data.Add(LUpLegTransform.localRotation.x);
            _data.Add(LUpLegTransform.localRotation.y);
            _data.Add(LUpLegTransform.localRotation.z);
            _data.Add(LUpLegTransform.localRotation.w);
            _data.Add(RUpLegTransform.localRotation.x);
            _data.Add(RUpLegTransform.localRotation.y);
            _data.Add(RUpLegTransform.localRotation.z);
            _data.Add(RUpLegTransform.localRotation.w);
            _data.Add(LLowLegTransform.localRotation.x);
            _data.Add(LLowLegTransform.localRotation.y);
            _data.Add(LLowLegTransform.localRotation.z);
            _data.Add(LLowLegTransform.localRotation.w);
            _data.Add(RLowLegTransform.localRotation.x);
            _data.Add(RLowLegTransform.localRotation.y);
            _data.Add(RLowLegTransform.localRotation.z);
            _data.Add(RLowLegTransform.localRotation.w);
            _data.Add(LFootTransform.localRotation.x);
            _data.Add(LFootTransform.localRotation.y);
            _data.Add(LFootTransform.localRotation.z);
            _data.Add(LFootTransform.localRotation.w);
            _data.Add(RFootTransform.localRotation.x);
            _data.Add(RFootTransform.localRotation.y);
            _data.Add(RFootTransform.localRotation.z);
            _data.Add(RFootTransform.localRotation.w);
            _data.Add(firstTorsoTransform.localRotation.x);
            _data.Add(firstTorsoTransform.localRotation.y);
            _data.Add(firstTorsoTransform.localRotation.z);
            _data.Add(firstTorsoTransform.localRotation.w);
            _data.Add(thirdTorsoTransform.localRotation.x);
            _data.Add(thirdTorsoTransform.localRotation.y);
            _data.Add(thirdTorsoTransform.localRotation.z);
            _data.Add(thirdTorsoTransform.localRotation.w);
            _data.Add(fifthTorsoTransform.localRotation.x);
            _data.Add(fifthTorsoTransform.localRotation.y);
            _data.Add(fifthTorsoTransform.localRotation.z);
            _data.Add(fifthTorsoTransform.localRotation.w);
            _data.Add(seventhTorsoTransform.localRotation.x);
            _data.Add(seventhTorsoTransform.localRotation.y);
            _data.Add(seventhTorsoTransform.localRotation.z);
            _data.Add(seventhTorsoTransform.localRotation.w);
            _data.Add(RShoulderTransform.localRotation.x);
            _data.Add(RShoulderTransform.localRotation.y);
            _data.Add(RShoulderTransform.localRotation.z);
            _data.Add(RShoulderTransform.localRotation.w);
            _data.Add(LShoulderTransform.localRotation.x);
            _data.Add(LShoulderTransform.localRotation.y);
            _data.Add(LShoulderTransform.localRotation.z);
            _data.Add(LShoulderTransform.localRotation.w);
            _data.Add(firstNeckTransform.localRotation.x);
            _data.Add(firstNeckTransform.localRotation.y);
            _data.Add(firstNeckTransform.localRotation.z);
            _data.Add(firstNeckTransform.localRotation.w);
            _data.Add(secondNeckTransform.localRotation.x);
            _data.Add(secondNeckTransform.localRotation.y);
            _data.Add(secondNeckTransform.localRotation.z);
            _data.Add(secondNeckTransform.localRotation.w);
            _data.Add(RUpArmTransform.localRotation.x);
            _data.Add(RUpArmTransform.localRotation.y);
            _data.Add(RUpArmTransform.localRotation.z);
            _data.Add(RUpArmTransform.localRotation.w);
            _data.Add(LUpArmTransform.localRotation.x);
            _data.Add(LUpArmTransform.localRotation.y);
            _data.Add(LUpArmTransform.localRotation.z);
            _data.Add(LUpArmTransform.localRotation.w);
            _data.Add(RLowArmTransform.localRotation.x);
            _data.Add(RLowArmTransform.localRotation.y);
            _data.Add(RLowArmTransform.localRotation.z);
            _data.Add(RLowArmTransform.localRotation.w);
            _data.Add(LLowArmTransform.localRotation.x);
            _data.Add(LLowArmTransform.localRotation.y);
            _data.Add(LLowArmTransform.localRotation.z);
            _data.Add(LLowArmTransform.localRotation.w);
            _data.Add(RHandTransform.localRotation.x);
            _data.Add(RHandTransform.localRotation.y);
            _data.Add(RHandTransform.localRotation.z);
            _data.Add(RHandTransform.localRotation.w);
            _data.Add(LHandTransform.localRotation.x);
            _data.Add(LHandTransform.localRotation.y);
            _data.Add(LHandTransform.localRotation.z);
            _data.Add(LHandTransform.localRotation.w);
            
            if (_data.Count > 2610)
            {
                //2610個のデータを超えたら先頭から削除
                _data.RemoveRange(0, _data.Count - 2610);
            }

            return _data.ToArray();
        }
        
        private void SendMotionDataToServer()
        {
            float[] motionData = GetMotionData();  // モーションデータを取得するメソッドを想定
            if (motionData.Length < 2610)
            {
                Debug.Log("モーション取得中");
                return;
            }

            // モーションデータをJSONに変換してサーバーに送信
            SendData(motionData);
        }

        // UDP送信用のメソッド
        public void SendData(float[] motionData)
        {
            UdpClient udpClient = new UdpClient();
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, serverPort); // 送信先のエンドポイント

                // モーションデータをJSON文字列にシリアライズ
                string json = JsonUtility.ToJson(new MotionData { data = motionData });
                byte[] sendBytes = Encoding.UTF8.GetBytes(json);

                udpClient.Send(sendBytes, sendBytes.Length, endPoint); // 指定したエンドポイントにデータを送信
            }
            catch (SocketException e)
            {
                Debug.LogError($"SocketException: {e.Message}");
            }
            finally
            {
                udpClient.Close();
            }
        }
    }
}
