using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchInviteUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI invitedMessage;

    private void OnEnable()
    {
        invitedMessage.text = $"[{BackEndManager.Instance.GetMatchSystem().InviteUserInfo.m_nickName}]님이 하우징 초대를 하셨습니다.\n 수락하시겠습니까?";
    }

    public void AcceptBtn(bool value)
    {
        BackEndManager.Instance.GetMatchSystem().AreYouAccept(value);
        gameObject.SetActive(false);
    }
}
