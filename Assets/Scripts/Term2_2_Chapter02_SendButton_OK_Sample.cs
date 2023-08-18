using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator のための参照
using UnityEngine.Networking;   // UnityWebRequest のための参照
using System;                   // Serializable のための参照
using System.Text;              // Encoding のための参照

public class Term2_2_Chapter02_SendButton_OK_Sample : MonoBehaviour, IPointerClickHandler
{
    // 受信した JSON データを Unity で扱うデータにする ResultResponseData ベースクラス
    [Serializable]
    public class ResultResponseData
    {
        // result というプロパティ名で string 型で変換
        public string result;
        // recordPoint というプロパティ名で int 型で変換
        public int recordPoint;
    }

    // 送信する Unity データを JSON データ化する PointRequestData ベースクラス
    [Serializable]
    public class PointRequestData
    {
        // point というプロパティ名で int 型で変換
        public int point;
        // name というプロパティ名で int 型で変換
        public string name;
    }

    // アクセスする URL
    // サーバーURL + /api/post/result でアクセス
    string urlGitHub = "";

    public void OnPointerClick(PointerEventData eventData)
    {
        // HTTP リクエストを非同期処理を待つためコルーチンとして呼び出す
        StartCoroutine("PostPointData");
    }

    // POST リクエストする本体
    IEnumerator PostPointData()
    {
        // HTTP リクエストする(POST メソッド) UnityWebRequest を呼び出し
        // アクセスする先は変数 urlGitHub で設定
        UnityWebRequest request = new UnityWebRequest(urlGitHub, "POST");


        // PointRequestData ベースクラスを器として呼び出す
        PointRequestData pointRequestData = new PointRequestData();
        // データを設定
        // 現在のポイントを得る
        pointRequestData.point = GameObject.Find("ClickPart").GetComponent<Term2_2_Chapter02_ClickPart>().currentPoint;
        // 自分の名前を登録
        pointRequestData.name = "Seigo";

        // 送信データを JsonUtility.ToJson で JSON 文字列を作成
        // pointRequestData の構造に基づいて変換してくれる
        string strJSON = JsonUtility.ToJson(pointRequestData);
        Debug.Log($"strJSON : {strJSON}");
        // 送信データを Encoding.UTF8.GetBytes で byte データ化
        byte[] bodyRaw = Encoding.UTF8.GetBytes(strJSON);

        // アップロード（Unity→サーバ）のハンドラを作成
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        // ダウンロード（サーバ→Unity）のハンドラを作成
        request.downloadHandler = new DownloadHandlerBuffer();

        // JSON で送ると HTTP ヘッダーで宣言する
        request.SetRequestHeader("Content-Type", "application/json");

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

                // ResultResponseData クラスで Unity で扱えるデータ化
                ResultResponseData resultResponse = JsonUtility.FromJson<ResultResponseData>(request.downloadHandler.text);

                // StatusMessage にランキングを割り当て
                GameObject.Find("StatusMessage").GetComponent<TextMesh>().text = "登録されました！";

                break;
        }

    }
}
