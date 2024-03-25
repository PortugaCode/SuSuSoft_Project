using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellItem : MonoBehaviour
{
    public Image bodyCharater;
    public Image smileCharater;
    public GameObject sellPopUP;
    public GameObject smithSpeech;
    public bool isSellItem = false;
    [SerializeField] private InventorySystem sellHousingInven;
    [SerializeField] private InventorySystem sellTokenInven;

    [SerializeField] Sprite[] characterBodyImages;
    [SerializeField] Sprite[] characterSmileImages;

    private void OnEnable()
    {
        TestManager.instance.isAll = true;
        TestManager.instance.filterLayer = -1;
        sellHousingInven.LoadHousingInventory_Sell();
        //sellTokenInven.LoadHousingInventory();
        bodyCharater.sprite = characterBodyImages[DBManager.instance.user.currentCharacterIndex];
        switch(DBManager.instance.user.currentCharacterIndex)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                smileCharater.sprite = characterSmileImages[0];
                return;
            case 4:
            case 5:
            case 6:
            case 7:
                smileCharater.sprite = characterSmileImages[1];
                return;
            case 8:
            case 9:
            case 10:
            case 11:
                smileCharater.sprite = characterSmileImages[2];
                return;
            case 12:
            case 13:
            case 14:
            case 15:
                smileCharater.sprite = characterSmileImages[3];
                return;
            case 16:
            case 17:
            case 18:
            case 19:
                smileCharater.sprite = characterSmileImages[4];
                return;
        }
    }

    

    private void Update()
    {
        if(isSellItem)
        {
            StartCoroutine(SmithTalk());
            isSellItem = false;
        }
    }

    private void OnDisable()
    {
        isSellItem = false;
        smithSpeech.SetActive(false);
    }

    private IEnumerator SmithTalk()
    {
        smithSpeech.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        smithSpeech.SetActive(false);
    }
}
