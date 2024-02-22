using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarSlider_Bar : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] Image arrow;
    [SerializeField] PlayerProperty playerProperty;
    private Vector2 arrpwPos;

    

    private void Start()
    {
        fill.fillAmount = 0;
        arrpwPos = arrow.transform.position;
        playerProperty.onStarBar = GetStar;
    }


    private void GetStar(object sender, EventArgs args)
    {
        if (playerProperty.getStarCount < 0) playerProperty.getStarCount = 0;
        fill.fillAmount = (float)playerProperty.stars.Count / playerProperty.maxStar;

        //float fillAmout = Mathf.InverseLerp(fill.fillAmount, playerProperty.getStarCount, arrpwPos.x);
    }
}
