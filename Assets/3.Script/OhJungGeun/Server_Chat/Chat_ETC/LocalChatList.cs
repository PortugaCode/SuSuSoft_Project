using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;
using BackEnd.Tcp;



public class LocalChatList : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userName;
    [SerializeField] private TextMeshProUGUI userSpeakText;

    [SerializeField] private bool isWhisper = false;
    public bool IsWhisper => isWhisper;

    public void SetIsWhisper(bool isWhisper)
    {
        this.isWhisper = isWhisper;
    }

    public string GetUserName()
    {
        return userName.text;
    }

    public string GetUserSpeakText()
    {
        return userSpeakText.text;
    }

    public void SetLocalChatList(string userName, string userSpeakText)
    {
        this.userName.text = userName;
        this.userSpeakText.text = userSpeakText;
    }
}
