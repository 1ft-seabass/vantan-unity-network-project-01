using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;           // IEnumerator �̂��߂̎Q��
using UnityEngine.Networking;       // UnityWebRequest �̂��߂̎Q��
using System;                       // Serializable �̂��߂̎Q��
using System.Collections.Generic;   // List �̂��߂̎Q��
using UnityEngine.UI;

public class Sample05_NextButton_Direct_Airtable : MonoBehaviour, IPointerClickHandler
{

    // Airtable �� Base ID
    string settingAirtableBaseID = "settingAirtableBaseID";

    // Airtable �� API �L�[
    string settingAirtableAPIKey = "settingAirtableAPIKey";

    // Airtable �� Table ��
    string settingAirtableTable = "QuizList";

    // Airtable �� view ��
    string settingAirtableView = "Grid view";

    // ��M���� JSON �f�[�^�� Unity �ň����f�[�^�ɂ��� ResponseData �x�[�X�N���X
    [Serializable]
    public class ResponseData
    {
        public List<ResponseDataRecord> records;
    }

    [Serializable]
    public class ResponseDataRecord
    {
        public string id;
        public string createdTime;
        public ResponseDataRecordField fields;
    }

    [Serializable]
    public class ResponseDataRecordField
    {
        public string ID;
        public string QuizText;
        public string Select1;
        public string Select2;
        public string Select3;
        public int Answer;
    }

    // ���݂̃N�C�Y�ԍ�
    int currentQuizID = 1;

    // �N�C�Y���X�g
    ResponseData responseQuizList;

    // ���݂̃N�C�Y
    ResponseDataRecordField currentQuiz;

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
        currentQuiz = responseQuizList.records[currentQuizID - 1].fields;

        // �e�L�X�g�ݒ�
        GameObject.Find("QuizText").GetComponent<TextMesh>().text = currentQuiz.QuizText;
        GameObject.Find("Button1").GetComponentInChildren<Text>().text = currentQuiz.Select1;
        GameObject.Find("Button2").GetComponentInChildren<Text>().text = currentQuiz.Select2;
        GameObject.Find("Button3").GetComponentInChildren<Text>().text = currentQuiz.Select3;

    }
    public void OnClickSelectButon(int selectId)
    {
        

        // �������킹
        Debug.Log($"OnClickSelectButon selectId:{selectId}");

        if (selectId == currentQuiz.Answer)
        {
            // ������
            Debug.Log($"������I");

        } else
        {
            // �͂���
            Debug.Log($"�͂���");

        }

        // ����
        currentQuizID += 1;
        if (currentQuizID > 3)
        {
            currentQuizID = 1;  // �J��Ԃ�
        }

        // �N�C�Y�ݒ�
        SetCurrentQuiz();
    }

    IEnumerator GetAPI()
    {
        // API URL �쐬
        string urlAirtableAPI = "https://api.airtable.com/v0/" + settingAirtableBaseID + "/" + settingAirtableTable + "?view=" + settingAirtableView;
        // API URL �� Uri.AbsoluteUri �ƒʂ��� URL �p�����[�^����
        // �� ?view=Grid view �� ?view=Grid%20view �ɂȂ�
        urlAirtableAPI = new Uri(urlAirtableAPI).AbsoluteUri;

        // HTTP ���N�G�X�g����(GET ���\�b�h) UnityWebRequest ���Ăяo��
        UnityWebRequest request = UnityWebRequest.Get(urlAirtableAPI);

        // API �L�[�� HTTP �w�b�_�[�ɐݒ�
        request.SetRequestHeader("Authorization", "Bearer " + settingAirtableAPIKey);

        // ���N�G�X�g�J�n
        yield return request.SendWebRequest();

        Debug.Log("���N�G�X�g�J�n");

        // ���ʂɂ���ĕ���
        switch (request.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("���N�G�X�g��");
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("ProtocolError");
                Debug.Log($"responseCode : {request.responseCode}");
                Debug.Log($"error : {request.error}");
                break;

            case UnityWebRequest.Result.ConnectionError:
                Debug.Log("ConnectionError");
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
