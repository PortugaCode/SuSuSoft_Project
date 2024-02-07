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
        if (textInput.text.Length <= 1)
        {
            textInput.text = "";
            textInput.ActivateInputField();
            return;
        }
        else if(textInput.text.StartsWith("/w ") || textInput.text.StartsWith("/W ") || textInput.text.StartsWith("/±Ó¼Ó¸» ") || textInput.text.StartsWith("/¤¸ "))
        {
            string valueStr = textInput.text;
            string[] splitStr = valueStr.Split(' ');
            string nickName = splitStr[1];
            var wMesssageStart = splitStr[0].Length + splitStr[1].Length + 2;
            if (wMesssageStart < valueStr.Length)
            {
                Debug.Log("±Ó¼Ó¸» µé¾î¿È");
                string wMessage = valueStr.Substring(wMesssageStart);
                BackEndManager.Instance.GetChatManager().Whisper(nickName, wMessage);
                textInput.text = "";
                textInput.ActivateInputField();
            }
        }
        else if(textInput.text.StartsWith("/ÃÊ´ë "))
        {
            string valueStr = textInput.text;
            string[] splitStr = valueStr.Split(' ');
            string nickName = splitStr[1];

            BackEndManager.Instance.GetMatchSystem().CreateMatchRoom(nickName);

            textInput.text = "";
            textInput.ActivateInputField();
        }
        else
        {
            BackEndManager.Instance.GetChatManager().ChatToChannel(textInput.text);
            textInput.text = "";
            textInput.ActivateInputField();
        }
    }


}
