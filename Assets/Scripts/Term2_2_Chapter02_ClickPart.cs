using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator �̂��߂̎Q��
using UnityEngine.Networking;   // UnityWebRequest �̂��߂̎Q��
using System.Text;              // Encoding �̂��߂̎Q��
using System;                   // Serializable �̂��߂̎Q��

public class Term2_2_Chapter02_ClickPart : MonoBehaviour, IPointerClickHandler
{

    // �|�C���g���Z�ݒ�
    int addPoint = 1;

    // �~�σ|�C���g
    public int currentPoint = 0;

    void Start()
    {
        // �~�σ|�C���g�̃��Z�b�g
        currentPoint = 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // �|�C���g���Z
        currentPoint += addPoint;

        // �e�L�X�g�ɔ��f
        string infomation = currentPoint + "pt";
        GameObject.Find("CurrentPointMessage").GetComponent<TextMesh>().text = infomation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
