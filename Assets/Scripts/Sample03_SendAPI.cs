using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator のための参照
using UnityEngine.Networking;   // UnityWebRequest のための参照
using System.Collections.Generic; // Listのため

public class Sample03_SendAPI : MonoBehaviour, IPointerClickHandler
{

    // LINE Notify のトークン
    string tokenLINENotify = "token";


    byte[] dataImage;

    void Start()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // まずスクリーンショット
        StartCoroutine("DoScreendhot");
    }

    IEnumerator PostLINENofify(string message)
    {
        // IMultipartFormSection で multipart/form-data のデータとして送れます
        // https://docs.unity3d.com/ja/2018.4/Manual/UnityWebRequest-SendingForm.html
        // https://docs.unity3d.com/ja/2019.4/ScriptReference/Networking.IMultipartFormSection.html
        // https://docs.unity3d.com/ja/2020.3/ScriptReference/Networking.MultipartFormDataSection.html
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("message", message));
        // 画像を入れる
        // https://notify-bot.line.me/doc/ja/
        formData.Add(new MultipartFormFileSection("imageFile", dataImage, "01.jpg", "multipart/form-data"));

        // HTTP リクエストする(POST メソッド) UnityWebRequest を呼び出し
        // アクセスする先は変数 urlGitHub で設定
        // 第 2 引数で上記のフォームデータを割り当てて multipart/form-data のデータとして送ります
        UnityWebRequest request = UnityWebRequest.Post("https://notify-api.line.me/api/notify", formData);

        // LINE Notify の認証は Authorization ヘッダーで Bearer のあとに API トークンを入れる
        request.SetRequestHeader("Authorization", $"Bearer {tokenLINENotify}");

        // ダウンロード（サーバ→Unity）のハンドラを作成
        request.downloadHandler = new DownloadHandlerBuffer();

        // リクエスト開始
        yield return request.SendWebRequest();

        // 結果によって分岐
        switch (request.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("リクエスト中");
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("ProtocolError");
                Debug.Log(request.responseCode);
                Debug.Log(request.error);
                break;

            case UnityWebRequest.Result.ConnectionError:
                Debug.Log("ConnectionError");
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log("リクエスト成功");

                // コンソールに表示
                Debug.Log($"responseData: {request.downloadHandler.text}");

                break;
        }


    }

    IEnumerator DoScreendhot()
    {
        Debug.Log($"DoScreendhot");

        yield return new WaitForEndOfFrame();

        Texture2D textureScreenCapture = ScreenCapture.CaptureScreenshotAsTexture();

        dataImage = textureScreenCapture.EncodeToJPG();

        Debug.Log($"{dataImage}");

        UnityEngine.Object.Destroy(textureScreenCapture);

        // 第 2 引数で送りたい文言を入れる
        StartCoroutine("PostLINENofify", "Unityからテスト");
    }


    void Update()
    {

    }
}
