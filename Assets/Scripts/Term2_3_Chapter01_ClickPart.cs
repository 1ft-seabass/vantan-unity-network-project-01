using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator のための参照
using UnityEngine.Networking;   // UnityWebRequest のための参照
using System;                   // Serializable のための参照
using System.Text;              // Encoding のための参照

using System.Collections.Generic;

public class Term2_3_Chapter01_ClickPart : MonoBehaviour, IPointerClickHandler
{
    // 受信した JSON データを Unity で扱うデータにする ResponseData ベースクラス
    // API仕様 : https://platform.openai.com/docs/api-reference/completions/object
    [Serializable]
    public class ResponseData
    {
        public string id;
        public string @object; // object は予約語なので @ を使ってエスケープしています
        public int created;
        public List<ResponseDataChoice> choices;
        public ResponseDataUsage usage;
    }

    [Serializable]
    public class ResponseDataUsage
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }
    [Serializable]
    public class ResponseDataChoice
    {
        public int index;
        public RequestDataMessages message;
        public string finish_reason;
    }

    // 送信する Unity データを JSON データ化する PointRequestData ベースクラス
    [Serializable]
    public class RequestData
    {
        public string model;
        public List<RequestDataMessages> messages;
    }

    [Serializable]
    public class RequestDataMessages
    {
        public string role;
        public string content;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // HTTP リクエストを非同期処理を待つためコルーチンとして呼び出す
        StartCoroutine("PostChatGPT");
    }

    // ChatGPT の API キーを入力
    string tokenCHatGPT = "tokenCHatGPT";

    // リクエストする本体
    IEnumerator PostChatGPT()
    {
        // HTTP リクエストする(POST メソッド) UnityWebRequest を呼び出し
        // リクエスト仕様 : https://platform.openai.com/docs/guides/gpt/chat-completions-api
        // API仕様 : https://platform.openai.com/docs/api-reference/completions/object
        UnityWebRequest request = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST");

        RequestData requestData = new RequestData();
        // データを設定
        requestData.model = "gpt-3.5-turbo-0613";
        RequestDataMessages currentMessage = new RequestDataMessages();
        // ロールは user
        currentMessage.role = "user";
        // 実際の質問
        currentMessage.content = "こんにちは！";
        List<RequestDataMessages> currentMessages = new List<RequestDataMessages>();
        currentMessages.Add(currentMessage);
        requestData.messages = currentMessages;
        Debug.Log($"currentMessages[0].content : {currentMessages[0].content}");

        // 送信データを JsonUtility.ToJson で JSON 文字列を作成
        // RequestData, RequestDataMessages の構造に基づいて変換してくれる
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
        // ChatGPT 用の認証を伝える設定
        request.SetRequestHeader("Authorization", $"Bearer {tokenCHatGPT}");

        // リクエスト開始
        yield return request.SendWebRequest();

        Debug.Log("リクエスト...");

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

                ResponseData resultResponse = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);

                // 返答
                Debug.Log($"resultResponse.choices[0].message : {resultResponse.choices[0].message.content}");

                // テキストを反映
                GameObject.Find("MessageText").GetComponent<TextMesh>().text = resultResponse.choices[0].message.content;

                break;
        }


    }
}