using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;           // IEnumerator のための参照
using UnityEngine.Networking;       // UnityWebRequest のための参照
using System;                       // Serializable のための参照
using System.Collections.Generic;   // List のための参照
using UnityEngine.UI;

public class Sample05_NextButton_Direct_Airtable : MonoBehaviour, IPointerClickHandler
{

    // Airtable の Base ID
    string settingAirtableBaseID = "settingAirtableBaseID";

    // Airtable の API キー
    string settingAirtableAPIKey = "settingAirtableAPIKey";

    // Airtable の Table 名
    string settingAirtableTable = "QuizList";

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

    [Serializable]
    public class ResponseDataRecordField
    {
        public string ID;
        public string QuizText;
        public string Select1;
        public string Select2;
        public string Select3;
        public int Answer;
    }

    // 現在のクイズ番号
    int currentQuizID = 1;

    // クイズリスト
    ResponseData responseQuizList;

    // 現在のクイズ
    ResponseDataRecordField currentQuiz;

    void Start()
    {
        StartCoroutine("GetAPI");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 次へ
        currentQuizID += 1;
        if (currentQuizID > 3)
        {
            currentQuizID = 1;  // 繰り返し
        }
        // クイズ設定
        SetCurrentQuiz();
    }

    void SetCurrentQuiz()
    {
        // 現在のクイズ
        currentQuiz = responseQuizList.records[currentQuizID - 1].fields;

        // テキスト設定
        GameObject.Find("QuizText").GetComponent<TextMesh>().text = currentQuiz.QuizText;
        GameObject.Find("Button1").GetComponentInChildren<Text>().text = currentQuiz.Select1;
        GameObject.Find("Button2").GetComponentInChildren<Text>().text = currentQuiz.Select2;
        GameObject.Find("Button3").GetComponentInChildren<Text>().text = currentQuiz.Select3;

    }
    public void OnClickSelectButon(int selectId)
    {
        

        // 答え合わせ
        Debug.Log($"OnClickSelectButon selectId:{selectId}");

        if (selectId == currentQuiz.Answer)
        {
            // あたり
            Debug.Log($"あたり！");

        } else
        {
            // はずれ
            Debug.Log($"はずれ");

        }

        // 次へ
        currentQuizID += 1;
        if (currentQuizID > 3)
        {
            currentQuizID = 1;  // 繰り返し
        }

        // クイズ設定
        SetCurrentQuiz();
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
                responseQuizList = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);

                // 初回のクイズ
                SetCurrentQuiz();

                break;
        }

    }

    void Update()
    {

    }
}
