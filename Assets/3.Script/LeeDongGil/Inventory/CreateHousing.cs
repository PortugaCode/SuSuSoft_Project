using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CreateHousing : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    public GameObject inventory;
    public GameObject create;
    public GameObject upgrade;
    public int require = 5;

    [Header("Housing Data")]
    public HousingObject housingObject;
    public int tokenID = 0;

    [Header("Housing Information")]
    public int priceInt = 500;
    public TextMeshProUGUI enName;
    public TextMeshProUGUI price;
    public Image resultSlot;

    [Header("Check PopUP")]
    public GameObject checkPopUp;
    public Image checkImage;
    public TextMeshProUGUI checkEN_KRName;
    public TextMeshProUGUI checkStuff;
    public TextMeshProUGUI checkPrice;
    public TextMeshProUGUI checkNoStuff;
    public Image checkStuffImage;
    public Image checkGoldImage;
    public TextMeshProUGUI checkUpgrade;
    public GameObject notEnoughGold;

    [Header("Create Success")]
    public GameObject background;           //팝업 시 불투명 배경
    public GameObject successPopUp;
    public TextMeshProUGUI successENName;
    public Image successImage;
    public TextMeshProUGUI EN_KRName;
    public TextMeshProUGUI enText;
    public TextMeshProUGUI krText;

    [Header("Upgrade Success")]
    public GameObject background_up;           //팝업 시 불투명 배경
    public GameObject successPopUp_up;
    public TextMeshProUGUI successENName_up;
    public Image successImage_up;
    public TextMeshProUGUI EN_KRName_up;
    public TextMeshProUGUI enText_up;
    public TextMeshProUGUI krText_up;


    public List<Image> stuff = new List<Image>();
    [SerializeField] private Button createButton;
    [SerializeField] private Button upgradeButton;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    private void OnEnable()
    {
        require = 5;
        priceInt = 500;
        Debug.Log($"{housingObject.name_e} : 팝업");
        enName.text = string.Format("{0}", housingObject.name_e);
        price.text = string.Format("{0:#,###}", priceInt);
        if (DBManager.instance.user.tokens[tokenID] < require)
        {
            upgradeButton.interactable = false;
        }
        else
        {
            upgradeButton.interactable = true;
        }
    }

    public void CheckCreate()
    {
        checkPopUp.SetActive(true);
        background.SetActive(true);
        checkImage.sprite = resultSlot.sprite;
        checkEN_KRName.text = string.Format("{0} : {1}", housingObject.name_e, housingObject.name_k);
        if (require > 0)
        {
            checkStuff.text = string.Format("x {0}", require);
            checkPrice.text = string.Format("{0:#,###}", priceInt);
            checkNoStuff.text = string.Empty;
            checkStuffImage.color = Color.white;
            checkStuffImage.sprite = stuff[0].sprite;
        }
        else
        {
            checkStuff.text = string.Empty;
            checkPrice.text = string.Empty;
            checkNoStuff.text = "없음";
            checkStuffImage.color = new Color32(255, 255, 255, 0);
            checkGoldImage.color = new Color32(255, 255, 255, 0);
        }
    }

    public void CheckUpgrade()
    {
        checkPopUp.SetActive(true);
        background_up.SetActive(true);
        checkImage.sprite = resultSlot.sprite;
        checkEN_KRName.text = string.Format("{0} : {1}", housingObject.name_e, housingObject.name_k);
        if (housingObject.level < housingObject.maxLevel)
        {
            checkUpgrade.text = string.Format("{0} → {1}", housingObject.level, housingObject.level + 1);
        }
        else
        {
            checkUpgrade.text = string.Format("최대 레벨");
        }

        if (require > 0)
        {
            checkStuff.text = string.Format("x {0}", require);
            checkPrice.text = string.Format("{0:#,###}", priceInt);
            checkNoStuff.text = string.Empty;
            checkStuffImage.color = Color.white;
            checkStuffImage.sprite = stuff[0].sprite;
        }
        else
        {
            checkStuff.text = string.Empty;
            checkPrice.text = string.Empty;
            checkNoStuff.text = "없음";
            checkStuffImage.color = new Color32(255, 255, 255, 0);
            checkGoldImage.color = new Color32(255, 255, 255, 0);
        }
    }

    public void CreateHousingObject()
    {
        //골드 빼세요
        if (DBManager.instance.user.goods["gold"] >= priceInt && DBManager.instance.user.tokens[tokenID] >= require)
        {
            DBManager.instance.user.tokens[tokenID] -= require;
            DBManager.instance.user.goods["gold"] -= priceInt;
            uiManager.UpdateGoods();
            if (DBManager.instance.user.tokens[tokenID] < require)
            {
                for (int i = 0; i < stuff.Count; i++)
                {
                    stuff[i].sprite = null;
                    stuff[i].color = new Color32(255, 255, 255, 0);
                }

                for (int i = 0; i < DBManager.instance.user.tokens[tokenID]; i++)
                {
                    stuff[i].sprite = SpriteManager.instance.tokenSprites[tokenID];
                }
            }
            PopUpSuccess();
        }
        else if (DBManager.instance.user.goods["gold"] < priceInt)
        {
            notEnoughGold.SetActive(true);
        }
    }


    public void UpgradeHousingObject()
    {
        //골드 빼세요
        if (DBManager.instance.user.goods["gold"] >= priceInt && DBManager.instance.user.tokens[tokenID] >= require)
        {
            DBManager.instance.user.goods["gold"] -= priceInt;
            DBManager.instance.user.tokens[tokenID] -= require;
            uiManager.UpdateGoods();
            if (DBManager.instance.user.tokens[tokenID] < require)
            {
                for (int i = 0; i < stuff.Count; i++)
                {
                    stuff[i].sprite = null;
                    stuff[i].color = new Color32(255, 255, 255, 0);
                }

                for (int i = 0; i < DBManager.instance.user.tokens[tokenID]; i++)
                {
                    stuff[i].sprite = SpriteManager.instance.tokenSprites[tokenID];
                }
            }
            PopUpUpgradeSuccess();
        }
        else if (DBManager.instance.user.goods["gold"] < priceInt)
        {
            notEnoughGold.SetActive(true);
        }
    }

    private void PopUpSuccess()
    {
        successPopUp.SetActive(true);
        checkPopUp.SetActive(false);
        if (DBManager.instance.user.housingObject.ContainsKey(housingObject.name_e))
        {
            DBManager.instance.user.housingObject[housingObject.name_e] += 1;
        }
        else
        {
            DBManager.instance.user.housingObject.Add(housingObject.name_e, 1);
        }
        successENName.text = string.Format("{0}", housingObject.name_e);
        successImage.sprite = resultSlot.sprite;
        EN_KRName.text = string.Format("{0} : {1}", housingObject.name_e, housingObject.name_k);
        enText.text = string.Format("{0}", housingObject.text_e);
        krText.text = string.Format("{0}", housingObject.text_k);
    }

    private void PopUpUpgradeSuccess()
    {
        successPopUp_up.SetActive(true);
        checkPopUp.SetActive(false);
        if (DBManager.instance.user.housingObject.ContainsKey(housingObject.name_e))
        {
            DBManager.instance.user.housingObject[housingObject.name_e] += 1;
        }
        successENName_up.text = string.Format("{0}", housingObject.name_e);
        successImage_up.sprite = resultSlot.sprite;
        EN_KRName_up.text = string.Format("{0} : {1}", housingObject.name_e, housingObject.name_k);
        enText_up.text = string.Format("{0}", housingObject.text_e);
        krText_up.text = string.Format("{0}", housingObject.text_k);
    }

    private void OnDisable()
    {
        for (int i = 0; i < stuff.Count; i++)
        {
            stuff[i].sprite = null;
            stuff[i].color = new Color32(255, 255, 255, 0);
        }
        resultSlot.color = new Color32(255, 255, 255, 0);
    }

    public void SaveButton()
    {
        background.SetActive(false);
        successPopUp.SetActive(false);
        inventory.SetActive(true);
        create.SetActive(false);
    }

    public void ShowList_cr()
    {
        inventory.SetActive(true);
        create.SetActive(false);
    }

    public void ShowList_up()
    {
        inventory.SetActive(true);
        upgrade.SetActive(false);
    }
}
