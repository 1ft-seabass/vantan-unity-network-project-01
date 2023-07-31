using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator �̂��߂̎Q��
using UnityEngine.Networking;   // UnityWebRequest �̂��߂̎Q��
using System.Text;              // Encoding �̂��߂̎Q��

public class Term1_2_Chapter02_ClickPart : MonoBehaviour, IPointerClickHandler
{
    // �A�N�Z�X���� URL
    // �T�[�o�[ URL + /api/post/add_point
    string urlGitHub = "�����ɃT�[�o�[URL������";

    // �|�C���g���Z�ݒ�
    int addPoint = 0;

    // �~�σ|�C���g
    int currentPoint = 0;

    void Start()
    {
        // �N�����Ƀf�[�^��ǂݍ���

        // HTTP ���N�G�X�g��񓯊�������҂��߃R���[�`���Ƃ��ČĂяo��
        StartCoroutine("RequestAddPointSetting");

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
    IEnumerator RequestAddPointSetting()
    {
        // HTTP ���N�G�X�g����(POST ���\�b�h) UnityWebRequest ���Ăяo��
        // �A�N�Z�X�����͕ϐ� urlGitHub �Őݒ�
        UnityWebRequest request = new UnityWebRequest(urlGitHub, "POST");
        // ��ł͂��Ƃ�ł��Ȃ��̂ŁA����͉��� dummy ������p��
        byte[] bodyRaw = Encoding.UTF8.GetBytes("dummy");
        // �A�b�v���[�h�iUnity���T�[�o�j�̃n���h�����쐬
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        // �_�E�����[�h�i�T�[�o��Unity�j�̃n���h�����쐬
        request.downloadHandler = new DownloadHandlerBuffer();

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

                // �󂯎�������Z�|�C���g�ݒ��ϐ��ɋL�^
                // ������Ȃ̂� int.Parse �ŕϊ�
                addPoint = int.Parse(request.downloadHandler.text);

                // �e�L�X�g�ɔ��f
                string infomation = "AddPoint +" + request.downloadHandler.text + "pt";
                GameObject.Find("AddPointMessage").GetComponent<TextMesh>().text = infomation;

                break;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
