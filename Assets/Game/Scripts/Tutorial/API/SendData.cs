using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using MocopiDistinction;

public class SendData : MonoBehaviour
{
    [SerializeField] private InputJointAndGetResult inputJointAndGetResult;
    [SerializeField] private TutorialSystem tutorialSystem;
    // APIのURL
    string url = "http://127.0.0.1:8000/upload-csv/";
    // リセットAPIのURL
    string resetUrl = "http://127.0.0.1:8000/reset/";

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
                inputJointAndGetResult.BiginToAI();
                tutorialSystem.EndTutorial();
            }

            // UnityWebRequestとUploadHandlerRawはusingステートメント内で自動的に破棄されます
            // そのため、明示的なDispose呼び出しは必要ありません
        }
    }
    
    // リセット処理を送信する関数
    public void SendResetCommand()
    {
        StartCoroutine(PostResetRequest());
    }

    IEnumerator PostResetRequest()
    {
        // リセットコマンドを表すJSON形式のデータを作成
        // ここでは、ResetCommandモデルが特にデータを必要としない場合、空のJSONオブジェクトを送信しています
        string jsonData = "{}";
        using (var request = new UnityWebRequest(resetUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("Reset command sent successfully.");
                // 必要に応じて、リセットが成功したことを示す処理をここに追加
            }
        }
    }
}