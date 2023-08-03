using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator �̂��߂̎Q��
using UnityEngine.Networking;   // UnityWebRequest �̂��߂̎Q��

public class Term1_3_Chapter02_GetImage : MonoBehaviour, IPointerClickHandler
{
    void Start()
    {
        
    }

    // �A�N�Z�X���� URL
    // �T�[�o�[URL + /sample01.png �ŃA�N�Z�X
    string urlGitHub = "�����ɃT�[�o�[URL������";

    public void OnPointerClick(PointerEventData eventData)
    {
        // �}�E�X�N���b�N�C�x���g

        // HTTP ���N�G�X�g��񓯊�������҂��߃R���[�`���Ƃ��ČĂяo��
        StartCoroutine("GetTexture");
    }

    IEnumerator GetTexture()
    {
        urlGitHub = "https://tseigo-stunning-funicular-q6vj5g6rx6p347j4-8080.app.github.dev/sample01.png";

        // �e�N�X�`���� GET ���N�G�X�g�œǂݍ��ށB�u���E�U�ł������B
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(urlGitHub);


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

                // �e�N�X�`���Ɋ��蓖��
                Texture loadedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

                GameObject.Find("Tile01").GetComponent<MeshRenderer>().material.SetTexture("_MainTex", loadedTexture);

                break;
        }
    }
}