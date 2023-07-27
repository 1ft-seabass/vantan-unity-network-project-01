using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator のための参照
using UnityEngine.Networking;   // UnityWebRequest のための参照
using System.Text;              // Encoding のための参照

public class Scene04WelcomeMessage : MonoBehaviour
{
    // アクセスする URL
    // サーバー URL + /api/post/title
    string urlGitHub = "ここにサーバーURLを入れる";

    void Start()
    {
        // 起動時にデータを読み込む

        // HTTP リクエストを非同期処理を待つためコルーチンとして呼び出す
        StartCoroutine("RequestWelcomeMessage");
    }

    // リクエストする本体
    IEnumerator RequestWelcomeMessage()
    {
        // HTTP リクエストする(POST メソッド) UnityWebRequest を呼び出し
        // アクセスする先は変数 urlGitHub で設定
        UnityWebRequest request = new UnityWebRequest(urlGitHub, "POST");
        // 空ではやりとりできないので、今回は仮の dummy 文字を用意
        byte[] bodyRaw = Encoding.UTF8.GetBytes("dummy");
        // アップロード（Unity→サーバ）のハンドラを作成
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        // ダウンロード（サーバ→Unity）のハンドラを作成
        request.downloadHandler = new DownloadHandlerBuffer();

        // リクエスト開始
        Debug.Log("リクエスト開始");
        yield return request.SendWebRequest();

        // 結果によって分岐
        switch (request.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("リクエスト中");
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log("リクエスト成功");

                // コンソールに表示
                Debug.Log($"responseData: {request.downloadHandler.text}");

                // テキストに反映
                string message = request.downloadHandler.text;
                this.gameObject.GetComponent<TextMesh>().text = message;

                break;
        }


    }

}
