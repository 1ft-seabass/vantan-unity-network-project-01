using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator のための参照
using UnityEngine.Networking;   // UnityWebRequest のための参照
using System;                   // Serializable のための参照

using System.Collections.Generic; // List のための参照

public class Term2_1_Chapter03_GetAPI : MonoBehaviour
{
    // API の接続先
    // 今回は サーバーURL + /api/get を読み込む
    string urlAPI = "";

    // 受信した JSON データを Unity で扱うデータにする ResponseData ベースクラス
    // 今回は {"data":["Red","Yello","Blue"]} という data のオブジェクトの中に配列が入っています
    [Serializable]
    public class ResponseData
    {
        
    }

    void Start()
    {
        // 開始時に読み込み開始
        GetDataCore();
    }

    void Update()
    {

    }

    void GetDataCore()
    {
        // HTTP リクエストを非同期処理を待つためコルーチンとして呼び出す
        StartCoroutine("GetData");
    }

    IEnumerator GetData()
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

                // データを一つずつ反映する
                for (int i = 0; i < 3; i++)
                {
                    
                }

                break;
        }


    }
}
