using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNavigationBar : MonoBehaviour
{
    public GameObject WorkShopUI;
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
}
