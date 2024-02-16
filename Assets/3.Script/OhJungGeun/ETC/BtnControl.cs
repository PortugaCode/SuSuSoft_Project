using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnControl : MonoBehaviour
{
    [SerializeField] private GameObject chatUI;

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
        a.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SetInterfaceUI_Off(GameObject a)
    {
        a.SetActive(false);
        gameObject.SetActive(true);
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
