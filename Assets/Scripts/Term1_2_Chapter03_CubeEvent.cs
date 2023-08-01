using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator のための参照
using UnityEngine.Networking;   // UnityWebRequest のための参照
using System;                   // Serializable のための参照

public class Term1_2_Chapter03_CubeEvent : MonoBehaviour, IPointerClickHandler
{
    // JSON データ化する NameData ベースクラス
    [Serializable]
    public class NameData
    {
        // name というプロパティ名で string 型で変換
        public string name;
        // name2 というプロパティ名で string 型で変換
        public string name2;
    }

    // アクセスする URL
    // サーバーURL + /api/get/json でアクセス
    string urlGitHub = "ここにサーバーURLを入れる";

    public void OnPointerClick(PointerEventData eventData)
    {
        // マウスクリックイベント
        // Debug.Log($"オブジェクト {this.name} がクリックされたよ！");

        // HTTP GET リクエストを非同期処理を待つためコルーチンとして呼び出す
        StartCoroutine("GetGitHubData");
    }

    // GET リクエストする本体
    IEnumerator GetGitHubData()
    {
        // HTTP リクエストする(GET メソッド) UnityWebRequest を呼び出し
        // アクセスする先は変数 urlGitHub で設定
        UnityWebRequest request = UnityWebRequest.Get(urlGitHub);

        // リクエスト開始
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

                // そのうえで NameData クラスで Unity で扱えるデータ化
                NameData nameData = JsonUtility.FromJson<NameData>(request.downloadHandler.text);

                // Title にテキスト割り当て
                GameObject.Find("Title").GetComponent<TextMesh>().text = "Loaded!";

                break;
        }


    }
}