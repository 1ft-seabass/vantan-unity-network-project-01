using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator �̂��߂̎Q��
using UnityEngine.Networking;   // UnityWebRequest �̂��߂̎Q��
using System;                   // Serializable �̂��߂̎Q��

using System.Collections.Generic; // List �̂��߂̎Q��

public class Term2_1_Chapter03_CubeEvent01_OK_Sample : MonoBehaviour, IPointerClickHandler
{
    // API �̐ڑ���
    // ����� �T�[�o�[URL + /api/get ��ǂݍ���
    string urlAPI = "";

    // ��M���� JSON �f�[�^�� Unity �ň����f�[�^�ɂ��� ResponseData �x�[�X�N���X
    // ����� {"data":["A","B","C","D"]} �Ƃ��� data �̃I�u�W�F�N�g�̒��ɔz�񂪓����Ă��܂�
    [Serializable]
    public class ResponseData
    {
        public List<string> data;
    }

    void Start()
    {

    }

    void Update()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // �}�E�X�N���b�N�C�x���g

        // HTTP ���N�G�X�g��񓯊�������҂��߃R���[�`���Ƃ��ČĂяo��
        StartCoroutine("GetData");
    }

    IEnumerator GetData()
    {
        // HTTP ���N�G�X�g����(GET ���\�b�h) UnityWebRequest ���Ăяo��
        // �A�N�Z�X�����͕ϐ� urlGitHub �Őݒ�
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

                // �f�[�^��������O�ɏo��
                response.data.ForEach(Debug.Log);

                break;
        }


    }
}