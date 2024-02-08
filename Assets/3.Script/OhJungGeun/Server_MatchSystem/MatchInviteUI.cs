using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchInviteUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI invitedMessage;

    private void OnEnable()
    {
        invitedMessage.text = $"[{BackEndManager.Instance.GetMatchSystem().InviteUserInfo.m_nickName}]���� �Ͽ�¡ �ʴ븦 �ϼ̽��ϴ�.\n �����Ͻðڽ��ϱ�?";
    }

    public void AcceptBtn(bool value)
    {
        BackEndManager.Instance.GetMatchSystem().AreYouAccept(value);
        gameObject.SetActive(false);
    }
}
