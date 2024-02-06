using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using MocopiDistinction;
using UniRx;
using UnityEngine;

public class UDPReceiver : MonoBehaviour
{
    private UdpClient udpClient;
    private Subject<string> subject = new Subject<string>();
    [SerializeField] private InputJointAndGetResult inputJointAndGetResult;

    void Start() {
        udpClient = new UdpClient(6790);
        udpClient.BeginReceive(OnReceived, udpClient);

        subject
            .ObserveOnMainThread()
            .Subscribe(msg => {
                float[] results = ExtractNumbers(msg);
                inputJointAndGetResult.results = results;
            }).AddTo(this);
    }

    private void OnReceived(System.IAsyncResult result) {
        UdpClient getUdp = (UdpClient) result.AsyncState;
        IPEndPoint ipEnd = null;

        byte[] getByte = getUdp.EndReceive(result, ref ipEnd);

        var receivedData = Encoding.UTF8.GetString(getByte);
        subject.OnNext(receivedData);

        getUdp.BeginReceive(OnReceived, getUdp);
    }

    private void OnDestroy() {
        udpClient.Close();
    }
    
    public float[] ExtractNumbers(string jsonString)
    {
        List<float> numbers = new List<float>();

        // 正規表現を使用して文字列から数値を抽出（科学的表記を含む）
        string pattern = @"-?\d+(\.\d+)?([eE][-+]?\d+)?";
        foreach (Match match in Regex.Matches(jsonString, pattern))
        {
            if(float.TryParse(match.Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float number))
            {
                numbers.Add(number);
            }
        }


        return numbers.ToArray();
    }
}

