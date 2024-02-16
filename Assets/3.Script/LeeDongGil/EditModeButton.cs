using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditModeButton : MonoBehaviour
{
    public GameObject editModeBtn;
    public GameObject HousingInventory;
    public Transform cannotBuild;
    public Button button;
    public TextMeshProUGUI btnTxt;
    public bool isEdit;

    private void Start()
    {
        button = editModeBtn.GetComponent<Button>();
    }

    private void Update()
    {
        if (TestManager.instance.isEditMode)
        {
            isEdit = true;
            btnTxt.text = "편집모드 종료";
            if (cannotBuild.childCount > 0)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
        }
        else
        {
            isEdit = false;
            btnTxt.text = "편집모드 시작";
        }
    }


    public void EditMode()
    {
        TestManager.instance.isEditMode = !isEdit;
    }
}
