using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator �̂��߂̎Q��
using UnityEngine.Networking;   // UnityWebRequest �̂��߂̎Q��
using System;                   // Serializable �̂��߂̎Q��

using System.Collections.Generic; // List �̂��߂̎Q��

public class Term2_2_Chapter01_GetAPI_Direct_Airtable : MonoBehaviour
{
    // Airtable �� Base ID
    string settingAirtableBaseID = "settingAirtableBaseID";

    // Airtable �� API �L�[
    string settingAirtableAPIKey = "settingAirtableAPIKey";

    // Airtable �� Table ��
    string settingAirtableTable = "Sample02";

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
        // �t�B�[���h�� Color �� string �^
        public string Color;
        // �t�B�[���h�� ID �� string �^
        public string ID;
    }

    void Start()
    {
        // �J�n���ɓǂݍ��݊J�n
        GetDataCore();
    }

    void Update()
    {

    }

    void GetDataCore()
    {
        // HTTP ���N�G�X�g��񓯊�������҂��߃R���[�`���Ƃ��ČĂяo��
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

                // �f�[�^��������f����
                for (int i = 0; i < 3; i++)
                {
                    GameObject cube = GameObject.Find("Cube" + i.ToString());

                    // Debug.Log("Cube" + i.ToString());

                    string colorName = response.records[i].fields.Color;

                    // Debug.Log(colorName);

                    cube.transform.Find("MessageText").GetComponent<TextMesh>().text = colorName;

                    // �J���[�ύX
                    if (colorName == "Red")
                    {
                        cube.GetComponent<Renderer>().material.color = Color.red;
                    }
                    else if (colorName == "Yellow")
                    {
                        cube.GetComponent<Renderer>().material.color = Color.yellow;
                    }
                    else if (colorName == "Blue")
                    {
                        cube.GetComponent<Renderer>().material.color = Color.blue;
                    }
                }

                break;
        }


    }
}
