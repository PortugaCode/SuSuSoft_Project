using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfo : MonoBehaviour
{
    public ItemData _itemData;
    public int _itemCount;
    public string _itemName;
    public bool upgradeSlot = false;
    public Button button;

    [Header("Inventory Display")]
    public TextMeshProUGUI countText;
    public GameObject countObject;
    public Image image;



    public GameObject inventory;
    public GameObject create;
    public GameObject upgrade;

    private void Update()
    {
        if (!upgradeSlot)
        {
            if (_itemCount < 100)
            {
                if (_itemCount <= 0)
                {
                    image.color = new Color(1, 1, 1, 0);
                    countObject.SetActive(false);
                }
                else
                {
                    image.color = new Color(1, 1, 1, 1);
                    countObject.SetActive(true);
                }
                countText.text = string.Format("{0}", _itemCount);
            }
            else
            {
                countText.text = "99+";
            }
        }
    }

    public void SetTokenCreate()
    {
        inventory.SetActive(false);
        create.SetActive(true);
    }

    public void SetHousingUpgrade()
    {
        inventory.SetActive(false);
        upgrade.SetActive(true);
    }
}
