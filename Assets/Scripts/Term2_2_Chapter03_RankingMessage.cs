using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections.Generic; // List のための参照
using System.Collections;       // IEnumerator のための参照
using UnityEngine.Networking;   // UnityWebRequest のための参照
using System;                   // Serializable のための参照
using System.Text;              // Encoding のための参照


public class Term2_2_Chapter03_RankingMessage : MonoBehaviour
{
    // 受信した JSON データを Unity で扱うデータにする ResponseData ベースクラス
    // 今回は data の中身が配列で、さらに Name , Point , CreatedTime のプロパティが入っているので、List の中身を定義する ResponseDataList 型を作成。
    [Serializable]
    public class ResponseData
    {
        public List<ResponseDataList> data;
    }

    // Name , Point , CreatedTime のプロパティが入っているので、List の中身を定義する ResponseDataList 型を作成
    [Serializable]
    public class ResponseDataList
    {
        
    }

    void Start()
    {
        GetDataCore();
    }

    public void GetDataCore()
    {
        StartCoroutine("GetData");
    }

    // アクセスする URL
    // サーバーURL + /api/get/ranking でアクセス
    string urlAPI = "";

    IEnumerator GetData()
    {
        // HTTP リクエストする(GET メソッド) UnityWebRequest を呼び出し
        // アクセスする先は変数 urlAPI で設定
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

                // 文字列を初期化
                string textRankingList = "[Ranking]\n";

                Debug.Log(response.data);

                // データを一つずつ反映する
                for (int i = 0; i < 3; i++)
                {
                    ResponseDataList currentLine = response.data[i];

                    Debug.Log(currentLine);

                    // 文字列を連結
                    textRankingList += "";
                }

                // メッセージに反映
                this.transform.GetComponent<TextMesh>().text = textRankingList;

                break;
        }


    }
}
