using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;           // IEnumerator �̂��߂̎Q��
using UnityEngine.Networking;       // UnityWebRequest �̂��߂̎Q��
using System;                       // Serializable �̂��߂̎Q��
using System.Collections.Generic;   // List �̂��߂̎Q��

public class Sample01_GetAPI_Direct_Airtable : MonoBehaviour
{
    // Airtable �� Base ID
    string settingAirtableBaseID = "settingAirtableBaseID";

    // Airtable �� API �L�[
    string settingAirtableAPIKey = "settingAirtableAPIKey";

    // Airtable �� Table ��
    string settingAirtableTable = "ImageList";

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
        // �t�B�[���h�� Image �ɂ͉摜�֘A�̏�񂪓����Ă���
        public List<ResponseDataRecordFieldListImage> Image;
    }

    // Image ��̃f�[�^�̒��g
    [Serializable]
    public class ResponseDataRecordFieldListImage
    {
        public int width;
        public int height;
        public string url;
    }

    void Start()
    {
        StartCoroutine("GetAPI");
    }

    IEnumerator GetAPI()
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

                for (int i = 0; i < 3; i++)
                {
                    // ������� Tile
                    GameObject tile = GameObject.Find("Tile" + i.ToString());

                    // ����̍s�f�[�^
                    ResponseDataRecord currentLine = response.records[i];

                    // �e�L�X�g���f
                    tile.transform.Find("MessageText").GetComponent<TextMesh>().text = currentLine.fields.Name;

                    // �摜�ǂݍ���
                    StartCoroutine(GetTexture(currentLine.fields.Image[0].url, tile));
                }

                break;
        }


    }

    // �e�N�X�`���ǂݍ���
    IEnumerator GetTexture(string url, GameObject tile)
    {
        // �e�N�X�`���� GET ���N�G�X�g�œǂݍ��ށB�u���E�U�ł������B
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        // ���N�G�X�g�J�n
        yield return request.SendWebRequest();

        Debug.Log("���N�G�X�g�J�n");

        // ���ʂɂ���ĕ���
        switch (request.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("���N�G�X�g��");
                break;

            case UnityWebRequest.Result.ConnectionError:
                Debug.Log("ConnectionError");
                Debug.Log(request.error);
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("ProtocolError");
                Debug.Log(request.responseCode);
                Debug.Log(request.error);
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log("���N�G�X�g����");

                // �e�N�X�`���Ɋ��蓖��
                Texture loadedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

                tile.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", loadedTexture);

                break;
        }
    }
}
