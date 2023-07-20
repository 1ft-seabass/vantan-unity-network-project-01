using UnityEngine;
using UnityEngine.EventSystems;

public class CubeEvent : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // マウスクリックイベント
        Debug.Log($"オブジェクト {this.name} がクリックされたよ！");
    }
}