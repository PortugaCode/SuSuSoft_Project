using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnControl : MonoBehaviour
{
    [SerializeField] private GameObject chatUI;

    [SerializeField] private List<GameObject> allUI;



    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject testHousingButton;
    [SerializeField] private GameObject editModeButton;
    [SerializeField] private GameObject stageUI;

    public void SetChatUI(bool value)
    {
        chatUI.SetActive(value);
    }

    public void SetInterfaceUI(GameObject a)
    {
        for(int i = 0; i < allUI.Count; i++)
        {
            allUI[i].SetActive(false);
        }
        a.SetActive(true);
    }

    public void PopUp_CharacterUI(bool value)
    {
        characterUI.SetActive(value);
    }

    public void PopUp_InventoryUI(bool value)
    {
        inventoryUI.SetActive(value);
        testHousingButton.SetActive(value);
        editModeButton.SetActive(value);
    }

    public void PopUp_StageUI(bool value)
    {
        stageUI.SetActive(value);
    }
}
