using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BossHPSlide : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] Image start;

    private float maxValue = 1;
    private float currentValue;

    BossControll bossControll;

    private void Start()
    {
        SetStarDamage();
        SetObstacleDamge();
    }

    private void SetStarDamage()
    {
        GameObject[] stars = GameObject.FindGameObjectsWithTag("BossStar");
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].GetComponent<DamageStar>().onBossHPSlide = DamageHealth;
        }

        bossControll = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossControll>();

        currentValue = maxValue;
        fill.fillAmount = 1;
        //bossControll.onBossHPSlide = DamageHealth; 
    }

    private void SetObstacleDamge()
    {
        GameObject[] greenObstacle = GameObject.FindGameObjectsWithTag("GreenObstacle");
        for (int i = 0; i < greenObstacle.Length; i++)
        {
            greenObstacle[i].GetComponent<DamageObstacle>().onBossHPSlide = DamageHealth;
        }
    }

    private void DamageHealth(object sender, EventArgs args)
    {
        if (currentValue < 0) currentValue = 0;

        fill.fillAmount = (float)bossControll.currentHealth / bossControll.maxHealth;
        SetEndImage();
    }

    private void SetEndImage()
    {
        if (bossControll.currentHealth < bossControll.maxHealth)
        {
            start.gameObject.SetActive(false);
        }
        else
        {
            start.gameObject.SetActive(true);
        }
    }

    
}
