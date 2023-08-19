using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator �̂��߂̎Q��
using UnityEngine.Networking;   // UnityWebRequest �̂��߂̎Q��
using System;                   // Serializable �̂��߂̎Q��
using System.Text;              // Encoding �̂��߂̎Q��

using System.Collections.Generic;

public class Term2_3_Chapter01_ClickPart : MonoBehaviour, IPointerClickHandler
{
    // ��M���� JSON �f�[�^�� Unity �ň����f�[�^�ɂ��� ResponseData �x�[�X�N���X
    // API�d�l : https://platform.openai.com/docs/api-reference/completions/object
    [Serializable]
    public class ResponseData
    {
        public string id;
        public string @object; // object �͗\���Ȃ̂� @ ���g���ăG�X�P�[�v���Ă��܂�
        public int created;
        public List<ResponseDataChoice> choices;
        public ResponseDataUsage usage;
    }

    [Serializable]
    public class ResponseDataUsage
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }
    [Serializable]
    public class ResponseDataChoice
    {
        public int index;
        public RequestDataMessages message;
        public string finish_reason;
    }

    // ���M���� Unity �f�[�^�� JSON �f�[�^������ PointRequestData �x�[�X�N���X
    [Serializable]
    public class RequestData
    {
        public string model;
        public List<RequestDataMessages> messages;
    }

    [Serializable]
    public class RequestDataMessages
    {
        public string role;
        public string content;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // HTTP ���N�G�X�g��񓯊�������҂��߃R���[�`���Ƃ��ČĂяo��
        StartCoroutine("PostChatGPT");
    }

    // ChatGPT �� API �L�[�����
    string tokenCHatGPT = "tokenCHatGPT";

    // ���N�G�X�g����{��
    IEnumerator PostChatGPT()
    {
        // HTTP ���N�G�X�g����(POST ���\�b�h) UnityWebRequest ���Ăяo��
        // ���N�G�X�g�d�l : https://platform.openai.com/docs/guides/gpt/chat-completions-api
        // API�d�l : https://platform.openai.com/docs/api-reference/completions/object
        UnityWebRequest request = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST");

        RequestData requestData = new RequestData();
        // �f�[�^��ݒ�
        requestData.model = "gpt-3.5-turbo-0613";
        RequestDataMessages currentMessage = new RequestDataMessages();
        // ���[���� user
        currentMessage.role = "user";
        // ���ۂ̎���
        currentMessage.content = "����ɂ��́I";
        List<RequestDataMessages> currentMessages = new List<RequestDataMessages>();
        currentMessages.Add(currentMessage);
        requestData.messages = currentMessages;
        Debug.Log($"currentMessages[0].content : {currentMessages[0].content}");

        // ���M�f�[�^�� JsonUtility.ToJson �� JSON ��������쐬
        // RequestData, RequestDataMessages �̍\���Ɋ�Â��ĕϊ����Ă����
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
        // ChatGPT �p�̔F�؂�`����ݒ�
        request.SetRequestHeader("Authorization", $"Bearer {tokenCHatGPT}");

        // ���N�G�X�g�J�n
        yield return request.SendWebRequest();

        Debug.Log("���N�G�X�g...");

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

                ResponseData resultResponse = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);

                // �ԓ�
                Debug.Log($"resultResponse.choices[0].message : {resultResponse.choices[0].message.content}");

                // �e�L�X�g�𔽉f
                GameObject.Find("MessageText").GetComponent<TextMesh>().text = resultResponse.choices[0].message.content;

                break;
        }


    }
}