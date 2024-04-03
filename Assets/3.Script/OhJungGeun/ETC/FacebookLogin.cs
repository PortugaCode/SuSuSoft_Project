using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using BackEnd;
using BackEnd.Tcp;
//using Facebook.Unity;

public class FacebookLogin : MonoBehaviour
{
/*
    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("Facebook SDK 초기화 실패");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void OnCFBLogin()
    {
        var perms = new List<string>() { "public_profile", "email" }; // 이메일 정보를 요구하기 위한 값입니다.
        FB.LogInWithReadPermissions(perms, AuthCallback);

    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            string facebookToken = aToken.TokenString;

            BackendReturnObject bro = Backend.BMember.AuthorizeFederation(facebookToken, FederationType.Facebook, "페이스북 로그인");

            if (bro.IsSuccess())
            {
                string name_ = string.Empty;
                // 회원가입인 경우
                if (bro.GetStatusCode() == "201")
                {
                    Debug.Log("페이스북 회원가입 성공");
                    var web = new WebClient();
                    name_ = web.DownloadString("https://graph.facebook.com/me?access_token=" + facebookToken);
                    name_ = name_.Split(",")[0].Split(":")[1].Replace("\"", "");
                    name_ = convert_from_unicode(name_).Replace(" ", "_");

                    DBManager.instance.user.isLogin = true;
                    ChartManager.instance.GetChartData();
                    Backend.BMember.CreateNickname(name_);

                    Param param = new Param();
                    param.Add("UserName", name_);
                    Backend.PlayerData.UpdateMyLatestData("User", param);

                    DBManager.instance.DB_Add(facebookToken, "", name_);

                    BackEndManager.Instance.GetMatchSystem().JoinMatchMaking();
                    BackEndManager.Instance.GetChatManager().GetChatStatus();

                }
                else if (bro.GetStatusCode() == "200")
                {
                    Debug.Log("페이스북 로그인 성공");
                    DBManager.instance.user.isLogin = true;

                    ChartManager.instance.GetChartData();
                    DBManager.instance.DB_Load(facebookToken, "");

                    BackEndManager.Instance.GetMatchSystem().JoinMatchMaking();
                    BackEndManager.Instance.GetChatManager().GetChatStatus();
                }
            }
            else
            {
                Debug.LogError("페이스북 로그인 실패");
            }
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
*/
}
