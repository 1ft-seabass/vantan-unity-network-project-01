using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CubeEvent3 : MonoBehaviour
{
    // キャプチャされた画像データ
    byte[] dataImage;

    void Start()
    {
        StartCoroutine(DoScreenShot());
    }

    IEnumerator DoScreenShot()
    {
        Debug.Log($"DoScreenShot");

        // レンダリング後に処理を開始
        yield return new WaitForEndOfFrame();

        // Texture2D でスクリーンショットを取得
        Texture2D textureScreenCapture = ScreenCapture.CaptureScreenshotAsTexture();

        // EncodeToJPG で JPEG に変換する。EncodeToPNG なら PNG に変換できる
        dataImage = textureScreenCapture.EncodeToJPG();

        Debug.Log($"撮影完了");
        Debug.Log($"{dataImage}");

        // textureScreenCapture を削除
        Destroy(textureScreenCapture);

        // 画像送信
        StartCoroutine(PostData());
    }

    IEnumerator PostData()
    {
        // IMultipartFormSection で multipart/form-data のデータとして送れます
        // https://docs.unity3d.com/ja/2018.4/Manual/UnityWebRequest-SendingForm.html
        // https://docs.unity3d.com/ja/2019.4/ScriptReference/Networking.IMultipartFormSection.html
        // https://docs.unity3d.com/ja/2020.3/ScriptReference/Networking.MultipartFormDataSection.html
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        // 画像フォームで用意
        formData.Add(new MultipartFormFileSection("imageFile", dataImage, "01.jpg", "multipart/form-data"));

        // HTTP リクエストする(POST メソッド) UnityWebRequest を呼び出し
        // 第 2 引数で上記のフォームデータを割り当てて multipart/form-data のデータとして送ります
        UnityWebRequest request = UnityWebRequest.Post("http://localhost:1880/api/post/image", formData);

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
}
