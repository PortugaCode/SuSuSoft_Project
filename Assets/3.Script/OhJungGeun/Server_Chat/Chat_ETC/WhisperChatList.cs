using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;
using BackEnd.Tcp;

public class WhisperChatList : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userName;
    [SerializeField] private TextMeshProUGUI userSpeakText;


    public void SetWhisperChatList(string userName, string userSpeakText)
    {
        this.userName.text = userName;
        this.userSpeakText.text = userSpeakText;
    }
}
