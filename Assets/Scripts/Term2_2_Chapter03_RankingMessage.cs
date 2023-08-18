using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections.Generic; // List �̂��߂̎Q��
using System.Collections;       // IEnumerator �̂��߂̎Q��
using UnityEngine.Networking;   // UnityWebRequest �̂��߂̎Q��
using System;                   // Serializable �̂��߂̎Q��
using System.Text;              // Encoding �̂��߂̎Q��


public class Term2_2_Chapter03_RankingMessage : MonoBehaviour
{
    // ��M���� JSON �f�[�^�� Unity �ň����f�[�^�ɂ��� ResponseData �x�[�X�N���X
    // ����� data �̒��g���z��ŁA����� Name , Point , CreatedTime �̃v���p�e�B�������Ă���̂ŁAList �̒��g���`���� ResponseDataList �^���쐬�B
    [Serializable]
    public class ResponseData
    {
        public List<ResponseDataList> data;
    }

    // Name , Point , CreatedTime �̃v���p�e�B�������Ă���̂ŁAList �̒��g���`���� ResponseDataList �^���쐬
    [Serializable]
    public class ResponseDataList
    {
        
    }

    void Start()
    {
        GetDataCore();
    }

    public void GetDataCore()
    {
        StartCoroutine("GetData");
    }

    // �A�N�Z�X���� URL
    // �T�[�o�[URL + /api/get/ranking �ŃA�N�Z�X
    string urlAPI = "";

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

                // �������������
                string textRankingList = "[Ranking]\n";

                Debug.Log(response.data);

                // �f�[�^��������f����
                for (int i = 0; i < 3; i++)
                {
                    ResponseDataList currentLine = response.data[i];

                    Debug.Log(currentLine);

                    // �������A��
                    textRankingList += "";
                }

                // ���b�Z�[�W�ɔ��f
                this.transform.GetComponent<TextMesh>().text = textRankingList;

                break;
        }


    }
}
