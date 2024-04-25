using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEvent2 : MonoBehaviour
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
        UnityEngine.Object.Destroy(textureScreenCapture);
    }

}
