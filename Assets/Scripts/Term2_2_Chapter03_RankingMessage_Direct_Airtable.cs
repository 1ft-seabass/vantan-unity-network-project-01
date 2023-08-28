using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections.Generic; // List �̂��߂̎Q��
using System.Collections;       // IEnumerator �̂��߂̎Q��
using UnityEngine.Networking;   // UnityWebRequest �̂��߂̎Q��
using System;                   // Serializable �̂��߂̎Q��
using System.Text;              // Encoding �̂��߂̎Q��


public class Term2_2_Chapter03_RankingMessage_Direct_Airtable : MonoBehaviour
{

    // Airtable �� Base ID
    string settingAirtableBaseID = "settingAirtableBaseID";

    // Airtable �� API �L�[
    string settingAirtableAPIKey = "settingAirtableAPIKey";

    // Airtable �� Table ��
    string settingAirtableTable = "PointList";

    // Airtable �� view ��
    string settingAirtableView = "Grid view";

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
        // �t�B�[���h�� Name �� string �^
        public string Name;
        // �t�B�[���h�� Point �� int �^
        public string Point;
        // �t�B�[���h�� CreatedTime �� string �^
        public string CreatedTime;
    }


    void Start()
    {
        GetDataCore();
    }

    public void GetDataCore()
    {
        StartCoroutine("GetData");
    }


    IEnumerator GetData()
    {
        // API URL �쐬
        string urlAirtableAPI = "https://api.airtable.com/v0/" + settingAirtableBaseID + "/" + settingAirtableTable + "?view=" + settingAirtableView;
        // API URL �� Uri.AbsoluteUri �ƒʂ��� URL �p�����[�^����
        // �� ?view=Grid view �� ?view=Grid%20view �ɂȂ�
        urlAirtableAPI = new Uri(urlAirtableAPI).AbsoluteUri;

        // HTTP ���N�G�X�g����(GET ���\�b�h) UnityWebRequest ���Ăяo��
        UnityWebRequest request = UnityWebRequest.Get(urlAirtableAPI);

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

            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("ProtocolError");
                Debug.Log($"responseCode : {request.responseCode}");
                Debug.Log($"error : {request.error}");
                break;

            case UnityWebRequest.Result.ConnectionError:
                Debug.Log("ConnectionError");
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log("���N�G�X�g����");

                // �R���\�[���ɕ\��
                Debug.Log($"responseData: {request.downloadHandler.text}");

                // ResponseData �N���X�� Unity �ň�����f�[�^��
                ResponseData response = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);

                // �������������
                string textRankingList = "[Ranking]\n";

                Debug.Log($"�f�[�^�� : {response.records.Count}");

                // �f�[�^��������f����
                for (int i = 0; i < 3; i++)
                {
                    ResponseDataRecord record = response.records[i];
                    ResponseDataRecordField fields = record.fields;

                    // �������A��
                    textRankingList += "[" + i.ToString() + "]" + fields.Name + " " + fields.Point.ToString() + "pt" + "\n";
                }

                // ���b�Z�[�W�ɔ��f
                this.transform.GetComponent<TextMesh>().text = textRankingList;

                break;
        }


    }
}
