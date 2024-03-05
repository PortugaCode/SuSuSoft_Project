using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkshopBtn : MonoBehaviour
{
    [Header("TOP")]
    public List<Image> topButtons = new List<Image>();

    [Header("Filter")]
    public List<Image> filterButtons = new List<Image>();

    [Header("Workshop UI Object")]
    public GameObject filterBTN;
    public GameObject createInventory;
    public GameObject upgradeInventory;
    public GameObject sellInventory;

    public Color selectedColor = new Color();

    private void Start()
    {
        for(int i = 0; i < topButtons.Count; i++)
        {
            topButtons[i].color = Color.white;
        }
        for (int i = 0; i < filterButtons.Count; i++)
        {
            filterButtons[i].color = Color.white;
        }

        topButtons[0].color = selectedColor;
        filterButtons[0].color = selectedColor;

        createInventory.SetActive(true);
        filterBTN.SetActive(true);
    }

    public void OpenCreateInventory(Image image)
    {
        for (int i = 0; i < topButtons.Count; i++)
        {
            topButtons[i].color = Color.white;
        }
        image.color = selectedColor;

        upgradeInventory.SetActive(false);
        createInventory.SetActive(true);
        filterBTN.SetActive(true);
        sellInventory.SetActive(false);
    }

    public void OpenUpgradeInventory(Image image)
    {
        for (int i = 0; i < topButtons.Count; i++)
        {
            topButtons[i].color = Color.white;
        }
        image.color = selectedColor;

        createInventory.SetActive(false);
        upgradeInventory.SetActive(true);
        filterBTN.SetActive(false);
        sellInventory.SetActive(false);
    }

    public void OpenSellInventory(Image image)
    {
        for (int i = 0; i < topButtons.Count; i++)
        {
            topButtons[i].color = Color.white;
        }
        image.color = selectedColor;

        createInventory.SetActive(false);
        upgradeInventory.SetActive(false);
        filterBTN.SetActive(false);
        sellInventory.SetActive(true);
    }

    public void SelectedBTN(Image image)
    {
        for(int i = 0; i < filterButtons.Count; i++)
        {
            filterButtons[i].color = Color.white;
        }
        image.color = selectedColor;
    }
}
