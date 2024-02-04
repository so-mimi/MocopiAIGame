using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// CSVにモーションデータを保存するクラス
/// </summary>
public class MotionDataInputer : MonoBehaviour
{
    private enum InputMode
    {
        Debug,
        Application
    }
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
    
    [SerializeField] private InputMode inputMode = InputMode.Debug;
    [SerializeField] private TMP_InputField filePathInputField = null;
    
    /// <summary>
    /// データ受信最中であるかどうか
    /// </summary>
    public bool isDataInput = false;
    
    List<float> data = new List<float>();
    
    private string _fileName = "";
    public string filePath = "";
    
    /// <summary>
    /// この関数を呼び出した際に、position,rotationのデータの保存を開始する
    ///  motionStateDataには識別結果の[1,0,0]みたいなのを入れる
    /// </summary>
    public void DataInputTimer (List<float> motionStateData) {
        
        data = new List<float>();
        Observable.Timer(System.TimeSpan.Zero, System.TimeSpan.FromSeconds(0.1f))
            .Take(30)
            .Subscribe(_ =>
            {
                Debug.Log("DataInputTimer" + Time.time);
                data.Add(rootTransform.localPosition.x);
                data.Add(rootTransform.localPosition.y);
                data.Add(rootTransform.localPosition.z);
                data.Add(rootTransform.localRotation.x);
                data.Add(rootTransform.localRotation.y);
                data.Add(rootTransform.localRotation.z);
                data.Add(rootTransform.localRotation.w);
                data.Add(LUpLegTransform.localRotation.x);
                data.Add(LUpLegTransform.localRotation.y);
                data.Add(LUpLegTransform.localRotation.z);
                data.Add(LUpLegTransform.localRotation.w);
                data.Add(RUpLegTransform.localRotation.x);
                data.Add(RUpLegTransform.localRotation.y);
                data.Add(RUpLegTransform.localRotation.z);
                data.Add(RUpLegTransform.localRotation.w);
                data.Add(LLowLegTransform.localRotation.x);
                data.Add(LLowLegTransform.localRotation.y);
                data.Add(LLowLegTransform.localRotation.z);
                data.Add(LLowLegTransform.localRotation.w);
                data.Add(RLowLegTransform.localRotation.x);
                data.Add(RLowLegTransform.localRotation.y);
                data.Add(RLowLegTransform.localRotation.z);
                data.Add(RLowLegTransform.localRotation.w);
                data.Add(LFootTransform.localRotation.x);
                data.Add(LFootTransform.localRotation.y);
                data.Add(LFootTransform.localRotation.z);
                data.Add(LFootTransform.localRotation.w);
                data.Add(RFootTransform.localRotation.x);
                data.Add(RFootTransform.localRotation.y);
                data.Add(RFootTransform.localRotation.z);
                data.Add(RFootTransform.localRotation.w);
                data.Add(firstTorsoTransform.localRotation.x);
                data.Add(firstTorsoTransform.localRotation.y);
                data.Add(firstTorsoTransform.localRotation.z);
                data.Add(firstTorsoTransform.localRotation.w);
                data.Add(thirdTorsoTransform.localRotation.x);
                data.Add(thirdTorsoTransform.localRotation.y);
                data.Add(thirdTorsoTransform.localRotation.z);
                data.Add(thirdTorsoTransform.localRotation.w);
                data.Add(fifthTorsoTransform.localRotation.x);
                data.Add(fifthTorsoTransform.localRotation.y);
                data.Add(fifthTorsoTransform.localRotation.z);
                data.Add(fifthTorsoTransform.localRotation.w);
                data.Add(seventhTorsoTransform.localRotation.x);
                data.Add(seventhTorsoTransform.localRotation.y);
                data.Add(seventhTorsoTransform.localRotation.z);
                data.Add(seventhTorsoTransform.localRotation.w);
                data.Add(RShoulderTransform.localRotation.x);
                data.Add(RShoulderTransform.localRotation.y);
                data.Add(RShoulderTransform.localRotation.z);
                data.Add(RShoulderTransform.localRotation.w);
                data.Add(LShoulderTransform.localRotation.x);
                data.Add(LShoulderTransform.localRotation.y);
                data.Add(LShoulderTransform.localRotation.z);
                data.Add(LShoulderTransform.localRotation.w);
                data.Add(firstNeckTransform.localRotation.x);
                data.Add(firstNeckTransform.localRotation.y);
                data.Add(firstNeckTransform.localRotation.z);
                data.Add(firstNeckTransform.localRotation.w);
                data.Add(secondNeckTransform.localRotation.x);
                data.Add(secondNeckTransform.localRotation.y);
                data.Add(secondNeckTransform.localRotation.z);
                data.Add(secondNeckTransform.localRotation.w);
                data.Add(RUpArmTransform.localRotation.x);
                data.Add(RUpArmTransform.localRotation.y);
                data.Add(RUpArmTransform.localRotation.z);
                data.Add(RUpArmTransform.localRotation.w);
                data.Add(LUpArmTransform.localRotation.x);
                data.Add(LUpArmTransform.localRotation.y);
                data.Add(LUpArmTransform.localRotation.z);
                data.Add(LUpArmTransform.localRotation.w);
                data.Add(RLowArmTransform.localRotation.x);
                data.Add(RLowArmTransform.localRotation.y);
                data.Add(RLowArmTransform.localRotation.z);
                data.Add(RLowArmTransform.localRotation.w);
                data.Add(LLowArmTransform.localRotation.x);
                data.Add(LLowArmTransform.localRotation.y);
                data.Add(LLowArmTransform.localRotation.z);
                data.Add(LLowArmTransform.localRotation.w);
                data.Add(RHandTransform.localRotation.x);
                data.Add(RHandTransform.localRotation.y);
                data.Add(RHandTransform.localRotation.z);
                data.Add(RHandTransform.localRotation.w);
                data.Add(LHandTransform.localRotation.x);
                data.Add(LHandTransform.localRotation.y);
                data.Add(LHandTransform.localRotation.z);
                data.Add(LHandTransform.localRotation.w);
            },
            () => {
                data.AddRange(motionStateData);
                if (_fileName == "")
                {
                    DateTime time = DateTime.Now;
                    _fileName = time.Month + "_" + time.Day + "_" + time.Hour + "_" + time.Second + "MotionData";
                }
                
                LogSave(data, _fileName);
            }
        ).AddTo(this);
    }
    
    private IObservable<int> Countdown(int seconds) {
        return Observable.Timer(System.TimeSpan.Zero, System.TimeSpan.FromSeconds(1))
            .Take(seconds)
            .Select(x => seconds - (int)x);
    }
    
    public void LogSave(List<float> x, string fileName)
    {
        if (inputMode == InputMode.Debug)
        {
            filePath = Application.dataPath + "/" + fileName + ".csv";
        }
        else
        {
            filePath = filePathInputField.text + "/" + fileName + ".csv";
        }

        bool append = true;

        // ファイルが存在しない場合は新しく作成
        if (!File.Exists(filePath))
        {
            using (StreamWriter sw = new StreamWriter(filePath, false))
            {
                sw.Flush();
                sw.Close();
            }
        }

        // StreamWriterを作成し、データを書き込む
        using (StreamWriter sw = new StreamWriter(filePath, append))
        {
            // カンマで区切って1行に書き込む
            for (int i = 0; i < x.Count; i++)
            {
                sw.Write(x[i].ToString());
                if (i != x.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.WriteLine();

            sw.Flush();
            sw.Close();
        }
    }
    
    
}