using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;           // IEnumerator のための参照
using UnityEngine.Networking;       // UnityWebRequest のための参照
using System;                       // Serializable のための参照
using System.Collections.Generic;   // List のための参照

public class Sample01_GetAPI : MonoBehaviour
{
    // アクセスする URL
    // サーバーURL + /api/get でアクセス
    string urlAPI = "";

    // 受信した JSON データを Unity で扱うデータにする ResponseData ベースクラス
    [Serializable]
    public class ResponseData
    {
        public List<ResponseDataList> data;
    }

    [Serializable]
    public class ResponseDataList
    {
        public string Name;
        public List<ResponseDataListImage> Image;
    }

    // Image 列のデータの中身
    [Serializable]
    public class ResponseDataListImage
    {
        public int width;
        public int height;
        public string url;
    }

    void Start()
    {
        StartCoroutine("GetAPI");
    }

    IEnumerator GetAPI()
    {
        // HTTP リクエストする(GET メソッド) UnityWebRequest を呼び出し
        // アクセスする先は変数 urlGitHub で設定
        UnityWebRequest request = UnityWebRequest.Get(urlAPI);

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

                // コンソールに表示
                Debug.Log($"responseData: {request.downloadHandler.text}");

                // ResponseData クラスで Unity で扱えるデータ化
                ResponseData response = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);

                for (int i = 0; i < 3 ; i++)
                {
                    // 割当先の Tile
                    GameObject tile = GameObject.Find("Tile" + i.ToString());

                    // 今回の行データ
                    ResponseDataList currentLine = response.data[i];

                    // テキスト反映
                    tile.transform.Find("MessageText").GetComponent<TextMesh>().text = currentLine.Name;

                    // 画像読み込み
                    StartCoroutine(GetTexture( currentLine.Image[0].url, tile ));
                }

                break;
        }


    }

    // テクスチャ読み込み
    IEnumerator GetTexture(string url, GameObject tile)
    {
        // テクスチャを GET リクエストで読み込む。ブラウザでも見れる。
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        // リクエスト開始
        yield return request.SendWebRequest();

        Debug.Log("リクエスト開始");

        // 結果によって分岐
        switch (request.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("リクエスト中");
                break;

            case UnityWebRequest.Result.ConnectionError:
                Debug.Log("ConnectionError");
                Debug.Log(request.error);
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("ProtocolError");
                Debug.Log(request.responseCode);
                Debug.Log(request.error);
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log("リクエスト成功");

                // テクスチャに割り当て
                Texture loadedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

                tile.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", loadedTexture);

                break;
        }
    }
}
