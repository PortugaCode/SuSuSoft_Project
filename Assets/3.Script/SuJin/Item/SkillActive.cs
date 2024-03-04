using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SkillActive : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float followSpeed;
    [SerializeField] Vector3 offset;

    //Cool Time
    [SerializeField] private float coolTimeMax; //CoolTime
    private float coolTime; // 남은 CoolTime

    [SerializeField] private Image skillBG;
    public Image itemFillImage;   //남은 시간 표시 이미지

    public bool isItemOn = false;
    [SerializeField] PlayerProperty playerProperty;


    private void Start()
    {
        itemFillImage.fillAmount = 1;
    }

    private void FixedUpdate()
    {
        ChasePlayer();
    }

    private void ChasePlayer()
    {
        if (player != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(player.transform.position + offset);
        }
    }

    public IEnumerator CoolTime_Co()
    {

        coolTime = coolTimeMax;

        while(true)
        {
            if (!isItemOn)
            {
                if (coolTime >= 0)
                {
                    coolTime -= Time.deltaTime;
                    itemFillImage.fillAmount = coolTime / coolTimeMax;
                }
                else
                {
                    isItemOn = true;
                    coolTime = coolTimeMax;
                }
            }
            else
            {
                Debug.Log("Break");
                yield break;
            }
            Debug.Log("CoolTime on");
            yield return null;

        }
    }

    //fillAmount가 1이라면 다시 CoolTime_Co 실행
    // CoolTime이 끝났디면, ActiveSkill On.
}
