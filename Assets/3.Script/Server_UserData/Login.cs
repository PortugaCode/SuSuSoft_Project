using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;
using BackEnd.Tcp;

public class Login : LoginBase
{
    [SerializeField] private Image imageID;                  //ID 색상 변경
    [SerializeField] private TMP_InputField inputFieldID;    //ID Input

    [SerializeField] private Image imagePW;                  //PW 색상 변경
    [SerializeField] private TMP_InputField inputFieldPW;    //PW Input

    [SerializeField] private Button btnLogin;


    public void OnClickLogin()
    {
        ResetUI(imageID, imagePW);

        if (IsFieldDataEmpty(imageID, inputFieldID.text, "ID")) return;
        if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "PassWord")) return;

        btnLogin.interactable = false;


        StartCoroutine(nameof(LoginProcess));

        ResponseToLogin(inputFieldID.text, inputFieldPW.text);
    }



    private void ResponseToLogin(string Id, string Pw)
    {
        Backend.BMember.CustomLogin(Id, Pw, callback =>
        {
            StopCoroutine(nameof(LoginProcess));


            if (callback.IsSuccess())
            {

                SetMessage($"{inputFieldID.text}님 환영합니다.");
                //BackEndManager.Instance.GetMatchSystem().JoinMatchMaking();
                //BackEndManager.Instance.GetChatManager().GetChatStatus();
            }

            else
            {
                btnLogin.interactable = true;

                string message = string.Empty;

                switch(int.Parse(callback.GetStatusCode()))
                { 
                    case 401: 
                        message = callback.GetMessage().Contains("customId") ? "존재하지 않는 아이디입니다." : "잘못된 비밀번호입니다.";
                        break;

                    case 403: 
                        message = callback.GetMessage().Contains("user") ? "차단한 유저입니다." : "차단당한 디바이스입니다.";
                        break;
                    case 410:
                        message = "탈퇴가 진행중인 계정입니다.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }

                if(message.Contains("비밀번호"))
                {
                    GuideForIncorrectlyEnteredData(imagePW, message);
                }
                else
                {
                    GuideForIncorrectlyEnteredData(imageID, message);
                }
            }
        });
    }

    private IEnumerator LoginProcess()
    {
        float timer = 0f;

        while(true)
        {
            timer += Time.deltaTime;

            SetMessage($"로그인 중입니다... {timer:F1}");
            yield return null;
        }
    }
}
