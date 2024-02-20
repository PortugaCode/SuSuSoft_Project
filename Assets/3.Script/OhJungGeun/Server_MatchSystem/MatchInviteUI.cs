using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchInviteUI : MonoBehaviour
{
    [SerializeField] private bool isCheckUI;
    [SerializeField] private bool isFailInRoom = false;

    [SerializeField] private TextMeshProUGUI invitedMessage;
    [SerializeField] private TextMeshProUGUI checkMessage;

    private void OnEnable()
    {
        if (isFailInRoom) return;

        if(isCheckUI)
        {
            checkMessage.text = $"[{BackEndManager.Instance.GetMatchSystem().inviteUserNickName}]님에게 하우징 초대를 성공하셨습니다. \n잠시만 기다려주세요.";
        }
        else
        {
            invitedMessage.text = $"[{BackEndManager.Instance.GetMatchSystem().InviteUserInfo.m_nickName}]님이 하우징 초대를 하셨습니다.\n수락하시겠습니까?";
        }
    }

    public void AcceptBtn(bool value)
    {
        BackEndManager.Instance.GetMatchSystem().AreYouAccept(value);
        gameObject.SetActive(false);
    }

    public void ClosePopUp()
    {
        gameObject.SetActive(false);
    }
}
