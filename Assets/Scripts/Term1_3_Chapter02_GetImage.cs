using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator のための参照
using UnityEngine.Networking;   // UnityWebRequest のための参照

public class Term1_3_Chapter02_GetImage : MonoBehaviour, IPointerClickHandler
{
    void Start()
    {
        
    }

    // アクセスする URL
    // サーバーURL + /sample01.png でアクセス
    string urlGitHub = "ここにサーバーURLを入れる";

    public void OnPointerClick(PointerEventData eventData)
    {
        // マウスクリックイベント

        // HTTP リクエストを非同期処理を待つためコルーチンとして呼び出す
        StartCoroutine("GetTexture");
    }

    IEnumerator GetTexture()
    {
        urlGitHub = "https://tseigo-stunning-funicular-q6vj5g6rx6p347j4-8080.app.github.dev/sample01.png";

        // テクスチャを GET リクエストで読み込む。ブラウザでも見れる。
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(urlGitHub);


        // リクエスト開始
        yield return request.SendWebRequest();

        Debug.Log("リクエスト開始");

        // 結果によって分岐
        switch (request.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("リクエスト中");
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log("リクエスト成功");

                // テクスチャに割り当て
                Texture loadedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

                GameObject.Find("Tile01").GetComponent<MeshRenderer>().material.SetTexture("_MainTex", loadedTexture);

                break;
        }
    }
}