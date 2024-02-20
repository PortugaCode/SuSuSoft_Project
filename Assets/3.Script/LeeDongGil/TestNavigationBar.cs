using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNavigationBar : MonoBehaviour
{
    public GameObject WorkShopUI;
    public GameObject HousingInventoryUI;
    public GameObject HousingWindowUI;
    public GameObject EditButton;
    public void OpenWorkShop()
    {
        if (!WorkShopUI.activeSelf)
        {
            WorkShopUI.SetActive(true);
        }
        else
        {
            WorkShopUI.SetActive(false);
        }
    }

    public void OpenHousingInventory()
    {
        if (!HousingInventoryUI.activeSelf)
        {
            if (!HousingWindowUI.activeSelf)
            {
                HousingInventoryUI.SetActive(true);
                EditButton.SetActive(true);
            }
        }
        else
        {
            HousingInventoryUI.SetActive(false);
            EditButton.SetActive(false);
        }
    }
}
