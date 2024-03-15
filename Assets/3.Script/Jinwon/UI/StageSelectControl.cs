using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageSelectControl : MonoBehaviour
{
    [Header("Stage UI")]
    [SerializeField] private Sprite starIcon_On;
    [SerializeField] private Sprite starIcon_Off;
    [SerializeField] private GameObject[] starIcons;

    private void OnEnable()
    {
        for (int i = 0; i < 5; i++) // i < 스테이지 개수 로 변경 필요
        {
            for (int j = 0; j < 3; j++)
            {
                if (DBManager.instance.user.clearInfo[i,j] == 1)
                {
                    starIcons[i].transform.GetChild(j).GetComponent<Image>().sprite = starIcon_On;
                }
                else
                {
                    starIcons[i].transform.GetChild(j).GetComponent<Image>().sprite = starIcon_Off;
                }
            }
        }
    }

    public void UseActivePoint()
    {
        // 입장시가 아닌 클리어시에 사용하는것으로 변경
        //DBManager.instance.UseActivePoint();
    }
}
