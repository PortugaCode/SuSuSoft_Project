using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditModeButton : MonoBehaviour
{
    public GameObject editModeBtn;
    public GameObject HousingInventory;
    public Transform cannotBuild;
    public Button button;

    private void Start()
    {
        button = editModeBtn.GetComponent<Button>();
    }

    private void Update()
    {
        if(TestManager.instance.isEditMode)
        {
            editModeBtn.SetActive(true);

            if(cannotBuild.childCount > 0)
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
            editModeBtn.SetActive(false);
        }
    }

    public void StartEditMode()
    {
        TestManager.instance.isEditMode = true;
        HousingInventory.SetActive(true);
    }


    public void EndEditMode()
    {
        TestManager.instance.isEditMode = false;
        HousingInventory.SetActive(false);
    }
}
