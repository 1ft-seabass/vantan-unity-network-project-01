using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator �̂��߂̎Q��
using UnityEngine.Networking;   // UnityWebRequest �̂��߂̎Q��
using System;                   // Serializable �̂��߂̎Q��

using System.Collections.Generic; // List �̂��߂̎Q��

public class Term2_2_Chapter01_GetAPI_OK_Sample : MonoBehaviour
{
    // API �̐ڑ���
    // ����� �T�[�o�[URL + /api/get ��ǂݍ���
    string urlAPI = "";

    // ��M���� JSON �f�[�^�� Unity �ň����f�[�^�ɂ��� ResponseData �x�[�X�N���X
    // ����� {"data":["Red","Yello","Blue"]} �Ƃ��� data �̃I�u�W�F�N�g�̒��ɔz�񂪓����Ă��܂�
    [Serializable]
    public class ResponseData
    {
        public List<string> data;
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
        // HTTP ���N�G�X�g����(GET ���\�b�h) UnityWebRequest ���Ăяo��
        // �A�N�Z�X�����͕ϐ� urlAPI �Őݒ�
        UnityWebRequest request = UnityWebRequest.Get(urlAPI);

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

                // ResponseData �N���X�� Unity �ň�����f�[�^��
                ResponseData response = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);

                // �f�[�^��������f����
                for (int i = 0; i < 3; i++)
                {
                    GameObject cube = GameObject.Find("Cube" + i.ToString());

                    // Debug.Log("Cube" + i.ToString());

                    string colorName = response.data[i];

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
