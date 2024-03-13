using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DrawUISystem : MonoBehaviour
{
    [Header("Body & Face")]
    [SerializeField] private Sprite[] bodies;
    [SerializeField] private Sprite[] faces;



    [Header("Result Charater List")]
    [SerializeField] private GameObject resultObject;
    [SerializeField] private Image characterBody;
    [SerializeField] private Image characterFace;
    [SerializeField] private TextMeshProUGUI charaterName;
    [SerializeField] private TextMeshProUGUI charaterCount;

    [Header("Result Charater List_10")]
    [SerializeField] private GameObject resultObject_10;
    [SerializeField] private Image[] characterBody_10;
    [SerializeField] private Image[] characterFace_10;
    [SerializeField] private TextMeshProUGUI[] charaterName_10;
    [SerializeField] private TextMeshProUGUI[] charaterCount_10;
    private List<int> characterIndex = new List<int>();

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



    public void ClickDrawBtn()
    {
        CloseUpdatePopUp();
        CloseUpdatePopUp_10();

        if (!CanDraw()) return;

        ResetDrawCharacter();
        int rand = Random.Range(0, bodies.Length);
        SetCharaterImage(rand);
        UpdateUserCharaterData(rand);



        if (drawAnimation != null)
        {
            StopCoroutine(drawAnimation);
        }
        drawAnimation = StartCoroutine(DrawAnimation(1));
    }
    public void ClickDrawBtn_10()
    {
        CloseUpdatePopUp();
        CloseUpdatePopUp_10();

        if (!CanDraw(10)) return;

        ResetDrawCharacter();
        characterIndex.Clear();
        for (int i = 0; i < 10; i++)
        {
            int rand = Random.Range(0, bodies.Length);
            SetCharaterImage(rand, i);
            UpdateUserCharaterData(rand);
        }

        SetCharaterCount_10();

        if (drawAnimation != null)
        {
            StopCoroutine(drawAnimation);
        }
        drawAnimation = StartCoroutine(DrawAnimation(10));
    }

    private void SetCharaterImage(int index)
    {
        #region[Set Body]
        characterBody.sprite = bodies[index];
        #endregion
        #region [Set Face]
        if (index <= 3)
        {
            characterFace.sprite = faces[0];
        }
        else if(index <= 7 && index >= 4)
        {
            characterFace.sprite = faces[1];
        }
        else if (index <= 11 && index >= 8)
        {
            characterFace.sprite = faces[2];
        }
        else if (index <= 15 && index >= 12)
        {
            characterFace.sprite = faces[3];
        }
        else if (index <= 19 && index >= 16)
        {
            characterFace.sprite = faces[4];
        }
        #endregion
        #region[Set Name]
        switch (index)
        {
            case 0:
                charaterName.text = "옐로우 벨라";
                break;
            case 1:
                charaterName.text = "옐로우 베일리";
                break;
            case 2:
                charaterName.text = "옐로우 모모";
                break;
            case 3:
                charaterName.text = "옐로우 심바";
                break;
            case 4:
                charaterName.text = "레드 벨라";
                break;
            case 5:
                charaterName.text = "레드 베일리";
                break;
            case 6:
                charaterName.text = "레드 모모";
                break;
            case 7:
                charaterName.text = "레드 심바";
                break;
            case 8:
                charaterName.text = "블루 벨라";
                break;
            case 9:
                charaterName.text = "블루 베일리";
                break;
            case 10:
                charaterName.text = "블루 모모";
                break;
            case 11:
                charaterName.text = "블루 심바";
                break;
            case 12:
                charaterName.text = "퍼플 벨라";
                break;
            case 13:
                charaterName.text = "퍼플 베일리";
                break;
            case 14:
                charaterName.text = "퍼플 모모";
                break;
            case 15:
                charaterName.text = "퍼플 심바";
                break;
            case 16:
                charaterName.text = "그린 벨라";
                break;
            case 17:
                charaterName.text = "그린 베일리";
                break;
            case 18:
                charaterName.text = "그린 모모";
                break;
            case 19:
                charaterName.text = "그린 심바";
                break;
            default:
                Debug.Log("해당 인덱스가 없습니다.");
                break;
        }
        #endregion
    }

    private void SetCharaterImage(int index, int characterIndex)
    {
        #region[Set Body]
        characterBody_10[characterIndex].sprite = bodies[index];
        #endregion
        #region [Set Face]
        if (index <= 3)
        {
            characterFace_10[characterIndex].sprite = faces[0];
        }
        else if (index <= 7 && index >= 4)
        {
            characterFace_10[characterIndex].sprite = faces[1];
        }
        else if (index <= 11 && index >= 8)
        {
            characterFace_10[characterIndex].sprite = faces[2];
        }
        else if (index <= 15 && index >= 12)
        {
            characterFace_10[characterIndex].sprite = faces[3];
        }
        else if (index <= 19 && index >= 16)
        {
            characterFace_10[characterIndex].sprite = faces[4];
        }
        #endregion
        #region[Set Name]
        switch (index)
        {
            case 0:
                charaterName_10[characterIndex].text = "옐로우 벨라";
                break;
            case 1:
                charaterName_10[characterIndex].text = "옐로우 베일리";
                break;
            case 2:
                charaterName_10[characterIndex].text = "옐로우 모모";
                break;
            case 3:
                charaterName_10[characterIndex].text = "옐로우 심바";
                break;
            case 4:
                charaterName_10[characterIndex].text = "레드 벨라";
                break;
            case 5:
                charaterName_10[characterIndex].text = "레드 베일리";
                break;
            case 6:
                charaterName_10[characterIndex].text = "레드 모모";
                break;
            case 7:
                charaterName_10[characterIndex].text = "레드 심바";
                break;
            case 8:
                charaterName_10[characterIndex].text = "블루 벨라";
                break;
            case 9:
                charaterName_10[characterIndex].text = "블루 베일리";
                break;
            case 10:
                charaterName_10[characterIndex].text = "블루 모모";
                break;
            case 11:
                charaterName_10[characterIndex].text = "블루 심바";
                break;
            case 12:
                charaterName_10[characterIndex].text = "퍼플 벨라";
                break;
            case 13:
                charaterName_10[characterIndex].text = "퍼플 베일리";
                break;
            case 14:
                charaterName_10[characterIndex].text = "퍼플 모모";
                break;
            case 15:
                charaterName_10[characterIndex].text = "퍼플 심바";
                break;
            case 16:
                charaterName_10[characterIndex].text = "그린 벨라";
                break;
            case 17:
                charaterName_10[characterIndex].text = "그린 베일리";
                break;
            case 18:
                charaterName_10[characterIndex].text = "그린 모모";
                break;
            case 19:
                charaterName_10[characterIndex].text = "그린 심바";
                break;
            default:
                Debug.Log("해당 인덱스가 없습니다.");
                break;
        }
        #endregion
    }

    private void SetCharaterCount_10()
    {
        for(int i = 0; i < 10; i++)
        {
            switch(characterIndex[i])
            {
                case 101:
                    charaterCount_10[i].text = $"{GetCharaterCount(101)} / 30";
                    break;
                case 102:
                    charaterCount_10[i].text = $"{GetCharaterCount(102)} / 30";
                    break;
                case 103:
                    charaterCount_10[i].text = $"{GetCharaterCount(103)} / 30";
                    break;
                case 104:
                    charaterCount_10[i].text = $"{GetCharaterCount(104)} / 30";
                    break;
                case 201:
                    charaterCount_10[i].text = $"{GetCharaterCount(201)} / 30";
                    break;
                case 202:
                    charaterCount_10[i].text = $"{GetCharaterCount(202)} / 30";
                    break;
                case 203:
                    charaterCount_10[i].text = $"{GetCharaterCount(203)} / 30";
                    break;
                case 204:
                    charaterCount_10[i].text = $"{GetCharaterCount(204)} / 30";
                    break;
                case 301:
                    charaterCount_10[i].text = $"{GetCharaterCount(301)} / 30";
                    break;
                case 302:
                    charaterCount_10[i].text = $"{GetCharaterCount(302)} / 30";
                    break;
                case 303:
                    charaterCount_10[i].text = $"{GetCharaterCount(303)} / 30";
                    break;
                case 304:
                    charaterCount_10[i].text = $"{GetCharaterCount(304)} / 30";
                    break;
                case 401:
                    charaterCount_10[i].text = $"{GetCharaterCount(401)} / 30";
                    break;
                case 402:
                    charaterCount_10[i].text = $"{GetCharaterCount(402)} / 30";
                    break;
                case 403:
                    charaterCount_10[i].text = $"{GetCharaterCount(403)} / 30";
                    break;
                case 404:
                    charaterCount_10[i].text = $"{GetCharaterCount(404)} / 30";
                    break;
                case 501:
                    charaterCount_10[i].text = $"{GetCharaterCount(501)} / 30";
                    break;
                case 502:
                    charaterCount_10[i].text = $"{GetCharaterCount(502)} / 30";
                    break;
                case 503:
                    charaterCount_10[i].text = $"{GetCharaterCount(503)} / 30";
                    break;
                case 504:
                    charaterCount_10[i].text = $"{GetCharaterCount(504)} / 30";
                    break;

            }
        }
    }

    private int GetCharaterCount(int index)
    {
        foreach (Character character in DBManager.instance.user.character)
        {
            if (character.index == index)
            {
                return character.count;
            }
        }
        return 0;
    }

    private void UpdateUserCharaterData(int index)
    {
        switch (index)
        {
            case 0:
                AddDrawCharater(101);
                characterIndex.Add(101);
                break;
            case 1:
                AddDrawCharater(102);
                characterIndex.Add(102);
                break;
            case 2:
                AddDrawCharater(103);
                characterIndex.Add(103);
                break;
            case 3:
                AddDrawCharater(104);
                characterIndex.Add(104);
                break;
            case 4:
                AddDrawCharater(201);
                characterIndex.Add(201);
                break;
            case 5:
                AddDrawCharater(202);
                characterIndex.Add(202);
                break;
            case 6:
                AddDrawCharater(203);
                characterIndex.Add(203);
                break;
            case 7:
                AddDrawCharater(204);
                characterIndex.Add(204);
                break;
            case 8:
                AddDrawCharater(301);
                characterIndex.Add(301);
                break;
            case 9:
                AddDrawCharater(302);
                characterIndex.Add(302);
                break;
            case 10:
                AddDrawCharater(303);
                characterIndex.Add(303);
                break;
            case 11:
                AddDrawCharater(304);
                characterIndex.Add(304);
                break;
            case 12:
                AddDrawCharater(401);
                characterIndex.Add(401);
                break;
            case 13:
                AddDrawCharater(402);
                characterIndex.Add(402);
                break;
            case 14:
                AddDrawCharater(403);
                characterIndex.Add(403);
                break;
            case 15:
                AddDrawCharater(404);
                characterIndex.Add(404);
                break;
            case 16:
                AddDrawCharater(501);
                characterIndex.Add(501);
                break;
            case 17:
                AddDrawCharater(502);
                characterIndex.Add(502);
                break;
            case 18:
                AddDrawCharater(503);
                characterIndex.Add(503);
                break;
            case 19:
                AddDrawCharater(504);
                characterIndex.Add(504);
                break;
            default:
                Debug.Log("해당 인덱스가 없습니다.");
                break;
        }
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
        if (DBManager.instance.user.goods["gold"] > drawGold)
        {
            DBManager.instance.user.goods["gold"] -= drawGold;
            return true;
        }
        else
        {
            errorPopUp.SetActive(true);
            return false;
        }
    }

    private bool CanDraw(int index)
    {
        if (DBManager.instance.user.goods["gold"] > (drawGold * 10))
        {
            DBManager.instance.user.goods["gold"] -= (drawGold * 10);
            return true;
        }
        else
        {
            errorPopUp.SetActive(true);
            return false;
        }
    }


    private void AddDrawCharater(int index)
    {
        Debug.Log($"뽑은 캐릭터 인덱스 : {index}");
        if (HaveUserIndexCharater(index))
        {
            //중복된 캐릭터 보유 메서드
            DBManager.instance.UpdateCharacter(index);
            charaterCount.text = $"{GetCharaterCount(index)} / 30";
        }
        else
        {
            DBManager.instance.AddCharacter(index);
            charaterCount.text = $"{GetCharaterCount(index)} / 30";
        }
    }

    private void ResetDrawCharacter()
    {
        chargeParticle_0.Stop();
        chargeParticle_1.Stop();
        finalPaticle.SetActive(false);
        bangParticle_0.Stop();
        bangParticle_1.Stop();
        resultObject.SetActive(false);
        resultObject_10.SetActive(false);
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
        ResetDrawCharacter();
        drawPopUP.SetActive(false);
    }

    private void RaiseCharaterCount(Character character)
    {
        character.count += 1;
        charaterCount.text = $"{character.count} / 30";
    }
}
