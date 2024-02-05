using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using BackEnd;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputFieldID;
    [SerializeField] private TMP_InputField inputFieldPW;
    [SerializeField] private Button btnLogin;

    private void Update()
    {
        if (Backend.IsInitialized)
        {
            Backend.AsyncPoll();
        }
    }

    public void Login()
    {
        // 캐싱
        string idText = inputFieldID.text;
        string pwText = inputFieldPW.text;

        if (idText.Trim().Equals("") || pwText.Trim().Equals("")) // ID 또는 PW 비워진 상태
        {
            return;
        }

        btnLogin.interactable = false;

        Backend.BMember.CustomLogin(idText, pwText, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log($"로그인 성공");
            }
            else
            {
                inputFieldID.text = "";
                inputFieldPW.text = "";
                btnLogin.interactable = true;

                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 401:
                        message = callback.GetMessage().Contains("customID") ? "존재하지 않는 아이디입니다." : "잘못된 비밀번호입니다.";
                        break;
                    case 403:
                        message = callback.GetMessage().Contains("user") ? "차단당한 유저입니다." : "차단당한 디바이스입니다.";
                        break;
                    case 410:
                        message = "탈퇴가 진행중인 유저입니다.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }
                Debug.Log($"{message}");
            }
        });

    }
}
