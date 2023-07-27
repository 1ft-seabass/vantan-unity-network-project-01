using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator �̂��߂̎Q��
using UnityEngine.Networking;   // UnityWebRequest �̂��߂̎Q��
using System.Text;              // Encoding �̂��߂̎Q��

public class Scene04WelcomeMessage : MonoBehaviour
{
    // �A�N�Z�X���� URL
    // �T�[�o�[ URL + /api/post/title
    string urlGitHub = "�����ɃT�[�o�[URL������";

    void Start()
    {
        // �N�����Ƀf�[�^��ǂݍ���

        // HTTP ���N�G�X�g��񓯊�������҂��߃R���[�`���Ƃ��ČĂяo��
        StartCoroutine("RequestWelcomeMessage");
    }

    // ���N�G�X�g����{��
    IEnumerator RequestWelcomeMessage()
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

                // �e�L�X�g�ɔ��f
                string message = request.downloadHandler.text;
                this.gameObject.GetComponent<TextMesh>().text = message;

                break;
        }


    }

}
