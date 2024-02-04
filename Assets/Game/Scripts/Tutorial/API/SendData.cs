using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class SendData : MonoBehaviour
{
    // APIのURL
    string url = "http://127.0.0.1:8000/upload-csv/";

    // CSVファイルパスを送信する関数
    public void SendCSVPath(string filePath)
    {
        StartCoroutine(PostRequest(filePath));
    }

    IEnumerator PostRequest(string filePath)
    {
        // JSON形式のデータを作成
        string jsonData = $"{{\"filePath\":\"{filePath}\"}}";
        using (var request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }

            // UnityWebRequestとUploadHandlerRawはusingステートメント内で自動的に破棄されます
            // そのため、明示的なDispose呼び出しは必要ありません
        }
    }
}