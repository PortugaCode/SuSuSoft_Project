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

        //ȸ�� ����
        Backend.BMember.CustomSignUp(ID, PW);
/*
        //�̸��� ����
        Backend.BMember.UpdateCustomEmail(email);

        //�α���
        Backend.BMember.CustomLogin(ID, PW);

        //���̵� ã��
        Backend.BMember.FindCustomID(email);

        //��й�ȣ ã��
        Backend.BMember.ResetPassword(ID, email);

        //�г����� ���� �� ���� �г��� ����
        Backend.BMember.CreateNickname(nickName);

        //�г��� ���� => ���� �г����� ���� �� CreateNickname(string)�� ȣ��ȴ�.
        Backend.BMember.UpdateNickname(nickName);*/
    }

}
