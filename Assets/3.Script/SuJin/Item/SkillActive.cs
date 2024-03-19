using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillActive : MonoBehaviour
{
    GameObject player;
    [SerializeField] float followSpeed;
    [SerializeField] Vector3 offset;

    //Cool Time
    [SerializeField] private float coolTimeMax; //CoolTime
    private float coolTime; // 남은 CoolTime

    [SerializeField] private Image skillBG;

    public Image shieldFillImage;   //남은 시간 표시 이미지 <- 우리가 바꿀 애
    public Image shieldImage;   

    public Image magneticFillImage;  
    public Image recoveryFillImage;  
    public Image speedUpFillImage;   

    public bool isItemOn = false;
    PlayerProperty playerProperty;

    [SerializeField]GameObject skillActiveUI;

    private void Awake()
    {
        skillActiveUI.SetActive(true);      //GameOver or Result 씬에서 false 시키기
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerProperty = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerProperty>();

        shieldFillImage.fillAmount = 1;

        // 지금 어떤스킬쓰는지
        if ((int)playerProperty.playerActiveSkill == 0)
        {
            shieldFillImage.sprite = shieldFillImage.sprite;
            shieldImage.sprite = shieldFillImage.sprite;
        }
        else if((int)playerProperty.playerActiveSkill == 1)
        {
            shieldFillImage.sprite = magneticFillImage.sprite;
            shieldImage.sprite = magneticFillImage.sprite;
        }
        else if ((int)playerProperty.playerActiveSkill == 2)
        {
            shieldFillImage.sprite = recoveryFillImage.sprite;
            shieldImage.sprite = recoveryFillImage.sprite;
        }
        else if ((int)playerProperty.playerActiveSkill == 3)
        {
            shieldFillImage.sprite = speedUpFillImage.sprite;
            shieldImage.sprite = speedUpFillImage.sprite;
        }
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
                    shieldFillImage.fillAmount = coolTime / coolTimeMax;
                }
                else
                {
                    isItemOn = true;
                    coolTime = coolTimeMax;
                }
            }
            else
            {
                //Debug.Log("Break");
                yield break;
            }
            //Debug.Log("CoolTime on");
            yield return null;

        }
    }
}
