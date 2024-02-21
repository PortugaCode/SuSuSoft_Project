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


    private void ResetUI()
    {
        characterUI.SetActive(false);
        inventoryUI.SetActive(false);
        testHousingButton.SetActive(false);
        editModeButton.SetActive(false);
        stageUI.SetActive(false);
    }

    public void SetChatUI(bool value)
    {
        chatUI.SetActive(value);
    }

    public void LoadScene(string sceneNames)
    {
        Utils.Instance.LoadScene(sceneNames);
    }

    public void SetInterfaceUI(GameObject a)
    {
        if (TestManager.instance.isEditMode) return;
        for(int i = 0; i < allUI.Count; i++)
        {
            allUI[i].SetActive(false);
        }
        a.SetActive(true);
    }

    public void PopUp_CharacterUI(bool value)
    {
        ResetUI();
        characterUI.SetActive(value);
    }

    public void PopUp_InventoryUI(bool value)
    {
        if (TestManager.instance.isEditMode) return;
        ResetUI();
        inventoryUI.SetActive(value);
        testHousingButton.SetActive(value);
        editModeButton.SetActive(value);
    }

    public void PopUp_StageUI(bool value)
    {
        ResetUI();
        stageUI.SetActive(value);
    }
}
