using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;           // IEnumerator のための参照
using UnityEngine.Networking;       // UnityWebRequest のための参照
using System;                       // Serializable のための参照
using System.Collections.Generic;   // List のための参照

public class Sample04_GetAPI_Direct_Airtable : MonoBehaviour
{
    // Airtable の Base ID
    string settingAirtableBaseID = "settingAirtableBaseID";

    // Airtable の API キー
    string settingAirtableAPIKey = "settingAirtableAPIKey";

    // Airtable の Table 名
    string settingAirtableTable = "Sample01";

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
        // フィールド名 Data が string 型
        public string Data;
        // フィールド名 Image には画像関連の情報が入っている
        public string ID;
    }

    void Start()
    {
        StartCoroutine("GetAPI");
    }

    IEnumerator GetAPI()
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

                for (int i = 0; i < response.records.Count; i++)
                {

                    // 今回の行データ
                    ResponseDataRecord currentLine = response.records[i];

                    Debug.Log($"Data : {currentLine.fields.Data}");
                    Debug.Log($"ID : {currentLine.fields.ID}");
                }

                break;
        }


    }

}
