using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellItem : MonoBehaviour
{
    public Image smileCharater;
    public bool isHousing = false;
    public GameObject sellPopUP;

    [Header("Sell Popup")]
    public TextMeshProUGUI enName;
    public Image popupImage;
    public TextMeshProUGUI en_krName;
    public Slider housingCountbar;
    public TextMeshProUGUI housingCountText;
    public int housingCount;
    public TextMeshProUGUI price;


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

    private void SetItemInfo_Housing()
    {
        enName.text = string.Format("{0}", housingObject.name_e);
        popupImage.sprite = SpriteManager.instance.sprites[housingObject.imageIndex];
        en_krName.text = string.Format("{0} : {1}", housingObject.name_e, housingObject.name_k);
        housingCount = DBManager.instance.user.housingObject[housingObject.name_e];
        housingCountText.text = string.Format("{0}", housingCountbar.value);
        housingCountbar.maxValue = housingCount;
        price.text = string.Format("{0:#,###}", housingObject.price);
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
        DBManager.instance.user.goods["gold"] += housingObject.price * (int)housingCountbar.value;
    }
}
