using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;           // IEnumerator のための参照
using UnityEngine.Networking;       // UnityWebRequest のための参照
using System;                       // Serializable のための参照
using System.Collections.Generic;   // List のための参照

public class Sample02_NextButton : MonoBehaviour, IPointerClickHandler
{

    // アクセスする URL
    // サーバーURL + /api/get でアクセス
    string urlAPI = "";

    // 現在のクイズ番号
    int currentQuizID = 1;

    // クイズリスト
    ResponseData responseQuizList;

    // 現在のクイズ
    ResponseDataList currentQuiz;

    // 受信した JSON データを Unity で扱うデータにする ResponseData ベースクラス
    [Serializable]
    public class ResponseData
    {
        public List<ResponseDataList> data;
    }

    [Serializable]
    public class ResponseDataList
    {
        public string ID;
        public string QuizText;
        public string Select1;
        public string Select2;
        public string Select3;
        public int Answer;
    }

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
        currentQuiz = responseQuizList.data[currentQuizID - 1];

        // テキスト設定
        GameObject.Find("QuizText").GetComponent<TextMesh>().text = currentQuiz.QuizText;
        GameObject.Find("SelectText1").GetComponent<TextMesh>().text = currentQuiz.Select1;
        GameObject.Find("SelectText2").GetComponent<TextMesh>().text = currentQuiz.Select2;
        GameObject.Find("SelectText3").GetComponent<TextMesh>().text = currentQuiz.Select3;

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
