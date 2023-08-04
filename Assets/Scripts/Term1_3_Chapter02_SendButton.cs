using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator �̂��߂̎Q��
using UnityEngine.Networking;   // UnityWebRequest �̂��߂̎Q��
using System;                   // Serializable �̂��߂̎Q��
using System.Text;              // Encoding �̂��߂̎Q��

public class Term1_3_Chapter02_SendButton : MonoBehaviour, IPointerClickHandler
{
    // Term1_3_Chapter01_CubeEvent �Ƃقړ���

    // ��M���� JSON �f�[�^�� Unity �ň����f�[�^�ɂ��� ResultResponseData �x�[�X�N���X
    [Serializable]
    public class ResultResponseData
    {
        // result �Ƃ����v���p�e�B���� string �^�ŕϊ�
        public string result;
    }

    // ���M���� Unity �f�[�^�� JSON �f�[�^������ PointRequestData �x�[�X�N���X
    [Serializable]
    public class PointRequestData
    {
        // point �Ƃ����v���p�e�B���� int �^�ŕϊ�
        public int point;
    }

    // �A�N�Z�X���� URL
    // �T�[�o�[URL + /api/post/result �ŃA�N�Z�X
    string urlGitHub = "�����ɃT�[�o�[URL������";

    public void OnPointerClick(PointerEventData eventData)
    {
        // �}�E�X�N���b�N�C�x���g
        // Debug.Log($"�I�u�W�F�N�g {this.name} ���N���b�N���ꂽ��I");

        // HTTP ���N�G�X�g��񓯊�������҂��߃R���[�`���Ƃ��ČĂяo��
        StartCoroutine("PostPointData");
    }

    // POST ���N�G�X�g����{��
    IEnumerator PostPointData()
    {
        // HTTP ���N�G�X�g����(POST ���\�b�h) UnityWebRequest ���Ăяo��
        // �A�N�Z�X�����͕ϐ� urlGitHub �Őݒ�
        UnityWebRequest request = new UnityWebRequest(urlGitHub, "POST");


        // PointRequestData �x�[�X�N���X����Ƃ��ČĂяo��
        PointRequestData pointRequestData = new PointRequestData();
        // �f�[�^��ݒ�
        pointRequestData.point = 1000; // �_�~�[�� 1000 pt �̃Q�[�����ʂ𑗂�

        // ���M�f�[�^�� JsonUtility.ToJson �� JSON ��������쐬
        // pointRequestData �̍\���Ɋ�Â��ĕϊ����Ă����
        string strJSON = JsonUtility.ToJson(pointRequestData);
        Debug.Log($"strJSON : {strJSON}");
        // ���M�f�[�^�� Encoding.UTF8.GetBytes �� byte �f�[�^��
        byte[] bodyRaw = Encoding.UTF8.GetBytes(strJSON);

        // �A�b�v���[�h�iUnity���T�[�o�j�̃n���h�����쐬
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        // �_�E�����[�h�i�T�[�o��Unity�j�̃n���h�����쐬
        request.downloadHandler = new DownloadHandlerBuffer();

        // JSON �ő���� HTTP �w�b�_�[�Ő錾����
        request.SetRequestHeader("Content-Type", "application/json");

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

                // ResultResponseData �N���X�� Unity �ň�����f�[�^��
                ResultResponseData resultResponse = JsonUtility.FromJson<ResultResponseData>(request.downloadHandler.text);

                // MessageText �Ɍ��ʃe�L�X�g���蓖��
                GameObject.Find("StatusMessage").GetComponent<TextMesh>().text = resultResponse.result;

                break;
        }

    }
}