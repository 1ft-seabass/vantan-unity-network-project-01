using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator �̂��߂̎Q��
using UnityEngine.Networking;   // UnityWebRequest �̂��߂̎Q��
using System;                   // Serializable �̂��߂̎Q��

public class Term1_2_Chapter03_CubeEvent : MonoBehaviour, IPointerClickHandler
{
    // JSON �f�[�^������ NameData �x�[�X�N���X
    [Serializable]
    public class NameData
    {
        // name �Ƃ����v���p�e�B���� string �^�ŕϊ�
        public string name;
        // name2 �Ƃ����v���p�e�B���� string �^�ŕϊ�
        public string name2;
    }

    // �A�N�Z�X���� URL
    // �T�[�o�[URL + /api/get/json �ŃA�N�Z�X
    string urlGitHub = "�����ɃT�[�o�[URL������";

    public void OnPointerClick(PointerEventData eventData)
    {
        // �}�E�X�N���b�N�C�x���g
        // Debug.Log($"�I�u�W�F�N�g {this.name} ���N���b�N���ꂽ��I");

        // HTTP GET ���N�G�X�g��񓯊�������҂��߃R���[�`���Ƃ��ČĂяo��
        StartCoroutine("GetGitHubData");
    }

    // GET ���N�G�X�g����{��
    IEnumerator GetGitHubData()
    {
        // HTTP ���N�G�X�g����(GET ���\�b�h) UnityWebRequest ���Ăяo��
        // �A�N�Z�X�����͕ϐ� urlGitHub �Őݒ�
        UnityWebRequest request = UnityWebRequest.Get(urlGitHub);

        // ���N�G�X�g�J�n
        yield return request.SendWebRequest();

        // ���ʂɂ���ĕ���
        switch (request.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("���N�G�X�g��");
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log("���N�G�X�g����");

                // �R���\�[���ɕ\��
                Debug.Log($"responseData: {request.downloadHandler.text}");

                // ���̂����� NameData �N���X�� Unity �ň�����f�[�^��
                NameData nameData = JsonUtility.FromJson<NameData>(request.downloadHandler.text);

                // Title �Ƀe�L�X�g���蓖��
                GameObject.Find("Title").GetComponent<TextMesh>().text = "Loaded!";

                break;
        }


    }
}