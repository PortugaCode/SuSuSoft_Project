using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellPopUP : MonoBehaviour
{
    public bool isHousing = false;
    public SellItem sell;
    public InventorySystem sellHousingInven;
    public InventorySystem sellTokenInven;
    
    [Header("Sell Popup")]
    public TextMeshProUGUI enName;
    public Image popupImage;
    public TextMeshProUGUI en_krName;
    public Slider housingCountbar;
    public TextMeshProUGUI housingCountText;
    public int housingCount;
    public TextMeshProUGUI price;
    public GameObject background;

    [Header("Sell ItemData")]
    public ItemData itemData;
    public HousingObject housingObject;

    private void OnEnable()
    {
        if (DBManager.instance == null) return;

        if (isHousing)
        {
            SetItemInfo_Housing();
        }
    }

    private void Update()
    {
        housingCountText.text = string.Format("{0}", housingCountbar.value);
        price.text = string.Format("가격 : {0:#,###}", housingObject.price * housingCountbar.value);
    }

    private void SetItemInfo_Housing()
    {
        enName.text = string.Format("{0}", housingObject.name_e);
        popupImage.sprite = SpriteManager.instance.sprites[housingObject.imageIndex];
        en_krName.text = string.Format("{0} : {1}", housingObject.name_e, housingObject.name_k);
        housingCount = DBManager.instance.user.housingObject[housingObject.name_e];
        housingCountbar.value = 1;
        housingCountText.text = string.Format("{0}", housingCountbar.value);
        housingCountbar.maxValue = housingCount;
        price.text = string.Format("가격 : {0:#,###}", housingObject.price);
    }

    public void CountPlus()
    {
        if (housingCountbar.value < housingCountbar.maxValue)
        {
            housingCountbar.value += 1;
        }
    }

    public void CountMinus()
    {
        if (housingCountbar.value > housingCountbar.minValue)
        {
            housingCountbar.value -= 1;
        }
    }

    public void SellItemButton()
    {
        DBManager.instance.user.housingObject[housingObject.name_e] -= (int)housingCountbar.value;
        if(DBManager.instance.user.housingObject[housingObject.name_e] == 0)
        {
            DBManager.instance.user.housingObject.Remove(housingObject.name_e);
        }
        DBManager.instance.user.goods["gold"] += housingObject.price * (int)housingCountbar.value;
        sell.isSellItem = true;
        background.SetActive(false);
        gameObject.SetActive(false);
    }


    private void OnDisable()
    {
        sellHousingInven.LoadInventory();
    }
}
