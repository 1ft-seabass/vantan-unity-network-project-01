using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator �̂��߂̎Q��
using UnityEngine.Networking;   // UnityWebRequest �̂��߂̎Q��
using System.Text;              // Encoding �̂��߂̎Q��
using System;                   // Serializable �̂��߂̎Q��

public class Term1_3_Chapter02_ClickPart : MonoBehaviour, IPointerClickHandler
{
    // ��M���� JSON �f�[�^�� Unity �ň����f�[�^�ɂ��� ResponseData �x�[�X�N���X
    [Serializable]
    public class ResponseData
    {
        // result �Ƃ����v���p�e�B���� string �^�ŕϊ�
        public string result;
    }

    // ���M���� Unity �f�[�^�� JSON �f�[�^������ RequestData �x�[�X�N���X
    [Serializable]
    public class RequestData
    {   
        // �����̖��O
        // name �Ƃ����v���p�e�B���� string �^�ŕϊ�
        public string name;
    }

    // �A�N�Z�X���� URL
    // �T�[�o�[ URL + /api/post/init
    string urlGitHub = "�����ɃT�[�o�[URL������";

    // �|�C���g���Z�ݒ�
    int addPoint = 1;

    // �~�σ|�C���g
    public int currentPoint = 0;

    void Start()
    {
        // �N�����Ƀf�[�^��ǂݍ���

        // HTTP ���N�G�X�g��񓯊�������҂��߃R���[�`���Ƃ��ČĂяo��
        StartCoroutine("RequestInit");

        // �~�σ|�C���g�̃��Z�b�g
        currentPoint = 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // �}�E�X�N���b�N�C�x���g
        // Debug.Log($"�I�u�W�F�N�g {this.name} ���N���b�N���ꂽ��I");

        // �|�C���g���Z
        currentPoint += addPoint;

        // �e�L�X�g�ɔ��f
        string infomation = currentPoint + "pt";
        GameObject.Find("CurrentPointMessage").GetComponent<TextMesh>().text = infomation;
    }

    // ���N�G�X�g����{��
    IEnumerator RequestInit()
    {
        // HTTP ���N�G�X�g����(POST ���\�b�h) UnityWebRequest ���Ăяo��
        // �A�N�Z�X�����͕ϐ� urlGitHub �Őݒ�
        UnityWebRequest request = new UnityWebRequest(urlGitHub, "POST");

        // ResponseData �x�[�X�N���X����Ƃ��ČĂяo��
        RequestData requestData = new RequestData();
        // �f�[�^��ݒ�

        // ���M�f�[�^�� JsonUtility.ToJson �� JSON ��������쐬
        // pointRequestData �̍\���Ɋ�Â��ĕϊ����Ă����
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

        // ���N�G�X�g�J�n
        Debug.Log("���N�G�X�g�J�n");
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
                ResponseData responseData = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);

                // StatusMessage �Ɍ��ʃe�L�X�g���蓖��
                GameObject.Find("StatusMessage").GetComponent<TextMesh>().text = responseData.result;

                break;
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
