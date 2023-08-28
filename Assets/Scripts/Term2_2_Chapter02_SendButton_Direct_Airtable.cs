using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;           // IEnumerator のための参照
using UnityEngine.Networking;       // UnityWebRequest のための参照
using System;                       // Serializable のための参照
using System.Text;                  // Encoding のための参照
using System.Collections.Generic;   // List のための参照

public class Term2_2_Chapter02_SendButton_Direct_Airtable : MonoBehaviour, IPointerClickHandler
{

    // Airtable の Base ID
    string settingAirtableBaseID = "settingAirtableBaseID";

    // Airtable の API キー
    string settingAirtableAPIKey = "settingAirtableAPIKey";

    // Airtable の Table 名
    string settingAirtableTable = "PointList";

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
        // フィールド名 Color が string 型
        public string Color;
        // フィールド名 ID が string 型
        public string ID;
    }

    // 送信する Unity データを JSON データ化する RequestData ベースクラス
    [Serializable]
    public class RequestData
    {
        public List<RequestDataRecord> records;
    }

    [Serializable]
    public class RequestDataRecord
    {
        public RequestDataRecordField fields;
    }

    // RequestDataRecordField は Table の列の内容によって変更します
    [Serializable]
    public class RequestDataRecordField
    {
        public int Point;
        public string Name;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        // HTTP リクエストを非同期処理を待つためコルーチンとして呼び出す
        StartCoroutine("PostPointData");
    }

    // POST リクエストする本体
    IEnumerator PostPointData()
    {
        // API URL 作成
        string urlAirtableAPI = "https://api.airtable.com/v0/" + settingAirtableBaseID + "/" + settingAirtableTable;
        // API URL を Uri.AbsoluteUri と通して URL パラメータ調整して戻す
        urlAirtableAPI = new Uri(urlAirtableAPI).AbsoluteUri;

        // HTTP リクエストする(POST メソッド) UnityWebRequest を呼び出し
        UnityWebRequest request = new UnityWebRequest(urlAirtableAPI, "POST");

        // RequestData ベースクラスを器として呼び出す
        RequestData requestData = new RequestData();
        requestData.records = new List<RequestDataRecord>();
        // 今回の値
        RequestDataRecord requestDataRecord = new RequestDataRecord();
        requestDataRecord.fields = new RequestDataRecordField();
        // データを設定
        // 現在のポイントを得る
        requestDataRecord.fields.Point = GameObject.Find("ClickPart").GetComponent<Term2_2_Chapter02_ClickPart>().currentPoint;
        // 自分の名前を登録
        requestDataRecord.fields.Name = "Seigo";
        // records に加える
        requestData.records.Add(requestDataRecord);

        // 送信データを JsonUtility.ToJson で JSON 文字列を作成
        // requestData の構造に基づいて変換してくれる
        string strJSON = JsonUtility.ToJson(requestData);
        Debug.Log($"strJSON : {strJSON}");
        // 送信データを Encoding.UTF8.GetBytes で byte データ化
        byte[] bodyRaw = Encoding.UTF8.GetBytes(strJSON);

        // アップロード（Unity→サーバ）のハンドラを作成
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        // ダウンロード（サーバ→Unity）のハンドラを作成
        request.downloadHandler = new DownloadHandlerBuffer();

        // JSON で送ると HTTP ヘッダーで宣言する
        request.SetRequestHeader("Content-Type", "application/json");

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

            case UnityWebRequest.Result.Success:
                Debug.Log("リクエスト成功");

                // コンソールに表示
                Debug.Log($"responseData: {request.downloadHandler.text}");

                // ResponseData クラスで Unity で扱えるデータ化
                ResponseData resultResponse = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);

                // StatusMessage にランキングを割り当て
                GameObject.Find("StatusMessage").GetComponent<TextMesh>().text = "登録されました！";

                break;
        }

    }
}
