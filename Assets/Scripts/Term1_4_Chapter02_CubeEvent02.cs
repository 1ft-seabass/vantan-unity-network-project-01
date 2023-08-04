using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator �̂��߂̎Q��
using UnityEngine.Networking;   // UnityWebRequest �̂��߂̎Q��
using System.Collections.Generic; // List�̂���

public class Term1_4_Chapter02_CubeEvent02 : MonoBehaviour, IPointerClickHandler
{

    // LINE Notify �̃g�[�N��
    string tokenLINENotify = "token";

    void Start()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // HTTP ���N�G�X�g��񓯊�������҂��߃R���[�`���Ƃ��ČĂяo��
        // �� 2 �����ő��肽������������
        StartCoroutine("PostLINENofify", "Unity����e�X�g");
    }

    IEnumerator PostLINENofify(string message)
    {
        // IMultipartFormSection �� multipart/form-data �̃f�[�^�Ƃ��đ���܂�
        // https://docs.unity3d.com/ja/2018.4/Manual/UnityWebRequest-SendingForm.html
        // https://docs.unity3d.com/ja/2019.4/ScriptReference/Networking.IMultipartFormSection.html
        // https://docs.unity3d.com/ja/2020.3/ScriptReference/Networking.MultipartFormDataSection.html
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("message", message));

        // HTTP ���N�G�X�g����(POST ���\�b�h) UnityWebRequest ���Ăяo��
        // �A�N�Z�X�����͕ϐ� urlGitHub �Őݒ�
        // �� 2 �����ŏ�L�̃t�H�[���f�[�^�����蓖�Ă� multipart/form-data �̃f�[�^�Ƃ��đ���܂�
        UnityWebRequest request = UnityWebRequest.Post("https://notify-api.line.me/api/notify", formData);

        // LINE Notify �̔F�؂� Authorization �w�b�_�[�� Bearer �̂��Ƃ� API �g�[�N��������
        request.SetRequestHeader("Authorization", $"Bearer {tokenLINENotify}");

        // �_�E�����[�h�i�T�[�o��Unity�j�̃n���h�����쐬
        request.downloadHandler = new DownloadHandlerBuffer();

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

                break;
        }


    }


    void Update()
    {
        
    }
}
