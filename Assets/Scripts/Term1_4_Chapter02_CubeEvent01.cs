using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator のための参照
using UnityEngine.Networking;   // UnityWebRequest のための参照
using System;                   // Serializable のための参照

public class Term1_4_Chapter02_CubeEvent01 : MonoBehaviour, IPointerClickHandler
{
    // アクセスする API の URL
    // https://dog.ceo/dog-api/
    string urlAPI = "https://dog.ceo/api/breeds/image/random";

    // 受信した JSON データを Unity で扱うデータにする ResponseData ベースクラス
    [Serializable]
    public class ResponseData
    {
        // message というプロパティ名で string 型で変換
        // message だけ取得
        public string message;
    }

    void Start()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // HTTP リクエストを非同期処理を待つためコルーチンとして呼び出す
        StartCoroutine("GetAPI");
    }

    // API 取得
    IEnumerator GetAPI()
    {
        // HTTP リクエストする(GET メソッド) UnityWebRequest を呼び出し
        // アクセスする先は変数 urlGitHub で設定
        UnityWebRequest request = UnityWebRequest.Get(urlAPI);

        // リクエスト開始
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

                // ResponseData クラスで Unity で扱えるデータ化
                ResponseData response = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);

                // 画像読み込み
                StartCoroutine("GetTexture",response.message);

                break;
        }


    }

    // テクスチャ読み込みb
    IEnumerator GetTexture(string url)
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

            case UnityWebRequest.Result.Success:
                Debug.Log("リクエスト成功");

                // テクスチャに割り当て
                Texture loadedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

                GameObject.Find("Tile").GetComponent<MeshRenderer>().material.SetTexture("_MainTex", loadedTexture);

                break;
        }
    }


    void Update()
    {
        
    }
}
