using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using BackEnd;
using System.Linq;

public class LoginManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject popUp_Menu;

    [Header("LogIn")]
    [SerializeField] private GameObject popUp_LogIn;
    [SerializeField] private TMP_InputField inputFieldID;
    [SerializeField] private TMP_InputField inputFieldPW;
    [SerializeField] private Button btnLogin;
    [SerializeField] private Button btnGoToSignUp;
    [SerializeField] private TMP_Text logInErrorText;

    [Header("SignUp")]
    [SerializeField] private GameObject popUp_SignUp;
    [SerializeField] private TMP_InputField inputFieldSignUpID;
    [SerializeField] private TMP_InputField inputFieldSignUpPW_1;
    [SerializeField] private TMP_InputField inputFieldSignUpPW_2;
    [SerializeField] private TMP_InputField inputFieldSignUpUserName;
    [SerializeField] private Button btnSignUp;
    [SerializeField] private Button btnGoToLogIn;
    [SerializeField] private TMP_Text signUpErrorText;

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

        if (idText.Trim().Equals("") || pwText.Trim().Equals(""))
        {
            // InputField가 비워져 있을 때
            inputFieldID.text = "";
            inputFieldPW.text = "";
            logInErrorText.text = $"정보를 입력해주세요.";
            logInErrorText.gameObject.SetActive(true);
            btnLogin.interactable = true;
            return;
        }

        btnLogin.interactable = false;

        Backend.BMember.CustomLogin(idText, pwText, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log($"로그인 성공");
                ChartManager.instance.GetChartData();
                DBManager.instance.DB_Init(idText, pwText);
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

                logInErrorText.text = $"{message}";
                logInErrorText.gameObject.SetActive(true);
            }
        });
    }

    public void SignUp()
    {
        // 캐싱
        string idText = inputFieldSignUpID.text;
        string pwText_1 = inputFieldSignUpPW_1.text;
        string pwText_2 = inputFieldSignUpPW_2.text;
        string userNameText = inputFieldSignUpUserName.text;

        if (idText.Trim().Equals("") || pwText_1.Trim().Equals("") || pwText_2.Trim().Equals("") || userNameText.Trim().Equals(""))
        {
            // InputField가 비워져있을 때
            inputFieldSignUpID.text = "";
            inputFieldSignUpPW_1.text = "";
            inputFieldSignUpPW_2.text = "";
            inputFieldSignUpUserName.text = "";
            signUpErrorText.text = "정보를 입력해주세요.";
            signUpErrorText.gameObject.SetActive(true);
            btnSignUp.interactable = true;
            return;
        }
        else if (pwText_1 != pwText_2)
        {
            // 패스워드가 일치하지 않을 때
            inputFieldSignUpID.text = "";
            inputFieldSignUpPW_1.text = "";
            inputFieldSignUpPW_2.text = "";
            inputFieldSignUpUserName.text = "";
            signUpErrorText.text = "비밀번호가 일치하지 않습니다.";
            signUpErrorText.gameObject.SetActive(true);
            btnSignUp.interactable = true;
            return;
        }
        else if (idText.Any(x => char.IsWhiteSpace(x) == true) || pwText_1.Any(x => char.IsWhiteSpace(x) == true) || pwText_2.Any(x => char.IsWhiteSpace(x) == true) || userNameText.Any(x => char.IsWhiteSpace(x) == true))
        {
            // 공백이 포함되어 있을 때
            inputFieldSignUpID.text = "";
            inputFieldSignUpPW_1.text = "";
            inputFieldSignUpPW_2.text = "";
            inputFieldSignUpUserName.text = "";
            signUpErrorText.text = "공백을 포함할 수 없습니다.";
            signUpErrorText.gameObject.SetActive(true);
            btnSignUp.interactable = true;
            return;
        }

        btnSignUp.interactable = false;

        Backend.BMember.CustomSignUp(idText, pwText_1, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log($"회원가입 성공");

                // 닉네임 항목에 입력한 유저 닉네임 할당
                Backend.BMember.CreateNickname(userNameText);

                inputFieldSignUpID.text = "";
                inputFieldSignUpPW_1.text = "";
                inputFieldSignUpPW_2.text = "";
                inputFieldSignUpUserName.text = "";
                btnSignUp.interactable = true;

                GoToCustomLogIn();
            }
            else
            {
                inputFieldSignUpID.text = "";
                inputFieldSignUpPW_1.text = "";
                inputFieldSignUpPW_2.text = "";
                inputFieldSignUpUserName.text = "";
                btnSignUp.interactable = true;

                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 401:
                        message = "프로젝트 상태가 '점검'입니다.";
                        break;
                    case 403:
                        message = callback.GetMessage().Contains("blocked") ? "차단당한 디바이스입니다." : "출시 설정이 테스트인데 AU가 10을 초과하였습니다.";
                        break;
                    case 409:
                        message = "중복된 아이디입니다.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }

                inputFieldSignUpID.text = "";
                inputFieldSignUpPW_1.text = "";
                inputFieldSignUpPW_2.text = "";
                inputFieldSignUpUserName.text = "";
                signUpErrorText.text = $"{message}";
                signUpErrorText.gameObject.SetActive(true);
                btnSignUp.interactable = true;
            }
        });
    }

    public void GoToCustomLogIn()
    {
        popUp_Menu.SetActive(false);
        popUp_LogIn.SetActive(true);
        popUp_SignUp.SetActive(false);
    }

    public void GoToSignUp()
    {
        popUp_LogIn.SetActive(false);
        popUp_SignUp.SetActive(true);
    }

    public void GoToMenu()
    {
        popUp_Menu.SetActive(true);
        popUp_LogIn.SetActive(false);
    }
}
