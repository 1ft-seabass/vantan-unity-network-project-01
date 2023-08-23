using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;           // IEnumerator �̂��߂̎Q��
using UnityEngine.Networking;       // UnityWebRequest �̂��߂̎Q��
using System;                       // Serializable �̂��߂̎Q��
using System.Collections.Generic;   // List �̂��߂̎Q��

public class Sample01_GetAPI : MonoBehaviour
{
    // �A�N�Z�X���� URL
    // �T�[�o�[URL + /api/get �ŃA�N�Z�X
    string urlAPI = "";

    // ��M���� JSON �f�[�^�� Unity �ň����f�[�^�ɂ��� ResponseData �x�[�X�N���X
    [Serializable]
    public class ResponseData
    {
        public List<ResponseDataList> data;
    }

    [Serializable]
    public class ResponseDataList
    {
        public string Name;
        public List<ResponseDataListImage> Image;
    }

    // Image ��̃f�[�^�̒��g
    [Serializable]
    public class ResponseDataListImage
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
        // HTTP ���N�G�X�g����(GET ���\�b�h) UnityWebRequest ���Ăяo��
        // �A�N�Z�X�����͕ϐ� urlGitHub �Őݒ�
        UnityWebRequest request = UnityWebRequest.Get(urlAPI);

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
                ResponseData response = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);

                for (int i = 0; i < 3 ; i++)
                {
                    // ������� Tile
                    GameObject tile = GameObject.Find("Tile" + i.ToString());

                    // ����̍s�f�[�^
                    ResponseDataList currentLine = response.data[i];

                    // �e�L�X�g���f
                    tile.transform.Find("MessageText").GetComponent<TextMesh>().text = currentLine.Name;

                    // �摜�ǂݍ���
                    StartCoroutine(GetTexture( currentLine.Image[0].url, tile ));
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
