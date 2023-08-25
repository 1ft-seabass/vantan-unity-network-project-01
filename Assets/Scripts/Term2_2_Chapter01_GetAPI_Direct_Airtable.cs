using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator のための参照
using UnityEngine.Networking;   // UnityWebRequest のための参照
using System;                   // Serializable のための参照

using System.Collections.Generic; // List のための参照

public class Term2_2_Chapter01_GetAPI_Direct_Airtable : MonoBehaviour
{
    // Airtable の Base ID
    string settingAirtableBaseID = "settingAirtableBaseID";

    // Airtable の API キー
    string settingAirtableAPIKey = "settingAirtableAPIKey";

    // Airtable の Table 名
    string settingAirtableTable = "Sample02";

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
        // フィールド名 Color が string 型
        public string Color;
        // フィールド名 ID が string 型
        public string ID;
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

                // データを一つずつ反映する
                for (int i = 0; i < 3; i++)
                {
                    GameObject cube = GameObject.Find("Cube" + i.ToString());

                    // Debug.Log("Cube" + i.ToString());

                    string colorName = response.records[i].fields.Color;

                    // Debug.Log(colorName);

                    cube.transform.Find("MessageText").GetComponent<TextMesh>().text = colorName;

                    // カラー変更
                    if (colorName == "Red")
                    {
                        cube.GetComponent<Renderer>().material.color = Color.red;
                    }
                    else if (colorName == "Yellow")
                    {
                        cube.GetComponent<Renderer>().material.color = Color.yellow;
                    }
                    else if (colorName == "Blue")
                    {
                        cube.GetComponent<Renderer>().material.color = Color.blue;
                    }
                }

                break;
        }


    }
}
