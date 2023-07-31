using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator のための参照
using UnityEngine.Networking;   // UnityWebRequest のための参照
using System.Text;              // Encoding のための参照

public class Term1_2_Chapter02_ClickPart : MonoBehaviour, IPointerClickHandler
{
    // アクセスする URL
    // サーバー URL + /api/post/add_point
    string urlGitHub = "ここにサーバーURLを入れる";

    // ポイント加算設定
    int addPoint = 0;

    // 蓄積ポイント
    int currentPoint = 0;

    void Start()
    {
        // 起動時にデータを読み込む

        // HTTP リクエストを非同期処理を待つためコルーチンとして呼び出す
        StartCoroutine("RequestAddPointSetting");

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
    IEnumerator RequestAddPointSetting()
    {
        // HTTP リクエストする(POST メソッド) UnityWebRequest を呼び出し
        // アクセスする先は変数 urlGitHub で設定
        UnityWebRequest request = new UnityWebRequest(urlGitHub, "POST");
        // 空ではやりとりできないので、今回は仮の dummy 文字を用意
        byte[] bodyRaw = Encoding.UTF8.GetBytes("dummy");
        // アップロード（Unity→サーバ）のハンドラを作成
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        // ダウンロード（サーバ→Unity）のハンドラを作成
        request.downloadHandler = new DownloadHandlerBuffer();

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

                // 受け取った加算ポイント設定を変数に記録
                // 文字列なので int.Parse で変換
                addPoint = int.Parse(request.downloadHandler.text);

                // テキストに反映
                string infomation = "AddPoint +" + request.downloadHandler.text + "pt";
                GameObject.Find("AddPointMessage").GetComponent<TextMesh>().text = infomation;

                break;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
