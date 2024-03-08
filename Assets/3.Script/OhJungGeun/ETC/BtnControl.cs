using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnControl : MonoBehaviour
{
    [Header("Chat UI On / Off")]
    [SerializeField] private GameObject chatUI;


    [Header("InterFace UI On / Off")]
    [SerializeField] private List<GameObject> allUI;
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject workShopUI;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject inventoryWindowUI;
    [SerializeField] private GameObject testHousingButton;
    [SerializeField] private GameObject editModeButton;
    [SerializeField] private GameObject stageUI;

    [Header("Select Button Color")]
    [SerializeField] private List<Image> selectBtnList;
    [SerializeField] private Color selectColor;

    private void OnEnable()
    {
        if (selectBtnList.Count > 0)
        {
            if (TestManager.instance.isAll)
            {
                SelctBtn(selectBtnList[0]);
            }
            else if (TestManager.instance.isFront)
            {
                SelctBtn(selectBtnList[1]);
            }
            else if (TestManager.instance.isBack)
            {
                SelctBtn(selectBtnList[2]);
            }
            else if (TestManager.instance.isBuilding)
            {
                SelctBtn(selectBtnList[3]);
            }
        }
    }

    public void SelctBtn(Image me)
    {
        for (int i = 0; i < selectBtnList.Count; i++)
        {
            selectBtnList[i].color = Color.white;
        }

        me.color = selectColor;
    }




    private void ResetUI()
    {
        characterUI.SetActive(false);
        inventoryUI.SetActive(false);
        inventoryWindowUI.SetActive(false);
        testHousingButton.SetActive(false);
        editModeButton.SetActive(false);
        stageUI.SetActive(false);
        workShopUI.SetActive(false);
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
        for (int i = 0; i < allUI.Count; i++)
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
        if (inventoryWindowUI.activeSelf)
        {
            inventoryWindowUI.transform.GetComponentInChildren<InventorySystem>().GotoScroll();
            inventoryWindowUI.transform.GetChild(0).GetComponent<FilterButton>().ReturnButton();
            inventoryWindowUI.SetActive(false);
        }
        ResetUI();
        inventoryUI.SetActive(value);
        testHousingButton.SetActive(value);
        editModeButton.SetActive(value);
    }

    public void PopUp_WorkshopUI(bool value)
    {
        ResetUI();
        workShopUI.SetActive(value);
    }

    public void PopUp_StageUI(bool value)
    {
        ResetUI();
        stageUI.SetActive(value);
    }


}
