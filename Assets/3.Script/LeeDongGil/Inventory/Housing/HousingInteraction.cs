using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HousingInteraction : MonoBehaviour
{
    public GameObject window;
    private void Update()
    {
        if(!TestManager.instance.isEditMode)
        {
            InteractionWindow();
        }
    }

    private void InteractionWindow()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {

            }
        }
    }
}
