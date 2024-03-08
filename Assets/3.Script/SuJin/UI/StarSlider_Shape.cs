using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarSlider_Shape : MonoBehaviour
{
    [SerializeField] Image fill;
    PlayerProperty playerProperty;

    private void Start()
    {
        playerProperty = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerProperty>();

        fill.fillAmount = 0;
        playerProperty.onStarShape = GetStarShape;
    }

    private void GetStarShape(object sender, EventArgs args)
    {
        if (playerProperty.getStarCount < 0) playerProperty.getStarCount = 0;
        fill.fillAmount = (float)playerProperty.stars.Count / playerProperty.maxStar;
    }

}
