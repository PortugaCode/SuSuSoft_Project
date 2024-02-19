using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageClear : MonoBehaviour
{
    [SerializeField] private TMP_Text stageInfoText;
    [SerializeField] private GameObject character;

    [SerializeField] Sprite[] characterBodyImages; // 캐릭터 Body 이미지 배열
    [SerializeField] Sprite[] characterFaceImages; // 캐릭터 Face 이미지 배열

    [Header("Reward")]
    [SerializeField] private TMP_Text rewardText_1;
    [SerializeField] private TMP_Text rewardText_2;
    [SerializeField] private TMP_Text rewardText_3;
    [SerializeField] private TMP_Text rewardText_gold;
    [SerializeField] private TMP_Text rewardText_token;

    public void ShowClearUI()
    {
        int chapter = 1;
        int stageLevel = 1;
        string stageName = "튜토리얼";
        stageInfoText.text = $"{chapter} - {stageLevel} {stageName}";

        int characterIndex = 0; // 현재 사용중인 캐릭터의 인덱스 매칭 필요
        character.transform.GetChild(0).GetComponent<Image>().sprite = characterBodyImages[characterIndex];
        character.transform.GetChild(1).GetComponent<Image>().sprite = characterFaceImages[characterIndex];

        int reward_1 = 0;
        int reward_2 = 0;
        int reward_3 = 0;
        int reward_gold = 0;
        int reward_token = 0;
        rewardText_1.text = $"{reward_1}";
        rewardText_2.text = $"{reward_2}";
        rewardText_3.text = $"{reward_3}";
        rewardText_gold.text = $"{reward_gold}";
        rewardText_token.text = $"{reward_token}";
        // 토큰 보상 없으면 setactive false 하기
    }

    public void GoToHome()
    {

    }

    public void GoNextStage()
    {

    }

    public void Retry()
    {

    }
}
