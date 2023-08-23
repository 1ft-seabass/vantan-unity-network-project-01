using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;           // IEnumerator �̂��߂̎Q��
using UnityEngine.Networking;       // UnityWebRequest �̂��߂̎Q��
using System;                       // Serializable �̂��߂̎Q��
using System.Collections.Generic;   // List �̂��߂̎Q��

public class Sample02_NextButton : MonoBehaviour, IPointerClickHandler
{

    // �A�N�Z�X���� URL
    // �T�[�o�[URL + /api/get �ŃA�N�Z�X
    string urlAPI = "";

    // ���݂̃N�C�Y�ԍ�
    int currentQuizID = 1;

    // �N�C�Y���X�g
    ResponseData responseQuizList;

    // ���݂̃N�C�Y
    ResponseDataList currentQuiz;

    // ��M���� JSON �f�[�^�� Unity �ň����f�[�^�ɂ��� ResponseData �x�[�X�N���X
    [Serializable]
    public class ResponseData
    {
        public List<ResponseDataList> data;
    }

    [Serializable]
    public class ResponseDataList
    {
        public string ID;
        public string QuizText;
        public string Select1;
        public string Select2;
        public string Select3;
        public int Answer;
    }

    void Start()
    {
        StartCoroutine("GetAPI");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // ����
        currentQuizID += 1;
        if (currentQuizID > 3)
        {
            currentQuizID = 1;  // �J��Ԃ�
        }
        // �N�C�Y�ݒ�
        SetCurrentQuiz();
    }

    void SetCurrentQuiz()
    {
        // ���݂̃N�C�Y
        currentQuiz = responseQuizList.data[currentQuizID - 1];

        // �e�L�X�g�ݒ�
        GameObject.Find("QuizText").GetComponent<TextMesh>().text = currentQuiz.QuizText;
        GameObject.Find("SelectText1").GetComponent<TextMesh>().text = currentQuiz.Select1;
        GameObject.Find("SelectText2").GetComponent<TextMesh>().text = currentQuiz.Select2;
        GameObject.Find("SelectText3").GetComponent<TextMesh>().text = currentQuiz.Select3;

    }

    IEnumerator GetAPI()
    {
        // HTTP ���N�G�X�g����(GET ���\�b�h) UnityWebRequest ���Ăяo��
        // �A�N�Z�X�����͕ϐ� urlGitHub �Őݒ�
        UnityWebRequest request = UnityWebRequest.Get(urlAPI);

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

                // �R���\�[���ɕ\��
                Debug.Log($"responseData: {request.downloadHandler.text}");

                // ResponseData �N���X�� Unity �ň�����f�[�^��
                responseQuizList = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);

                // ����̃N�C�Y
                SetCurrentQuiz();

                break;
        }

    }

    void Update()
    {
        
    }
}
