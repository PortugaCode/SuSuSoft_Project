using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CreateHousing : MonoBehaviour
{
    public GameObject inventory;
    public GameObject create;
    public GameObject upgrade;
    public int require = 5;

    [Header("Housing Data")]
    public HousingObject housingObject;
    public int tokenID = 0;

    [Header("Housing Information")]
    public int priceInt = 9999999;
    public TextMeshProUGUI enName;
    public TextMeshProUGUI price;
    public Image resultSlot;

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

    private void OnEnable()
    {
        price.text = string.Format("{0:#,###}", priceInt);
        if (DBManager.instance.user.tokens[tokenID] < require)
        {
            createButton.interactable = false;
            upgradeButton.interactable = false;
        }
        else
        {
            createButton.interactable = true;
            upgradeButton.interactable = true;
        }
    }

    public void CreateHousingObject()
    {
        DBManager.instance.user.tokens[tokenID] -= require;
        if(DBManager.instance.user.tokens[tokenID] < require)
        {
            for(int i = 0; i < stuff.Count; i++)
            {
                stuff[i].sprite = null;
            }

            for(int i = 0; i < DBManager.instance.user.tokens[tokenID]; i++)
            {
                stuff[i].sprite = SpriteManager.instance.tokenSprites[tokenID];
            }
        }
        PopUpSuccess();
    }

    public void UpgradeHousingObject()
    {
        DBManager.instance.user.tokens[tokenID] -= require;
        if (DBManager.instance.user.tokens[tokenID] < require)
        {
            for (int i = 0; i < stuff.Count; i++)
            {
                stuff[i].sprite = null;
            }

            for (int i = 0; i < DBManager.instance.user.tokens[tokenID]; i++)
            {
                stuff[i].sprite = SpriteManager.instance.tokenSprites[tokenID];
            }
        }
        PopUpUpgradeSuccess();
    }

    private void PopUpSuccess()
    {
        background.SetActive(true);
        successPopUp.SetActive(true);
        if(DBManager.instance.user.housingObject.ContainsKey(housingObject.name_e))
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
        background_up.SetActive(true);
        successPopUp_up.SetActive(true);
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
        }
    }

    public void SaveButton()
    {
        background.SetActive(false);
        successPopUp.SetActive(false);
    }

    public void ShowList()
    {
        inventory.SetActive(true);
        create.SetActive(false);
    }
}
