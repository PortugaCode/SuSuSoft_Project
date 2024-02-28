using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarSlider_Bar : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] PlayerProperty playerProperty;

    [SerializeField] RectTransform arrow;
    private Vector2 arrowPos;
    private float arrowPosX;   

    private void Start()
    {
        fill.fillAmount = 0;
        playerProperty.onStarBar = GetStar;
        arrowPos = arrow.transform.position;
        arrowPosX = arrowPos.x;
    }

    private void Update()
    {
        arrowPosX = fill.fillAmount;
        arrowPosX = Mathf.Lerp(fill.fillAmount, playerProperty.getStarCount, arrowPos.x);
    }

    private void GetStar(object sender, EventArgs args)
    {
        fill.fillAmount = (float)playerProperty.stars.Count / playerProperty.maxStar;
    }
}
