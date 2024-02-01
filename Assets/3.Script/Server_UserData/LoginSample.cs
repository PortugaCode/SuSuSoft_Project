using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class LoginSample : MonoBehaviour
{

    private void Awake()
    {
        string ID = "OhJungGeun";
        string PW = "1234";
        string email = "example123@gmail.com";
        string nickName = "Test User";

        //회원 가입
        Backend.BMember.CustomSignUp(ID, PW);
/*
        //이메일 설정
        Backend.BMember.UpdateCustomEmail(email);

        //로그인
        Backend.BMember.CustomLogin(ID, PW);

        //아이디 찾기
        Backend.BMember.FindCustomID(email);

        //비밀번호 찾기
        Backend.BMember.ResetPassword(ID, email);

        //닉네임이 없을 때 최초 닉네임 설정
        Backend.BMember.CreateNickname(nickName);

        //닉네임 수정 => 만약 닉네임이 없을 시 CreateNickname(string)이 호출된다.
        Backend.BMember.UpdateNickname(nickName);*/
    }

}
