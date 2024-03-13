using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatInputControl : MonoBehaviour
{
    [SerializeField] private TMP_InputField textInput;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Send();
        }
    }

    public void Send()
    {
        if (textInput.text.Length <= 0)
        {
            textInput.text = "";
            return;
        }
        else if(textInput.text.StartsWith("/w ") || textInput.text.StartsWith("/W ") || textInput.text.StartsWith("/귓속말 ") || textInput.text.StartsWith("/ㅈ ") || textInput.text.StartsWith("/귓 "))
        {
            string valueStr = textInput.text;
            string[] splitStr = valueStr.Split(' ');
            string nickName = splitStr[1];
            var wMesssageStart = splitStr[0].Length + splitStr[1].Length + 2;
            if (wMesssageStart < valueStr.Length)
            {
                Debug.Log("귓속말 들어옴");
                string wMessage = valueStr.Substring(wMesssageStart);
                BackEndManager.Instance.GetChatManager().Whisper(nickName, wMessage);
                textInput.text = "";
            }
        }
        else if(textInput.text.StartsWith("/초대 ") || textInput.text.StartsWith("/invite ") || textInput.text.StartsWith("/Invite "))
        {
            string valueStr = textInput.text;
            string[] splitStr = valueStr.Split(' ');
            string nickName = splitStr[1];

            BackEndManager.Instance.GetMatchSystem().CreateMatchRoom(nickName);

            textInput.text = "";
        }
        else
        {
            BackEndManager.Instance.GetChatManager().ChatToChannel(textInput.text);
            textInput.text = "";
        }
    }


}
