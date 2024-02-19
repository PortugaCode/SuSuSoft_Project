using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenHousingInventory : MonoBehaviour
{
    private Button editButton;

    private void Start()
    {
        editButton = GetComponent<Button>();
    }
    private void Update()
    {
        if(TestManager.instance.isEditMode)
        {
            editButton.interactable = false;
        }
        else
        {
            editButton.interactable = true;
        }
    }
}
