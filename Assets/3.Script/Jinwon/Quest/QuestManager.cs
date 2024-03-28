using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class QuestManager : MonoBehaviour
{
    [Header("Upper Tab")]
    [SerializeField] private TMP_Text tabText;

    [Header("Time Left Text")]
    [SerializeField] private TMP_Text timeLeftText;

    [Header("Day Week Tab")]
    [SerializeField] private Button dayTabButton;
    [SerializeField] private Button weekTabButton;

    [Header("Reward Tab")]
    [SerializeField] private GameObject[] checkIcons;
    private int[] rewardsQuantity = { 1, 3, 5, 7, 10 };
    private int[] rewards = { 500, 700, 800, 1000, 1 };

    [Header("Quest Tab")]
    [SerializeField] private GameObject questPrefab;
    [SerializeField] private Transform content;

    private void OnEnable()
    {
        DBManager.instance.TimerEvent += UpdateTimeText;
    }

    private void OnDisable()
    {
        DBManager.instance.TimerEvent -= UpdateTimeText;
    }

    public void OpenDayTab()
    {
        dayTabButton.interactable = false;
        weekTabButton.interactable = true;

        tabText.text = $"일일 퀘스트 목록";

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        DBManager.instance.AllResetCheck();

        ShowDayQuestList();
    }

    public void OpenWeekTab()
    {
        dayTabButton.interactable = true;
        weekTabButton.interactable = false;

        tabText.text = $"업적 퀘스트 목록";

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        DBManager.instance.AllResetCheck();
    }

    public void ShowDayQuestList()
    {
        // 일일 퀘스트 완료 횟수 체크 및 보상 지급
        for (int i = 0; i < 5; i++)
        {
            if (DBManager.instance.user.questRewardCount >= rewardsQuantity[i])
            {
                checkIcons[i].SetActive(true);

                if (i < 4)
                {
                    if (DBManager.instance.user.questRewardInfo[i] == 0)
                    {
                        DBManager.instance.user.questRewardInfo[i] = 1;
                        DBManager.instance.user.goods["gold"] += rewards[i];
                    }
                }
                else
                {
                    // 랜덤 토큰 획득 메서드
                    if (DBManager.instance.user.questRewardInfo[i] == 0)
                    {
                        DBManager.instance.user.questRewardInfo[i] = 1;
                        DBManager.instance.user.tokens[UnityEngine.Random.Range(0, DBManager.instance.user.tokens.Length)] += 1;
                    }
                }
            }
            else
            {
                checkIcons[i].SetActive(false);
            }
        }

        for (int i = 0; i < ChartManager.instance.quests.Count; i++)
        {
            GameObject questTab = Instantiate(questPrefab);
            questTab.transform.SetParent(content);

            questTab.transform.GetChild(1).GetComponent<TMP_Text>().text = ChartManager.instance.quests[i].name;

            if (DBManager.instance.user.dayQuestInfo[i] == 0)
            {
                questTab.transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>().text = $"진행중";
            }
            else if (DBManager.instance.user.dayQuestInfo[i] == 1)
            {
                questTab.transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>().text = $"완료";
            }
        }
    }

    public void CheckDayQuest()
    {
        for (int i = 0; i < ChartManager.instance.quests.Count; i++)
        {
            switch (i)
            {
                case 0:
                    // 게임 접속
                    if (DBManager.instance.user.dayQuestInfo[i] == 0)
                    {
                        DBManager.instance.user.dayQuestInfo[i] = 1;
                        DBManager.instance.user.questRewardCount += 1;
                    }
                    break;

                case 1:
                    // 스타 소모 시 체크
                    break;

                case 2:
                    // 스테이지 클리어! (1회)
                    if (DBManager.instance.user.dayQuestInfo[i] == 0)
                    {
                        for (int j = 0; j < DBManager.instance.user.clearInfo.Length; j++)
                        {
                            if (DBManager.instance.user.clearInfo[j, 0] == 1)
                            {
                                DBManager.instance.user.dayQuestInfo[i] = 1;
                                DBManager.instance.user.questRewardCount += 1;
                                break;
                            }
                        }
                    }
                    break;

                case 3:
                    // 스테이지 클리어하기! (3회)
                    if (DBManager.instance.user.dayQuestInfo[i] == 0)
                    {
                        int sum = 0;

                        for (int j = 0; j < DBManager.instance.user.clearInfo.Length; j++)
                        {
                            if (DBManager.instance.user.clearInfo[j, 0] == 1)
                            {
                                sum += 1;
                            }
                        }

                        if (sum >= 3)
                        {
                            DBManager.instance.user.dayQuestInfo[i] = 1;
                            DBManager.instance.user.questRewardCount += 1;
                        }
                    }
                    break;

                case 4:
                    // 3별 획득하기! (1회)
                    if (DBManager.instance.user.dayQuestInfo[i] == 0)
                    {
                        for (int j = 0; j < DBManager.instance.user.clearInfo.Length; j++)
                        {
                            if (DBManager.instance.user.clearInfo[j, 2] == 1)
                            {
                                DBManager.instance.user.dayQuestInfo[i] = 1;
                                DBManager.instance.user.questRewardCount += 1;
                                break;
                            }
                        }
                    }
                    break;

                case 5:
                    // 3별 획득하기! (3회)
                    if (DBManager.instance.user.dayQuestInfo[i] == 0)
                    {
                        int sum = 0;

                        for (int j = 0; j < DBManager.instance.user.clearInfo.Length; j++)
                        {
                            if (DBManager.instance.user.clearInfo[j, 2] == 1)
                            {
                                sum += 1;
                            }
                        }

                        if (sum >= 3)
                        {
                            DBManager.instance.user.dayQuestInfo[i] = 1;
                            DBManager.instance.user.questRewardCount += 1;
                        }
                    }
                    break;

                case 6:
                    // 방해물 부술 때 체크
                    break;

                case 7:
                    // 별 획득시 체크
                    break;

                case 8:
                    // 하트 획득시 체크
                    break;

                case 9:
                    // 특수 스테이지 클리어시 체크
                    break;

                case 10:
                    // 캐릭터 스킬 사용시 체크
                    break;

                case 11:
                    // 다른 친구 행성 방문시 체크
                    break;

                case 12:
                    // 하우징 토큰 구매시 체크
                    break;

                default:
                    break;
            }
        }
    }

    public void UpdateTimeText(object sender, EventArgs e)
    {
        timeLeftText.text = DBManager.instance.user.timeLeftText;
    }

    public void OpenQuestPopup()
    {
        gameObject.SetActive(true);
    }

    public void CloseQuestPopup()
    {
        gameObject.SetActive(false);
    }
}
