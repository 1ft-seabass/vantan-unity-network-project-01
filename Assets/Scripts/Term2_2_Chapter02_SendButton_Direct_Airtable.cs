using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;           // IEnumerator �̂��߂̎Q��
using UnityEngine.Networking;       // UnityWebRequest �̂��߂̎Q��
using System;                       // Serializable �̂��߂̎Q��
using System.Text;                  // Encoding �̂��߂̎Q��
using System.Collections.Generic;   // List �̂��߂̎Q��

public class Term2_2_Chapter02_SendButton_Direct_Airtable : MonoBehaviour, IPointerClickHandler
{

    // Airtable �� Base ID
    string settingAirtableBaseID = "settingAirtableBaseID";

    // Airtable �� API �L�[
    string settingAirtableAPIKey = "settingAirtableAPIKey";

    // Airtable �� Table ��
    string settingAirtableTable = "PointList";

    // ��M���� JSON �f�[�^�� Unity �ň����f�[�^�ɂ��� ResponseData �x�[�X�N���X
    [Serializable]
    public class ResponseData
    {
        public List<ResponseDataRecord> records;
    }

    [Serializable]
    public class ResponseDataRecord
    {
        public string id;
        public string createdTime;
        public ResponseDataRecordField fields;
    }

    // ResponseDataRecordField �� Table �̗�̓��e�ɂ���ĕύX���܂�
    [Serializable]
    public class ResponseDataRecordField
    {
        // �t�B�[���h�� Color �� string �^
        public string Color;
        // �t�B�[���h�� ID �� string �^
        public string ID;
    }

    // ���M���� Unity �f�[�^�� JSON �f�[�^������ RequestData �x�[�X�N���X
    [Serializable]
    public class RequestData
    {
        public List<RequestDataRecord> records;
    }

    [Serializable]
    public class RequestDataRecord
    {
        public RequestDataRecordField fields;
    }

    // RequestDataRecordField �� Table �̗�̓��e�ɂ���ĕύX���܂�
    [Serializable]
    public class RequestDataRecordField
    {
        public int Point;
        public string Name;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        // HTTP ���N�G�X�g��񓯊�������҂��߃R���[�`���Ƃ��ČĂяo��
        StartCoroutine("PostPointData");
    }

    // POST ���N�G�X�g����{��
    IEnumerator PostPointData()
    {
        // API URL �쐬
        string urlAirtableAPI = "https://api.airtable.com/v0/" + settingAirtableBaseID + "/" + settingAirtableTable;
        // API URL �� Uri.AbsoluteUri �ƒʂ��� URL �p�����[�^�������Ė߂�
        urlAirtableAPI = new Uri(urlAirtableAPI).AbsoluteUri;

        // HTTP ���N�G�X�g����(POST ���\�b�h) UnityWebRequest ���Ăяo��
        UnityWebRequest request = new UnityWebRequest(urlAirtableAPI, "POST");

        // RequestData �x�[�X�N���X����Ƃ��ČĂяo��
        RequestData requestData = new RequestData();
        requestData.records = new List<RequestDataRecord>();
        // ����̒l
        RequestDataRecord requestDataRecord = new RequestDataRecord();
        requestDataRecord.fields = new RequestDataRecordField();
        // �f�[�^��ݒ�
        // ���݂̃|�C���g�𓾂�
        requestDataRecord.fields.Point = GameObject.Find("ClickPart").GetComponent<Term2_2_Chapter02_ClickPart>().currentPoint;
        // �����̖��O��o�^
        requestDataRecord.fields.Name = "Seigo";
        // records �ɉ�����
        requestData.records.Add(requestDataRecord);

        // ���M�f�[�^�� JsonUtility.ToJson �� JSON ��������쐬
        // requestData �̍\���Ɋ�Â��ĕϊ����Ă����
        string strJSON = JsonUtility.ToJson(requestData);
        Debug.Log($"strJSON : {strJSON}");
        // ���M�f�[�^�� Encoding.UTF8.GetBytes �� byte �f�[�^��
        byte[] bodyRaw = Encoding.UTF8.GetBytes(strJSON);

        // �A�b�v���[�h�iUnity���T�[�o�j�̃n���h�����쐬
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        // �_�E�����[�h�i�T�[�o��Unity�j�̃n���h�����쐬
        request.downloadHandler = new DownloadHandlerBuffer();

        // JSON �ő���� HTTP �w�b�_�[�Ő錾����
        request.SetRequestHeader("Content-Type", "application/json");

        // API �L�[�� HTTP �w�b�_�[�ɐݒ�
        request.SetRequestHeader("Authorization", "Bearer " + settingAirtableAPIKey);

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

                // ResponseData �N���X�� Unity �ň�����f�[�^��
                ResponseData resultResponse = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);

                // StatusMessage �Ƀ����L���O�����蓖��
                GameObject.Find("StatusMessage").GetComponent<TextMesh>().text = "�o�^����܂����I";

                break;
        }

    }
}
