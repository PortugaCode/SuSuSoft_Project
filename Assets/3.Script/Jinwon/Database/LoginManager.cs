using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using BackEnd;
using System.Linq;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Text.RegularExpressions;

public class LoginManager : MonoBehaviour
{
    [Header("Main Popup")]
    [SerializeField] private GameObject popUp_Main;
    [SerializeField] private TMP_InputField inputField_ID;
    [SerializeField] private TMP_InputField inputField_PW;
    [SerializeField] private TMP_Text logInErrorText;

    [Header("SignUp Popup")]
    [SerializeField] private GameObject popUp_SignUp;
    [SerializeField] private TMP_InputField inputField_SignUpID;
    [SerializeField] private TMP_InputField inputField_SignUpPW_1;
    [SerializeField] private TMP_InputField inputField_SignUpPW_2;
    [SerializeField] private TMP_Text idErrorText;
    [SerializeField] private TMP_Text pw1ErrorText;
    [SerializeField] private TMP_Text pw2ErrorText;
    private string id;
    private string pw;

    [Header("UserName Popup")]
    [SerializeField] private GameObject popUp_Nickname;
    [SerializeField] private TMP_InputField inputField_Nickname;
    [SerializeField] private TMP_Text nicknameErrorText;

    private void Update()
    {
        if (Backend.IsInitialized)
        {
            Backend.AsyncPoll();
        }
    }

    public void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            GetAccessCode();
            // Continue with Play Games Services
        }
        else
        {
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }

    public void GetAccessCode()
    {
        PlayGamesPlatform.Instance.RequestServerSideAccess(
          /* forceRefreshToken= */ false,
          code => {
              Debug.Log("구글 인증 코드 : " + code);

              Backend.BMember.GetGPGS2AccessToken(code, googleCallback =>
              {
                  Debug.Log("GetGPGS2AccessToken 함수 호출 결과 " + googleCallback);

                  string accessToken = "";

                  if (googleCallback.IsSuccess())
                  {
                      Debug.Log($"googleCallback 성공");

                      accessToken = googleCallback.GetReturnValuetoJSON()["access_token"].ToString();

                      Backend.BMember.AuthorizeFederation(accessToken, FederationType.GPGS2, "Google로 가입함", (callback) =>
                      {
                          Debug.Log($"{callback.GetMessage()}");
                          // 페더레이션 인증 이후 처리
                          if (int.Parse(callback.GetStatusCode()) == 201) // 신규 회원 가입의 경우
                          {
                              Debug.Log("회원가입 시작");
                              Backend.BMember.CustomSignUp(Social.localUser.id, "");
                              Backend.BMember.CustomLogin(Social.localUser.id, "");
                              DBManager.instance.user.isLogin = true;

                              ChartManager.instance.GetChartData();
                              Backend.BMember.CreateNickname(Social.localUser.userName);

                              var bro = Backend.GameData.Get("User", new Where());
                              Param param = new Param();
                              param.Add("UserName", Social.localUser.userName);
                              Backend.PlayerData.UpdateMyLatestData("User", param);

                              DBManager.instance.DB_Add(Social.localUser.id, "", Social.localUser.userName);

                              BackEndManager.Instance.GetMatchSystem().JoinMatchMaking();
                              BackEndManager.Instance.GetChatManager().GetChatStatus();
                              Debug.Log("회원가입 완료");
                          }
                          else if (int.Parse(callback.GetStatusCode()) == 200) // 기존 회원 로그인 성공의 경우
                          {
                              Debug.Log("로그인 시작");
                              Backend.BMember.CustomLogin(Social.localUser.id, "");
                              DBManager.instance.user.isLogin = true;

                              ChartManager.instance.GetChartData();
                              DBManager.instance.DB_Load(Social.localUser.id, "");

                              BackEndManager.Instance.GetMatchSystem().JoinMatchMaking();
                              BackEndManager.Instance.GetChatManager().GetChatStatus();
                              Debug.Log("로그인 완료");
                          }
                      });
                  }
              });
          });
    }

    public void GPGS_GoogleLogin()
    {
        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate((bool success) =>
        {
            if (success) //로그인에 성공하면
            {
                Debug.Log(Social.localUser.userName + "님 환영합니다");
                Debug.Log("당신의 ID는 " + Social.localUser.id + "입니다");

                PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
            }
            else //로그인에 실패하면
            {
                Debug.Log("로그인에 실패했습니다");
            }
        });
    }

    public void Login()
    {
        // 캐싱
        string idText = inputField_ID.text;
        string pwText = inputField_PW.text;

        id = idText;
        pw = pwText;

        if (idText.Trim().Equals("") || pwText.Trim().Equals(""))
        {
            // InputField가 비워져 있을 때
            inputField_ID.text = "";
            inputField_PW.text = "";
            logInErrorText.text = $"정보를 입력해주세요.";
            logInErrorText.gameObject.SetActive(true);
            return;
        }

        Backend.BMember.CustomLogin(idText, pwText, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log($"로그인 성공");

                Where where = new Where();
                where.Equal("owner_inDate", Backend.UserInDate); // 로그인 한 유저의 owner_inDate로 User DB 조회

                var bro = Backend.GameData.GetMyData("User", where);

                if (bro.IsSuccess() && (bro.GetReturnValuetoJSON()["rows"].Count == 0 || (bro.GetReturnValuetoJSON()["rows"].Count > 0) && bro.GetReturnValuetoJSON()["rows"][0]["UserName"][0].ToString().Equals(string.Empty)))
                {
                    Debug.Log("닉네임 재설정 필요");
                    GoToSetNickname();
                }
                else
                {
                    Debug.Log("로그인 완료");
                    DBManager.instance.user.isLogin = true;
                    ChartManager.instance.GetChartData();
                    DBManager.instance.DB_Load(idText, pwText);
                    BackEndManager.Instance.GetMatchSystem().JoinMatchMaking();
                    BackEndManager.Instance.GetChatManager().GetChatStatus();
                }
            }
            else
            {
                inputField_ID.text = "";
                inputField_PW.text = "";

                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 401:
                        message = callback.GetMessage().Contains("Id") ? "존재하지 않는 아이디입니다." : "잘못된 비밀번호입니다.";
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
        string idText = inputField_SignUpID.text;
        string pwText_1 = inputField_SignUpPW_1.text;
        string pwText_2 = inputField_SignUpPW_2.text;

        if (idText.Trim().Equals("") || pwText_1.Trim().Equals("") || pwText_2.Trim().Equals(""))
        {
            // InputField가 비워져있을 때
            inputField_SignUpID.text = "";
            inputField_SignUpPW_1.text = "";
            inputField_SignUpPW_2.text = "";
            if (idText.Trim().Equals(""))
            {
                idErrorText.text = "정보를 입력해주세요.";
                idErrorText.gameObject.SetActive(true);
                pw1ErrorText.gameObject.SetActive(false);
                pw2ErrorText.gameObject.SetActive(false);
            }
            else if (pwText_1.Trim().Equals(""))
            {
                pw1ErrorText.text = "정보를 입력해주세요.";
                idErrorText.gameObject.SetActive(false);
                pw1ErrorText.gameObject.SetActive(true);
                pw2ErrorText.gameObject.SetActive(false);
            }
            else if (pwText_2.Trim().Equals(""))
            {
                pw2ErrorText.text = "정보를 입력해주세요.";
                idErrorText.gameObject.SetActive(false);
                pw1ErrorText.gameObject.SetActive(false);
                pw2ErrorText.gameObject.SetActive(true);
            }
            return;
        }
        else if (pwText_1 != pwText_2)
        {
            // 패스워드가 일치하지 않을 때
            inputField_SignUpPW_2.text = "";
            pw1ErrorText.text = "";
            idErrorText.text = "";
            pw2ErrorText.text = "비밀번호가 일치하지 않습니다.";
            idErrorText.gameObject.SetActive(false);
            pw1ErrorText.gameObject.SetActive(false);
            pw2ErrorText.gameObject.SetActive(true);
            return;
        }
        else if (idText.Any(x => char.IsWhiteSpace(x) == true))
        {
            // 공백이 포함되어 있을 때
            inputField_SignUpID.text = "";
            idErrorText.text = "아이디는 공백을 포함할 수 없습니다.";
            idErrorText.gameObject.SetActive(true);
            pw1ErrorText.gameObject.SetActive(false);
            pw2ErrorText.gameObject.SetActive(false);
            return;
        }
        else if (pwText_1.Any(x => char.IsWhiteSpace(x) == true))
        {
            // 공백이 포함되어 있을 때
            inputField_SignUpPW_1.text = "";
            pw1ErrorText.text = "비밀번호는 공백을 포함할 수 없습니다.";
            idErrorText.gameObject.SetActive(false);
            pw1ErrorText.gameObject.SetActive(true);
            pw2ErrorText.gameObject.SetActive(false);
            return;
        }
        else if (pwText_2.Any(x => char.IsWhiteSpace(x) == true))
        {
            // 공백이 포함되어 있을 때
            inputField_SignUpPW_2.text = "";
            pw2ErrorText.text = "비밀번호는 공백을 포함할 수 없습니다.";
            idErrorText.gameObject.SetActive(false);
            pw1ErrorText.gameObject.SetActive(false);
            pw2ErrorText.gameObject.SetActive(true);
            return;
        }

        Backend.BMember.CustomSignUp(idText, pwText_1, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log($"회원가입 성공");

                inputField_SignUpID.text = "";
                inputField_SignUpPW_1.text = "";
                inputField_SignUpPW_2.text = "";

                id = idText;
                pw = pwText_1;

                Backend.BMember.CustomLogin(idText, pwText_1);
                ChartManager.instance.GetChartData();
                DBManager.instance.DB_Add(id, pw, "");

                GoToSetNickname();
            }
            else
            {
                inputField_SignUpID.text = "";
                inputField_SignUpPW_1.text = "";
                inputField_SignUpPW_2.text = "";

                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 401:
                        message = "프로젝트 상태가 '점검'입니다.";
                        break;
                    case 403:
                        message = callback.GetMessage().Contains("blocked") ? "차단당한 디바이스입니다." : "출시 설정이 '테스트'인데 AU가 10을 초과하였습니다.";
                        break;
                    case 409:
                        message = "중복된 아이디입니다.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }

                inputField_SignUpID.text = "";
                inputField_SignUpPW_1.text = "";
                inputField_SignUpPW_2.text = "";
                pw2ErrorText.text = $"{message}";
                pw2ErrorText.gameObject.SetActive(true);
            }
        });
    }

    public void SetUserName() // 유저 닉네임 설정
    {
        var bro = Backend.GameData.Get("User", new Where());

        string userNameText = inputField_Nickname.text;

        if (userNameText.Trim().Equals(""))
        {
            // InputField가 비워져있을 때
            inputField_Nickname.text = "";
            nicknameErrorText.text = "정보를 입력해주세요.";
            nicknameErrorText.gameObject.SetActive(true);
            return;
        }
        else if (userNameText.Any(x => char.IsWhiteSpace(x) == true))
        {
            // 공백이 포함되어 있을 때
            inputField_Nickname.text = "";
            nicknameErrorText.text = "닉네임은 공백을 포함할 수 없습니다.";
            nicknameErrorText.gameObject.SetActive(true);
            return;
        }

        if (bro.IsSuccess())
        {
            // 닉네임이 다른 유저의 닉네임과 중복될 때

            for (int i = 0; i < bro.GetReturnValuetoJSON()["rows"].Count; i++)
            {
                if (bro.GetReturnValuetoJSON()["rows"][i]["UserName"][0].ToString().Equals(userNameText))
                {
                    inputField_Nickname.text = "";
                    nicknameErrorText.text = "사용중인 닉네임입니다.";
                    nicknameErrorText.gameObject.SetActive(true);
                    return;
                }
            }
        }

        // 닉네임 항목에 입력한 유저 닉네임 할당
        Backend.BMember.CreateNickname(userNameText);

        Param param = new Param();
        param.Add("UserName", userNameText);

        // 해당 테이블 데이터 수정
        Backend.PlayerData.UpdateMyLatestData("User", param);

        GoToMain();
    }

    public void GoToSignUp() // 회원가입 팝업으로 이동
    {
        popUp_Main.gameObject.SetActive(false);
        popUp_SignUp.gameObject.SetActive(true);
        idErrorText.gameObject.SetActive(false);
        pw1ErrorText.gameObject.SetActive(false);
        pw2ErrorText.gameObject.SetActive(false);
        inputField_SignUpID.text = "";
        inputField_SignUpPW_1.text = "";
        inputField_SignUpPW_2.text = "";
        idErrorText.text = "";
        pw1ErrorText.text = "";
        pw2ErrorText.text = "";
    }

    public void GoToMain() // 메인 팝업으로 이동
    {
        popUp_Main.gameObject.SetActive(true);
        popUp_SignUp.gameObject.SetActive(false);
        popUp_Nickname.gameObject.SetActive(false);
        logInErrorText.gameObject.SetActive(false);
        inputField_ID.text = "";
        inputField_PW.text = "";
        logInErrorText.text = "";
    }

    public void GoToSetNickname() // 닉네임 설정 팝업으로 이동
    {
        popUp_SignUp.gameObject.SetActive(false);
        popUp_Nickname.gameObject.SetActive(true);
        nicknameErrorText.gameObject.SetActive(false);
        inputField_Nickname.text = "";
        nicknameErrorText.text = "";
    }
}
