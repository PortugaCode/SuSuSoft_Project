using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userName;

    [SerializeField] private bool isError = false;
    public bool IsError => isError;

    public void SetState(bool iserror)
    {
        isError = iserror;
    }

    public void SetJoinUserName(string username)
    {
        userName.text = $"[{username}]¥‘¿Ã ¡¢º”«œºÃΩ¿¥œ¥Ÿ.";
    }

    public void SetErrorMessage(string msg)
    {
        userName.text = msg;
    }
}
