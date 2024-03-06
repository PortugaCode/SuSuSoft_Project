using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CreateHousing : MonoBehaviour
{
    public GameObject inventory;
    public GameObject create;
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
    

    public List<Image> stuff = new List<Image>();
    [SerializeField] private Button createButton;

    private void OnEnable()
    {
        price.text = string.Format("{0:#,###}", priceInt);
        if (DBManager.instance.user.tokens[tokenID] < require)
        {
            createButton.interactable = false;
        }
        else
        {
            createButton.interactable = true;
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
