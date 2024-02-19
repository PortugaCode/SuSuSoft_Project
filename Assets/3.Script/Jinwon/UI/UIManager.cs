using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class UIManager : MonoBehaviour
{
    [Header("Character Select UI")]
    private int characterTabIndex = 0; // 캐릭터 탭(3개) 중 현재 탭 인덱스
    [SerializeField] GameObject[] characterButtons; // 캐릭터 버튼 9개
    [SerializeField] GameObject[] characterTabCounter; // 캐릭터 탭 3개
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text sightRangeText;
    [SerializeField] Sprite[] characterBodyImages; // 캐릭터 Body 이미지 배열
    [SerializeField] Sprite[] characterFaceImages; // 캐릭터 Face 이미지 배열
    private List<Character> characters;
    private List<Character> characterDatas;
    [SerializeField] GameObject currentCharacter; // 선택한 캐릭터

    [Header("Goods UI - Lobby")]
    [SerializeField] TMP_Text friendshipPointText;
    [SerializeField] TMP_Text rubyText;
    [SerializeField] TMP_Text goldText_1;

    [Header("Goods UI - StageSelect")]
    [SerializeField] TMP_Text activePointText;
    [SerializeField] TMP_Text goldText_2;

    private void OnEnable()
    {
        UpdateGoods();
    }

    private void Start()
    {
        characters = DBManager.instance.user.character;
        characterDatas = ChartManager.instance.characterDatas;
        UpdateCharacterButton();
        SelectCharacter(0);
    }

    public void UpdateGoods()
    {
        friendshipPointText.text = DBManager.instance.user.goods["friendshipPoint"].ToString();
        rubyText.text = DBManager.instance.user.goods["ruby"].ToString();
        goldText_1.text = DBManager.instance.user.goods["gold"].ToString();

        activePointText.text = $"임시"; // DB에 값 넣고 수정 필요
        goldText_2.text = DBManager.instance.user.goods["gold"].ToString();
    }

    public void NextTab()
    {
        if (characterTabIndex < 2)
        {
            characterTabIndex += 1;

            for (int i = 0; i < 3; i++)
            {
                if (i == characterTabIndex)
                {
                    Color color = characterTabCounter[i].GetComponent<Image>().color;
                    color.a = 1.0f;
                    characterTabCounter[i].GetComponent<Image>().color = color;
                }
                else
                {
                    Color color = characterTabCounter[i].GetComponent<Image>().color;
                    color.a = 0.2f;
                    characterTabCounter[i].GetComponent<Image>().color = color;
                }
            }
        }

        UpdateCharacterButton();
    }

    public void PrevTab()
    {
        if (characterTabIndex > 0)
        {
            characterTabIndex -= 1;
        }

        for (int i = 0; i < 3; i++)
        {
            if (i == characterTabIndex)
            {
                Color color = characterTabCounter[i].GetComponent<Image>().color;
                color.a = 1.0f;
                characterTabCounter[i].GetComponent<Image>().color = color;
            }
            else
            {
                Color color = characterTabCounter[i].GetComponent<Image>().color;
                color.a = 0.2f;
                characterTabCounter[i].GetComponent<Image>().color = color;
            }
        }

        UpdateCharacterButton();
    }

    public void UpdateCharacterButton()
    {
        if (characterTabIndex < 2)
        {
            // 모든 캐릭터 버튼 비활성화
            for (int i = 0; i < 9; i++)
            {
                characterButtons[i].GetComponent<Button>().interactable = false;
                characterButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = characterBodyImages[characterDatas[i + 9 * characterTabIndex].imageIndex];
                characterButtons[i].transform.GetChild(1).GetComponent<Image>().sprite = characterFaceImages[characterDatas[i + 9 * characterTabIndex].lookImageIndex + 1];
                characterButtons[i].transform.GetChild(2).GetComponent<TMP_Text>().text = $"{characterDatas[i + 9 * characterTabIndex].name}";

                Color color = characterButtons[i].GetComponent<Image>().color;
                color.a = 150.0f / 255.0f;
                characterButtons[i].GetComponent<Image>().color = color;
                color.a = 0.2f;
                characterButtons[i].transform.GetChild(0).GetComponent<Image>().color = color;
                characterButtons[i].transform.GetChild(1).GetComponent<Image>().color = color;
                characterButtons[i].transform.GetChild(2).GetComponent<TMP_Text>().color = color;
            }

            // 보유한 캐릭터 버튼 활성화
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].imageIndex >= 9 * characterTabIndex && characters[i].imageIndex <= 8 + 9 * characterTabIndex)
                {
                    int index = characters[i].imageIndex - 9 * characterTabIndex;
                    characterButtons[index].transform.GetChild(1).GetComponent<Image>().sprite = characterFaceImages[characters[i].lookImageIndex];
                    characterButtons[index].GetComponent<Button>().interactable = true;
                    Color color = characterButtons[index].GetComponent<Image>().color;
                    color.a = 1.0f;
                    characterButtons[index].GetComponent<Image>().color = color;
                    characterButtons[index].transform.GetChild(0).GetComponent<Image>().color = color;
                    characterButtons[index].transform.GetChild(1).GetComponent<Image>().color = color;
                    characterButtons[index].transform.GetChild(2).GetComponent<TMP_Text>().color = color;
                }
            }
        }
        else
        {
            // 모든 캐릭터 버튼 비활성화
            for (int i = 0; i < 9; i++)
            {
                if (i < 2)
                {
                    characterButtons[i].GetComponent<Button>().interactable = false;
                    characterButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = characterBodyImages[characterDatas[i + 9 * characterTabIndex].imageIndex];
                    characterButtons[i].transform.GetChild(1).GetComponent<Image>().sprite = characterFaceImages[characterDatas[i + 9 * characterTabIndex].lookImageIndex];
                    characterButtons[i].transform.GetChild(2).GetComponent<TMP_Text>().text = $"{characterDatas[i + 9 * characterTabIndex].name}";

                    Color color = characterButtons[i].GetComponent<Image>().color;
                    color.a = 150.0f / 255.0f;
                    characterButtons[i].GetComponent<Image>().color = color;
                    color.a = 0.2f;
                    characterButtons[i].transform.GetChild(0).GetComponent<Image>().color = color;
                    characterButtons[i].transform.GetChild(1).GetComponent<Image>().color = color;
                    characterButtons[i].transform.GetChild(2).GetComponent<TMP_Text>().color = color;
                }
                else
                {
                    Color color = characterButtons[i].GetComponent<Image>().color;
                    color.a = 0f;
                    characterButtons[i].GetComponent<Button>().interactable = false;
                    characterButtons[i].GetComponent<Image>().color = color;
                    characterButtons[i].transform.GetChild(0).GetComponent<Image>().color = color;
                    characterButtons[i].transform.GetChild(1).GetComponent<Image>().color = color;
                    characterButtons[i].transform.GetChild(2).GetComponent<TMP_Text>().color = color;
                }
            }

            // 보유한 캐릭터 버튼 활성화
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].imageIndex >= 9 * characterTabIndex && characters[i].imageIndex <= 1 + 9 * characterTabIndex)
                {
                    int index = characters[i].imageIndex - 9 * characterTabIndex;
                    characterButtons[index].GetComponent<Button>().interactable = true;
                    Color color = characterButtons[index].GetComponent<Image>().color;
                    color.a = 1.0f;
                    characterButtons[index].GetComponent<Image>().color = color;
                    characterButtons[index].transform.GetChild(0).GetComponent<Image>().color = color;
                    characterButtons[index].transform.GetChild(1).GetComponent<Image>().color = color;
                    characterButtons[index].transform.GetChild(2).GetComponent<TMP_Text>().color = color;
                }
            }
        }
    }

    public void SelectCharacter(int index)
    {
        currentCharacter.transform.GetChild(0).GetComponent<Image>().sprite = characterButtons[index].transform.GetChild(0).GetComponent<Image>().sprite;
        currentCharacter.transform.GetChild(1).GetComponent<Image>().sprite = characterButtons[index].transform.GetChild(1).GetComponent<Image>().sprite;
        currentCharacter.transform.GetChild(2).GetComponent<TMP_Text>().text = characterButtons[index].transform.GetChild(2).GetComponent<TMP_Text>().text;

        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].imageIndex == index + 9 * characterTabIndex)
            {
                healthText.text = $"체력 : {characters[i].maxHealth}";
                sightRangeText.text = $"시야 범위 : {characterDatas[i].maxSightRange}";
                break;
            }
        }
    }
}
