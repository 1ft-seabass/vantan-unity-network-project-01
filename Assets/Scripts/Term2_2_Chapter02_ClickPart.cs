using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator のための参照
using UnityEngine.Networking;   // UnityWebRequest のための参照
using System.Text;              // Encoding のための参照
using System;                   // Serializable のための参照

public class Term2_2_Chapter02_ClickPart : MonoBehaviour, IPointerClickHandler
{

    // ポイント加算設定
    int addPoint = 1;

    // 蓄積ポイント
    public int currentPoint = 0;

    void Start()
    {
        // 蓄積ポイントのリセット
        currentPoint = 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // ポイント加算
        currentPoint += addPoint;

        // テキストに反映
        string infomation = currentPoint + "pt";
        GameObject.Find("CurrentPointMessage").GetComponent<TextMesh>().text = infomation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
