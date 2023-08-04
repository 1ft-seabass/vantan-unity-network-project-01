using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator のための参照
using UnityEngine.Networking;   // UnityWebRequest のための参照
using System.Text;              // Encoding のための参照
using System;                   // Serializable のための参照

public class Term1_3_Chapter02_ClickPart : MonoBehaviour, IPointerClickHandler
{
    // 受信した JSON データを Unity で扱うデータにする ResponseData ベースクラス
    [Serializable]
    public class ResponseData
    {
        // result というプロパティ名で string 型で変換
        public string result;
    }

    // 送信する Unity データを JSON データ化する RequestData ベースクラス
    [Serializable]
    public class RequestData
    {   
        // 自分の名前
        // name というプロパティ名で string 型で変換
        public string name;
    }

    // アクセスする URL
    // サーバー URL + /api/post/init
    string urlGitHub = "ここにサーバーURLを入れる";

    // ポイント加算設定
    int addPoint = 1;

    // 蓄積ポイント
    public int currentPoint = 0;

    void Start()
    {
        // 起動時にデータを読み込む

        // HTTP リクエストを非同期処理を待つためコルーチンとして呼び出す
        StartCoroutine("RequestInit");

        // 蓄積ポイントのリセット
        currentPoint = 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // マウスクリックイベント
        // Debug.Log($"オブジェクト {this.name} がクリックされたよ！");

        // ポイント加算
        currentPoint += addPoint;

        // テキストに反映
        string infomation = currentPoint + "pt";
        GameObject.Find("CurrentPointMessage").GetComponent<TextMesh>().text = infomation;
    }

    // リクエストする本体
    IEnumerator RequestInit()
    {
        // HTTP リクエストする(POST メソッド) UnityWebRequest を呼び出し
        // アクセスする先は変数 urlGitHub で設定
        UnityWebRequest request = new UnityWebRequest(urlGitHub, "POST");

        // ResponseData ベースクラスを器として呼び出す
        RequestData requestData = new RequestData();
        // データを設定

        // 送信データを JsonUtility.ToJson で JSON 文字列を作成
        // pointRequestData の構造に基づいて変換してくれる
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

        // リクエスト開始
        Debug.Log("リクエスト開始");
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
                ResponseData responseData = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);

                // StatusMessage に結果テキスト割り当て
                GameObject.Find("StatusMessage").GetComponent<TextMesh>().text = responseData.result;

                break;
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
