using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator �̂��߂̎Q��
using UnityEngine.Networking;   // UnityWebRequest �̂��߂̎Q��
using System;                   // Serializable �̂��߂̎Q��
using System.Text;              // Encoding �̂��߂̎Q��

public class Term2_2_Chapter02_SendButton_OK_Sample : MonoBehaviour, IPointerClickHandler
{
    // ��M���� JSON �f�[�^�� Unity �ň����f�[�^�ɂ��� ResultResponseData �x�[�X�N���X
    [Serializable]
    public class ResultResponseData
    {
        // result �Ƃ����v���p�e�B���� string �^�ŕϊ�
        public string result;
        // recordPoint �Ƃ����v���p�e�B���� int �^�ŕϊ�
        public int recordPoint;
    }

    // ���M���� Unity �f�[�^�� JSON �f�[�^������ PointRequestData �x�[�X�N���X
    [Serializable]
    public class PointRequestData
    {
        // point �Ƃ����v���p�e�B���� int �^�ŕϊ�
        public int point;
        // name �Ƃ����v���p�e�B���� int �^�ŕϊ�
        public string name;
    }

    // �A�N�Z�X���� URL
    // �T�[�o�[URL + /api/post/result �ŃA�N�Z�X
    string urlGitHub = "";

    public void OnPointerClick(PointerEventData eventData)
    {
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
        // ���݂̃|�C���g�𓾂�
        pointRequestData.point = GameObject.Find("ClickPart").GetComponent<Term2_2_Chapter02_ClickPart>().currentPoint;
        // �����̖��O��o�^
        pointRequestData.name = "Seigo";

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
        Debug.Log("���N�G�X�g�J�n");

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

                // StatusMessage �Ƀ����L���O�����蓖��
                GameObject.Find("StatusMessage").GetComponent<TextMesh>().text = "�o�^����܂����I";

                break;
        }

    }
}