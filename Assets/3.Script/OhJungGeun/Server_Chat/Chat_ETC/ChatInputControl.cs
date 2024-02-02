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
        if (textInput.text == string.Empty)
        {
            return;
        }
        else
        {
            BackEndManager.Instance.GetChatManager().ChatToChannel(textInput.text);
            textInput.text = "";
            textInput.ActivateInputField();
        }
    }
}
