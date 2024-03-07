using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DrawUISystem : MonoBehaviour
{
    [Header("Result Charater List")]
    [SerializeField] private GameObject[] characterList;
    [SerializeField] private GameObject resultObject;


    [Header("Result Charater List_10")]
    [SerializeField] private GameObject resultObject_10;
    [SerializeField] private Dictionary<int, GameObject[]> characterDictionary = new Dictionary<int, GameObject[]>();

    [SerializeField] private GameObject[] characterList_0;
    [SerializeField] private GameObject[] characterList_1;
    [SerializeField] private GameObject[] characterList_2;
    [SerializeField] private GameObject[] characterList_3;
    [SerializeField] private GameObject[] characterList_4;
    [SerializeField] private GameObject[] characterList_5;
    [SerializeField] private GameObject[] characterList_6;
    [SerializeField] private GameObject[] characterList_7;
    [SerializeField] private GameObject[] characterList_8;
    [SerializeField] private GameObject[] characterList_9;

    [Header("Draw Gold")]
    [SerializeField] private int drawGold;

    [Header("UI")]
    [SerializeField] private GameObject drawPopUP;
    [SerializeField] private GameObject errorPopUp;
    [SerializeField] private GameObject updatePopUp;
    [SerializeField] private GameObject updatePopUp_10;

    [Header("Paticle System")]
    [SerializeField] private ParticleSystem chargeParticle_0;
    [SerializeField] private ParticleSystem chargeParticle_1;
    [SerializeField] private ParticleSystem bangParticle_0;
    [SerializeField] private ParticleSystem bangParticle_1;
    [SerializeField] private GameObject finalPaticle;

    private Coroutine drawAnimation;


    private void Start()
    {
        SetCharacterDictionary();
    }

    private void SetCharacterDictionary()
    {
        Debug.Log("리셋");
        characterDictionary.Add(0, characterList_0);
        characterDictionary.Add(1, characterList_1);
        characterDictionary.Add(2, characterList_2);
        characterDictionary.Add(3, characterList_3);
        characterDictionary.Add(4, characterList_4);
        characterDictionary.Add(5, characterList_5);
        characterDictionary.Add(6, characterList_6);
        characterDictionary.Add(7, characterList_7);
        characterDictionary.Add(8, characterList_8);
        characterDictionary.Add(9, characterList_9);
    }

    public void ClickDrawBtn()
    {
        ResetArray();
        CloseUpdatePopUp();
        CloseUpdatePopUp_10();
        //if (!CanDraw()) return;

        int a = Random.Range(0, characterList.Length);
        characterList[a].SetActive(true);
        UpdateUserCharaterData(a);



        if (drawAnimation != null)
        {
            StopCoroutine(drawAnimation);
        }
        drawAnimation = StartCoroutine(DrawAnimation(1));
    }

    private void UpdateUserCharaterData(int index)
    {
        switch (index)
        {
            case 0:
                //중복된 캐릭터 보유 메서드
                AddDrawCharater(101);
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

    public void ClickDrawBtn_10()
    {
        
        //Task.Run(() => ResetDictionary());
        
        CloseUpdatePopUp();
        CloseUpdatePopUp_10();

        //if (!CanDraw()) return;

        ResetDictionary();
        SetUserData_10();


        if (drawAnimation != null)
        {
            StopCoroutine(drawAnimation);
        }
        drawAnimation = StartCoroutine(DrawAnimation(10));
    }


    private IEnumerator DrawAnimation(int value)
    {
        chargeParticle_0.Play();
        chargeParticle_1.Play();
        yield return new WaitForSeconds(3f);
        chargeParticle_0.Stop();
        chargeParticle_1.Stop();
        bangParticle_0.Play();
        bangParticle_1.Play();
        finalPaticle.SetActive(true);

        if(value == 1)
        {
            resultObject.SetActive(true);
        }
        else
        {
            resultObject_10.SetActive(true);
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
/*        if (HaveUserIndexCharater(index))
        {
            //중복된 캐릭터 보유 메서드
        }
        else
        {
            DBManager.instance.AddCharacter(index);
        }*/
    }

    private void SetUserData_10()
    {
        for (int i = 0; i < 10; i++)
        {
            //랜덤으로 인덱스 뽑기
            int a = Random.Range(0, characterList.Length);

            //0~9 리스트 SetActiveTrue;
            characterDictionary[i][a].SetActive(true);


            //0~9 리스트 AddDrawCharater;
            UpdateUserCharaterData(a);
        }
    }

    private void ResetDictionary()
    {
        chargeParticle_0.Stop();
        chargeParticle_1.Stop();
        finalPaticle.SetActive(false);
        bangParticle_0.Stop();
        bangParticle_1.Stop();
        resultObject.SetActive(false);
        resultObject_10.SetActive(false);

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                characterDictionary[i][j].SetActive(false);
            }
        }

        //if (!CanDraw()) return;

    }

    private void ResetArray()
    {
        chargeParticle_0.Stop();
        chargeParticle_1.Stop();
        finalPaticle.SetActive(false);
        bangParticle_0.Stop();
        bangParticle_1.Stop();
        resultObject.SetActive(false);
        resultObject_10.SetActive(false);

        for (int i = 0; i < characterList.Length; i++)
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
                RaiseCharaterCount(character);
                return true;
            }
        }

        return false;
    }

    public void OpenUpdatePopUp()
    {
        updatePopUp.SetActive(true);
    }

    public void CloseUpdatePopUp()
    {
        updatePopUp.SetActive(false);
    }

    public void OpenUpdatePopUp_10()
    {
        updatePopUp_10.SetActive(true);
    }

    public void CloseUpdatePopUp_10()
    {
        updatePopUp_10.SetActive(false);
    }

    public void CloseErrorPopUp()
    {
        errorPopUp.SetActive(false);
    }

    public void CloseDrawPopUp()
    {
        Debug.Log("drawPopup Close");
        ResetArray();
        drawPopUP.SetActive(false);
    }

    private void RaiseCharaterCount(Character character)
    {
        //character.count + 1;
    }
}
