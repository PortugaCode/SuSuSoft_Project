using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleHousingInventory : MonoBehaviour
{
    [SerializeField] private Button editButton;
    [SerializeField] private Image editImage;
    public TextMeshProUGUI arrowText;
    public GameObject housingInventory;
    public RectTransform buttonRect;
    public RectTransform windowRect;

    private void Update()
    {
        if (housingInventory.activeSelf)
        {
            arrowText.text = "▼";
        }
        else
        {
            arrowText.text = "▲";
        }
    }

    public void InventorySetting()
    {
        if(housingInventory.activeSelf)
        {
            housingInventory.SetActive(false);
            windowRect.anchoredPosition = new Vector2(0, 320);
            buttonRect.anchoredPosition = new Vector2(200, -200);
        }
        else
        {
            windowRect.anchoredPosition = new Vector2(0, 720);
            buttonRect.anchoredPosition = new Vector2(200, 200);
            housingInventory.SetActive(true);
        }
    }
}
