using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;
using Facebook.Unity;
using System;
using System.Net;

public class BackEndFederation : MonoBehaviour
{
    private void Awake()
    {
        if(!FB.IsInitialized)
        {
            // 페이스북 SDK 초기화
            FB.Init(Initcallback, OnHideUnity);
        }
        else
        {
            // 이미 초기화 되어 있음
            FB.ActivateApp();
        }
    }

    private void Initcallback()
    {
        if(FB.IsInitialized)
        {
            // 앱을 활성화 시킨다.
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("FaceBook SDK 초기화 실패");
        }
    }

    private void OnHideUnity(bool isUnityShown)
    {
        if (!isUnityShown)
        {
            // 앱을 일시 정지 시킨다.
            Time.timeScale = 0;
        }
        else
        {
            // 앱을 다시 플레이 시킨다.
            Time.timeScale = 1;
        }
    }

    public void OnClickFBLogin()
    {
        // 읽어올 권한을 설정
        // 뒤끝 콘솔 > 유저 관리에서 회원의 이메일 정보를 알 수 있습니다.
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    private void AuthCallback(ILoginResult result)
    {
        if(FB.IsLoggedIn)
        {
            // 토큰 정보 참조
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;

            // 토큰을 string으로 변환
            string facebookToken = aToken.TokenString;

            // 뒤끝 서버에 획득한 페이스북 토큰으로 가입 요청
            // 동기 방법으로 가입 요청
            BackendReturnObject bro = Backend.BMember.AuthorizeFederation(facebookToken, FederationType.Facebook, "페이스북 로그인");

            // 페이스북에 로그인 성공
            if (bro.IsSuccess())
            {
                Debug.Log("페북 토큰으로 뒤끝 서버 로그인 성공 // 동기 방식");
                string name_ = string.Empty;

                // 회원가입인 경우
                if (bro.GetStatusCode() == "201")
                {
                    Debug.Log("페이스북 회원가입 성공");
                    //페이스북 이름을 닉네임으로 설정
                    var web = new WebClient();
                    name_ = web.DownloadString("https://graph.facebook.com/me?access_token=" + facebookToken);
                    name_ = name_.Split(",")[0].Split(":")[1].Replace("\"", "");
                    name_ = convert_from_unicode(name_).Replace(" ", "_");

                    //닉네임 설정


                }


                // 이미 회원가입한 유저 => 씬 이동
                DBManager.instance.user.isLogin = true;
                ChartManager.instance.GetChartData();
                DBManager.instance.DB_Load(name_, "1234");
                BackEndManager.Instance.GetMatchSystem().JoinMatchMaking();
                BackEndManager.Instance.GetChatManager().GetChatStatus();
            }

            else
            {
                switch (bro.GetStatusCode())
                {
                    case "403":
                        Debug.Log("차단된 사용자 입니다.");
                        break;
                    default:
                        Debug.Log("서버 공통 에러 발생");
                        break;
                }
            }
            /** [비동기 방식]
             * 비동기 방법으로 가입 요청
             * 비동기 방법으로 가입을 요청할 경우 Update 문에서 추가적인 처리 필요
             */
        }
        else
        {
            Debug.Log("유저 로그인 취소");
        }
    }

    private string convert_from_unicode(string str)
    {
        string rtstr = "";
        for (int i = 2; i < str.Length; i += 6)
        {
            string str1 = str.Substring(i, 4);
            rtstr += (char)Int16.Parse(str1, System.Globalization.NumberStyles.HexNumber);
        }
        return rtstr;
    }
}
