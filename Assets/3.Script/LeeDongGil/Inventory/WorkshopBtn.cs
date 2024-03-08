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
    public List<Image> sellFilterButtons = new List<Image>();

    [Header("Workshop UI Object")]
    public GameObject filterBTN;
    public GameObject createInventory;
    public GameObject upgradeInventory;
    public GameObject sellInventory;
    public GameObject createWindow;
    public GameObject upgradeWindow;
    public GameObject workShop;
    public GameObject inventory;

    public Color selectedColor = new Color();
    public bool isLoadingSell = false;

    private void Start()
    {
        for (int i = 0; i < topButtons.Count; i++)
        {
            topButtons[i].color = Color.white;
        }
        for (int i = 0; i < filterButtons.Count; i++)
        {
            filterButtons[i].color = Color.white;
        }

        topButtons[0].color = selectedColor;
        filterButtons[0].color = selectedColor;
        sellFilterButtons[0].color = selectedColor;
        inventory.SetActive(true);
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

        createInventory.SetActive(true);
        filterBTN.SetActive(true);
        upgradeInventory.SetActive(false);
        sellInventory.SetActive(false);
        createWindow.SetActive(false);
        upgradeWindow.SetActive(false);
    }

    public void OpenUpgradeInventory(Image image)
    {
        for (int i = 0; i < topButtons.Count; i++)
        {
            topButtons[i].color = Color.white;
        }
        image.color = selectedColor;

        upgradeInventory.SetActive(true);
        createInventory.SetActive(false);
        filterBTN.SetActive(false);
        sellInventory.SetActive(false);
        createWindow.SetActive(false);
        upgradeWindow.SetActive(false);
    }

    public void OpenSellInventory(Image image)
    {
        for (int i = 0; i < topButtons.Count; i++)
        {
            topButtons[i].color = Color.white;
        }
        image.color = selectedColor;

        sellInventory.SetActive(true);
        createInventory.SetActive(false);
        upgradeInventory.SetActive(false);
        filterBTN.SetActive(false);
        createWindow.SetActive(false);
        upgradeWindow.SetActive(false);
    }

    private void OpenCreateInven()
    {
        for (int i = 0; i < topButtons.Count; i++)
        {
            topButtons[i].color = Color.white;
        }
        topButtons[0].color = selectedColor;

        inventory.SetActive(true);
        createInventory.SetActive(true);
        filterBTN.SetActive(true);
        upgradeInventory.SetActive(false);
        sellInventory.SetActive(false);
        createWindow.SetActive(false);
        upgradeWindow.SetActive(false);
    }

    private void OpenUpgradeInven()
    {
        for (int i = 0; i < topButtons.Count; i++)
        {
            topButtons[i].color = Color.white;
        }
        topButtons[1].color = selectedColor;

        inventory.SetActive(true);
        upgradeInventory.SetActive(true);
        createInventory.SetActive(false);
        filterBTN.SetActive(false);
        sellInventory.SetActive(false);
        createWindow.SetActive(false);
        upgradeWindow.SetActive(false);
    }

    private void OpenSellInven()
    {
        for (int i = 0; i < topButtons.Count; i++)
        {
            topButtons[i].color = Color.white;
        }
        topButtons[2].color = selectedColor; 

        inventory.SetActive(true);
        sellInventory.SetActive(true);
        createInventory.SetActive(false);
        upgradeInventory.SetActive(false);
        filterBTN.SetActive(false);
        createWindow.SetActive(false);
        upgradeWindow.SetActive(false);
    }

    public void CloseButton()
    {
        createWindow.SetActive(false);
        upgradeWindow.SetActive(false);
        inventory.SetActive(true);
        workShop.SetActive(false);
    }

    public void LeftButton()
    {
        if (createInventory.activeSelf || createWindow.activeSelf)
        {
            return;
        }
        else if (upgradeInventory.activeSelf || upgradeWindow.activeSelf)
        {
            OpenCreateInven();
        }
        else if (sellInventory.activeSelf)
        {
            OpenUpgradeInven();
        }
    }

    public void RightButton()
    {
        if (createInventory.activeSelf || createWindow.activeSelf)
        {
            OpenUpgradeInven();
        }
        else if (upgradeInventory.activeSelf || upgradeWindow.activeSelf)
        {
            OpenSellInven();
        }
        else if (sellInventory.activeSelf)
        {
            return;
        }
    }

    public void SelectedBTN(Image image)
    {
        for (int i = 0; i < filterButtons.Count; i++)
        {
            filterButtons[i].color = Color.white;
        }
        image.color = selectedColor;
    }

    public void SellFilterBTN(Image image)
    {
        for (int i = 0; i < sellFilterButtons.Count; i++)
        {
            sellFilterButtons[i].color = Color.white;
        }
        image.color = selectedColor;
    }
}
