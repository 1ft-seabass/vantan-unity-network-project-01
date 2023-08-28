using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections.Generic; // List のための参照
using System.Collections;       // IEnumerator のための参照
using UnityEngine.Networking;   // UnityWebRequest のための参照
using System;                   // Serializable のための参照
using System.Text;              // Encoding のための参照


public class Term2_2_Chapter03_RankingMessage_Direct_Airtable : MonoBehaviour
{

    // Airtable の Base ID
    string settingAirtableBaseID = "settingAirtableBaseID";

    // Airtable の API キー
    string settingAirtableAPIKey = "settingAirtableAPIKey";

    // Airtable の Table 名
    string settingAirtableTable = "PointList";

    // Airtable の view 名
    string settingAirtableView = "Grid view";

    // 受信した JSON データを Unity で扱うデータにする ResponseData ベースクラス
    [Serializable]
    public class ResponseData
    {
        public List<ResponseDataRecord> records;
    }

    [Serializable]
    public class ResponseDataRecord
    {
        public string id;
        public string createdTime;
        public ResponseDataRecordField fields;
    }

    // ResponseDataRecordField は Table の列の内容によって変更します
    [Serializable]
    public class ResponseDataRecordField
    {
        // フィールド名 Name が string 型
        public string Name;
        // フィールド名 Point が int 型
        public string Point;
        // フィールド名 CreatedTime が string 型
        public string CreatedTime;
    }


    void Start()
    {
        GetDataCore();
    }

    public void GetDataCore()
    {
        StartCoroutine("GetData");
    }


    IEnumerator GetData()
    {
        // API URL 作成
        string urlAirtableAPI = "https://api.airtable.com/v0/" + settingAirtableBaseID + "/" + settingAirtableTable + "?view=" + settingAirtableView;
        // API URL を Uri.AbsoluteUri と通して URL パラメータ調整
        // 例 ?view=Grid view が ?view=Grid%20view になる
        urlAirtableAPI = new Uri(urlAirtableAPI).AbsoluteUri;

        // HTTP リクエストする(GET メソッド) UnityWebRequest を呼び出し
        UnityWebRequest request = UnityWebRequest.Get(urlAirtableAPI);

        // API キーを HTTP ヘッダーに設定
        request.SetRequestHeader("Authorization", "Bearer " + settingAirtableAPIKey);

        // リクエスト開始
        yield return request.SendWebRequest();

        Debug.Log("リクエスト開始");

        // 結果によって分岐
        switch (request.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("リクエスト中");
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("ProtocolError");
                Debug.Log($"responseCode : {request.responseCode}");
                Debug.Log($"error : {request.error}");
                break;

            case UnityWebRequest.Result.ConnectionError:
                Debug.Log("ConnectionError");
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log("リクエスト成功");

                // コンソールに表示
                Debug.Log($"responseData: {request.downloadHandler.text}");

                // ResponseData クラスで Unity で扱えるデータ化
                ResponseData response = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);

                // 文字列を初期化
                string textRankingList = "[Ranking]\n";

                Debug.Log($"データ数 : {response.records.Count}");

                // データを一つずつ反映する
                for (int i = 0; i < 3; i++)
                {
                    ResponseDataRecord record = response.records[i];
                    ResponseDataRecordField fields = record.fields;

                    // 文字列を連結
                    textRankingList += "[" + i.ToString() + "]" + fields.Name + " " + fields.Point.ToString() + "pt" + "\n";
                }

                // メッセージに反映
                this.transform.GetComponent<TextMesh>().text = textRankingList;

                break;
        }


    }
}
