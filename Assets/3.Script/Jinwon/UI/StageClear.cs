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
    [SerializeField] private TMP_Text starCountText;

    [Header("Player")]
    [SerializeField] private GameObject player;

    public void ShowClearUI()
    {
        int chapter = 1;
        int stageLevel = ChartManager.instance.stageInfos[0].index + 1;
        string stageName = ChartManager.instance.stageInfos[0].name_k;
        stageInfoText.text = $"{chapter} - {stageLevel} {stageName}";

        int characterIndex = 0; // 현재 사용중인 캐릭터의 인덱스 매칭 필요
        character.transform.GetChild(0).GetComponent<Image>().sprite = characterBodyImages[characterIndex];
        character.transform.GetChild(1).GetComponent<Image>().sprite = characterFaceImages[characterIndex];

        starCountText.text = $"획득한 별 개수 : {player.GetComponent<PlayerProperty>().stars.Count}";

        int reward_1 = ChartManager.instance.stageInfos[0].reward_1;
        int reward_2 = ChartManager.instance.stageInfos[0].reward_2;
        int reward_3 = ChartManager.instance.stageInfos[0].reward_3;
        int reward_gold = ChartManager.instance.stageInfos[0].reward_repeat;
        int reward_token = ChartManager.instance.stageInfos[0].reward_4;
        rewardText_1.text = $"{reward_1}";
        rewardText_2.text = $"{reward_2}";
        rewardText_3.text = $"{reward_3}";
        rewardText_gold.text = $"{reward_gold}";
        rewardText_token.text = $"{reward_token}";

        // 토큰 보상 없으면 setactive false 하기
        if (reward_token == -1)
        {
            rewardText_token.gameObject.SetActive(false);
        }
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
