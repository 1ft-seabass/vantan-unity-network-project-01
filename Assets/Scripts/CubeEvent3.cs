using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CubeEvent3 : MonoBehaviour
{
    // �L���v�`�����ꂽ�摜�f�[�^
    byte[] dataImage;

    void Start()
    {
        StartCoroutine(DoScreenShot());
    }

    IEnumerator DoScreenShot()
    {
        Debug.Log($"DoScreenShot");

        // �����_�����O��ɏ������J�n
        yield return new WaitForEndOfFrame();

        // Texture2D �ŃX�N���[���V���b�g���擾
        Texture2D textureScreenCapture = ScreenCapture.CaptureScreenshotAsTexture();

        // EncodeToJPG �� JPEG �ɕϊ�����BEncodeToPNG �Ȃ� PNG �ɕϊ��ł���
        dataImage = textureScreenCapture.EncodeToJPG();

        Debug.Log($"�B�e����");
        Debug.Log($"{dataImage}");

        // textureScreenCapture ���폜
        Destroy(textureScreenCapture);

        // �摜���M
        StartCoroutine(PostData());
    }

    IEnumerator PostData()
    {
        // IMultipartFormSection �� multipart/form-data �̃f�[�^�Ƃ��đ���܂�
        // https://docs.unity3d.com/ja/2018.4/Manual/UnityWebRequest-SendingForm.html
        // https://docs.unity3d.com/ja/2019.4/ScriptReference/Networking.IMultipartFormSection.html
        // https://docs.unity3d.com/ja/2020.3/ScriptReference/Networking.MultipartFormDataSection.html
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        // �摜�t�H�[���ŗp��
        formData.Add(new MultipartFormFileSection("imageFile", dataImage, "01.jpg", "multipart/form-data"));

        // HTTP ���N�G�X�g����(POST ���\�b�h) UnityWebRequest ���Ăяo��
        // �� 2 �����ŏ�L�̃t�H�[���f�[�^�����蓖�Ă� multipart/form-data �̃f�[�^�Ƃ��đ���܂�
        UnityWebRequest request = UnityWebRequest.Post("http://localhost:1880/api/post/image", formData);

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

            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("ProtocolError");
                Debug.Log(request.responseCode);
                Debug.Log(request.error);
                break;

            case UnityWebRequest.Result.ConnectionError:
                Debug.Log("ConnectionError");
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log("���N�G�X�g����");

                // �R���\�[���ɕ\��
                Debug.Log($"responseData: {request.downloadHandler.text}");

                break;
        }


    }
}
