using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEvent2 : MonoBehaviour
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
        UnityEngine.Object.Destroy(textureScreenCapture);
    }

}
