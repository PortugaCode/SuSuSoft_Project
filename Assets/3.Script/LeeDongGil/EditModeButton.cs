using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditModeButton : MonoBehaviour
{
    public GameObject editModeBtn;
    private void Update()
    {
        if(TestManager.instance.isEditMode)
        {
            editModeBtn.SetActive(true);
        }
        else
        {
            editModeBtn.SetActive(false);
        }
    }


    public void EndEditMode()
    {
        TestManager.instance.isEditMode = false;
    }
}
