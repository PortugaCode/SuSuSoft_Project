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
    public TouchMove character;
    public Button openButton;

    private void Start()
    {
        character = TestManager.instance.player.GetComponent<TouchMove>();
    }

    private void Update()
    {
        if (housingInventory.activeSelf)
        {
            arrowText.text = "▼";
            openButton.interactable = true;
            windowRect.anchoredPosition = new Vector2(0, 720);
            buttonRect.anchoredPosition = new Vector2(200, 200);
        }
        else
        {
            arrowText.text = "▲";
            windowRect.anchoredPosition = new Vector2(0, 320);
            buttonRect.anchoredPosition = new Vector2(200, -200);
        }
    }

    public void InventorySetting()
    {
        if(housingInventory.activeSelf)
        {
            housingInventory.SetActive(false);
            
        }
        else
        {
            housingInventory.SetActive(true);
        }
    }
}
