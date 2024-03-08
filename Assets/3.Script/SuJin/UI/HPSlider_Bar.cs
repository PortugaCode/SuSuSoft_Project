using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HPSlider_Bar : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] Image end;

    PlayerProperty playerProperty;
    private int maxValue = 1;
    private float currentValue;


    private void Start()
    {
        playerProperty = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerProperty>();

        currentValue = maxValue;
        fill.fillAmount = 1;
        playerProperty.onHPSlider = DamageHealth;
    }

    private void DamageHealth(object sender, EventArgs args)
    {
        if (currentValue < 0) currentValue = 0;
        fill.fillAmount = (float) playerProperty.currentHealth / playerProperty.maxHealth;

        SetEndImage();
    }

    private void SetEndImage()
    {
        if (playerProperty.currentHealth < playerProperty.maxHealth)
        {
            end.gameObject.SetActive(false);
        }
        else
        {
            end.gameObject.SetActive(true);
        }
    }

    private void GetHealth(object sender, EventArgs args)
    {
        fill.fillAmount += playerProperty.damage;
        if(currentValue >= maxValue)
        {
            end.gameObject.SetActive(true);
        }
    }
}
