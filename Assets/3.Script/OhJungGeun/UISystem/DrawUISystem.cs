using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawUISystem : MonoBehaviour
{
    [SerializeField] private GameObject[] characterList;
    [SerializeField] private int drawGold;

    [SerializeField] private GameObject drawPopUP;
    [SerializeField] private GameObject errorPopUp;


    public void ClickDrawBtn()
    {
        ResetArray();

        if (!CanDraw()) return;

        int a = Random.Range(0, characterList.Length);

        characterList[a].SetActive(true);

        switch(a)
        {
            case 0:
                //중복된 캐릭터 보유 메서드
                break;
            case 1:
                AddDrawCharater(102);
                break;
            case 2:
                AddDrawCharater(103);
                break;
            case 3:
                AddDrawCharater(104);
                break;
            case 4:
                AddDrawCharater(201);
                break;
            case 5:
                AddDrawCharater(202);
                break;
            case 6:
                AddDrawCharater(203);
                break;
            case 7:
                AddDrawCharater(204);
                break;
            case 8:
                AddDrawCharater(301);
                break;
            case 9:
                AddDrawCharater(302);
                break;
            case 10:
                AddDrawCharater(303);
                break;
            case 11:
                AddDrawCharater(304);
                break;
            case 12:
                AddDrawCharater(401);
                break;
            case 13:
                AddDrawCharater(402);
                break;
            case 14:
                AddDrawCharater(403);
                break;
            case 15:
                AddDrawCharater(404);
                break;
            case 16:
                AddDrawCharater(501);
                break;
            case 17:
                AddDrawCharater(502);
                break;
            case 18:
                AddDrawCharater(503);
                break;
            case 19:
                AddDrawCharater(504);
                break;
            default:
                Debug.Log("해당 인덱스가 없습니다.");
                break;
        }
    }

    private bool CanDraw()
    {
        if (DBManager.instance.user.goods["gold"] > drawGold) return true;
        else
        {
            errorPopUp.SetActive(true);
            return false;
        }
    }


    private void AddDrawCharater(int index)
    {
        if (HaveUserIndexCharater(index))
        {
            //중복된 캐릭터 보유 메서드
        }
        else
        {
            DBManager.instance.AddCharacter(index);
        }
    }

    private void ResetArray()
    {
        for(int i = 0; i < characterList.Length; i++)
        {
            characterList[i].SetActive(false);
        }
    }

    //유저가 해당 인덱스 캐릭터가 이미 있나요?
    private bool HaveUserIndexCharater(int index)
    {
        foreach(Character character in DBManager.instance.user.character)
        {
            if(character.index == index)
            {
                return true;
            }
        }

        return false;
    }

    public void CloseErrorPopUp()
    {
        errorPopUp.SetActive(false);
    }

    public void CloseDrawPopUp()
    {
        ResetArray();
        gameObject.SetActive(false);
    }
}
