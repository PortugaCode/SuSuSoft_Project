using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfo : MonoBehaviour
{
    public bool upgradeSlot = false;
    public Button button;
    
    [Header("Create Item Data")]
    public ItemData _itemData;
    public int _itemCount;
    public string _itemName;


    [Header("Inventory Display")]
    public TextMeshProUGUI countText;
    public GameObject countObject;
    public Image image;
    public int requireToken = 1;


    [Header("Housing Create")]
    public GameObject inventory;
    public GameObject create;
    public GameObject upgrade;
    public Image createResult;
    public Image targetHousing;
    public CreateHousing createHousing;

    private void Update()
    {
        if (!upgradeSlot)
        {
            _itemCount = DBManager.instance.user.tokens[_itemData.itemID];
            ButtonInteraction();
        }
        else
        {
            countObject.SetActive(false);
            UpgradeButton();
        }
    }
    private void ButtonInteraction()
    {
        if (_itemCount < 100)
        {
            if (_itemData == null)
            {
                image.color = new Color32(255, 255, 255, 0);
                button.interactable = false;
                countObject.SetActive(false);
            }
            else
            {
                countObject.SetActive(true);
                if (_itemCount < requireToken)
                {
                    image.color = new Color32(255, 255, 255, 120);
                    button.interactable = false;
                }
                else
                {
                    image.color = Color.white;
                    button.interactable = true;
                }
            }
            countText.text = string.Format("{0}/{1}", _itemCount, requireToken);
        }
        else
        {
            countText.text = string.Format("99+/{0}", requireToken);
        }
    }

    private void UpgradeButton()
    {
        if (_itemData == null)
        {
            image.color = new Color(255, 255, 255, 0);
            button.interactable = false;
        }
        else
        {
            if (DBManager.instance.user.housingObject[ChartManager.instance.housingObjectDatas[_itemData.housingIndex].name_e] == 0)
            {
                image.color = new Color32(255, 255, 255, 120);
                button.interactable = false;
            }
            else
            {
                image.color = Color.white;
                button.interactable = true;
            }
        }

    }

    public void SetTokenCreate()
    {
        create.SetActive(true);
        createHousing.require = requireToken;
        createHousing.resultSlot.sprite = image.sprite;
        createHousing.tokenID = _itemData.itemID;
        createHousing.housingObject = ChartManager.instance.housingObjectDatas[_itemData.housingIndex];
        if (DBManager.instance.user.tokens[_itemData.itemID] < 5)
        {
            for(int i = 0; i < DBManager.instance.user.tokens[_itemData.itemID]; i++)
            {
                createHousing.stuff[i].sprite = _itemData.sprite;
            }
        }
        else
        {
            for (int i = 0; i < DBManager.instance.user.tokens[_itemData.itemID]; i++)
            {
                createHousing.stuff[i].sprite = _itemData.sprite;
            }
        }
        inventory.SetActive(false);
    }

    public void SetHousingUpgrade()
    {
        upgrade.SetActive(true);
        createHousing.require = requireToken;
        createHousing.resultSlot.sprite = image.sprite;
        createHousing.housingObject = ChartManager.instance.housingObjectDatas[_itemData.housingIndex];
        if (DBManager.instance.user.tokens[_itemData.itemID] < 5)
        {
            for (int i = 0; i < DBManager.instance.user.tokens[_itemData.itemID]; i++)
            {
                createHousing.stuff[i].sprite = _itemData.sprite;
            }
        }
        else
        {
            for (int i = 0; i < DBManager.instance.user.tokens[_itemData.itemID]; i++)
            {
                createHousing.stuff[i].sprite = _itemData.sprite;
            }
        }
        inventory.SetActive(false);
    }


    /*  Chart To Token
    
    chart - token
        0 - 4       쌍둥이나무
        1 - 3       나무
        4 - 8       사과나무
        5 - 1       울타리
        6 - 2       수풀
        7 - 0       농구공
        8 - 5       테니스공
        9 - 6       골프
        10 - 7      축구공
        11 - 9      트로피
     
     */
}
