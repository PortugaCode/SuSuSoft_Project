using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class SkyStarObjectControl : MonoBehaviour
{
    [SerializeField] private GameObject[] skyStars;

    [SerializeField] private bool isMatchServer;

    private void Start()
    {
        if(isMatchServer)
        {
            SetStar_Match();
            return;
        }
        SetStar_Normal();
    }

    private void SetStar_Normal()
    {
        for (int i = 0; i < 5; i++)
        {
            if (DBManager.instance.user.clearInfo[i, 2] == 1)
            {
                skyStars[i].SetActive(true);
            }
            else
            {
                break;
            }
        }
    }

    private void SetStar_Match()
    {
        var n_bro = Backend.Social.GetUserInfoByNickName(BackEndManager.Instance.GetMatchSystem().masterUser_NickName);
        string n_inDate = n_bro.GetReturnValuetoJSON()["row"]["inDate"].ToString();

        var bro = Backend.PlayerData.GetOtherData("User", n_inDate);
        int[,] masterClearinfo = new int[10,4];

        // [보상 획득 정보 (2차원 배열)]
        for (int i = 0; i < 10; i++) // (총 스테이지 개수 = 10)
        {
            for (int j = 0; j < 4; j++) // (리워드 개수 = 4)
            {
                masterClearinfo[i, j] = int.Parse(bro.FlattenRows()[0]["ClearInfo"][i][j].ToString());
            }
        }


        for (int i = 0; i < 5; i++)
        {
            if (masterClearinfo[i, 2] == 1)
            {
                skyStars[i].SetActive(true);
            }
            else
            {
                break;
            }
        }
    }
}
