using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StageSelectControl : MonoBehaviour
{
    [Header("Stage UI")]
    [SerializeField] private Sprite starIcon_On;
    [SerializeField] private Sprite starIcon_Off;
    [SerializeField] private GameObject[] starIcons;

    [Header("CheckPoint")]
    [SerializeField] private Image progressBar;
    [SerializeField] private TMP_Text[] countTexts;
    private int[] checkCounts = { 10, 20, 30 };

    [Header("Planet")]
    [SerializeField] private GameObject[] planetObjects;

    [Header("Popup")]
    [SerializeField] private GameObject starLessPopup;

    private void OnEnable()
    {
        int count = 0;

        for (int i = 1; i < 5; i++)
        {
            planetObjects[i].SetActive(false);
        }

        for (int i = 0; i < 4; i++)
        {
            if (DBManager.instance.user.clearInfo[i, 0] == 1)
            {
                planetObjects[i + 1].SetActive(true);
            }
            else
            {
                Debug.Log($"현재 {i}단계 까지 클리어했습니다");
                break;
            }
        }

        for (int i = 0; i < 5; i++) // i < 스테이지 개수 로 변경 필요
        {
            for (int j = 0; j < 3; j++)
            {
                if (DBManager.instance.user.clearInfo[i, j] == 1)
                {
                    starIcons[i].transform.GetChild(j).GetComponent<Image>().sprite = starIcon_On;
                    count += 1;
                }
                else
                {
                    starIcons[i].transform.GetChild(j).GetComponent<Image>().sprite = starIcon_Off;
                }
            }
        }

        progressBar.fillAmount = (float)(count / 5);

        for (int i = 0; i < 3; i++)
        {
            countTexts[i].text = checkCounts[i].ToString();

            if (count >= checkCounts[i] && DBManager.instance.user.stageTotalInfo[i] == 0)
            {
                DBManager.instance.user.stageTotalInfo[i] = 1;

                switch (i)
                {
                    case 0:
                        DBManager.instance.user.goods["gold"] += 1000;
                        break;
                    case 1:
                        DBManager.instance.user.goods["ruby"] += 1000;
                        break;
                    case 2:
                        DBManager.instance.user.tokens[Random.Range(0, 10)] += 1;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void UseActivePoint(int stageIndex)
    {
        if (DBManager.instance.user.activePoint <= 0)
        {
            starLessPopup.SetActive(true);
            return;
        }
        else
        {
            GetComponent<NowStageSelect>().NowStageIndex(stageIndex);
            Utils.Instance.LoadScene(SceneNames.OnGame);
        }
    }
}
