using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userName;

    public void SetJoinUserName(string username)
    {
        userName.text = $"{username}���� �����ϼ̽��ϴ�.";
    }
}
