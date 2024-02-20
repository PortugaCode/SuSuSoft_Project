using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HousingInteraction : MonoBehaviour
{
    public GameObject window;
    public HousingItemData housingData;
    public float gameTime = 0;

    private void Start()
    {
        window = FindObjectOfType<WindowCanvas>().transform.GetComponentInChildren<HousingInterationWindow>().gameObject;
    }
    private void Update()
    {
        if (TestManager.instance.isEditMode)
        {
            InteractionWindow();
        }
    }

    private void InteractionWindow()
    {
        if (Input.touchCount > 0)
        {
            Touch touch;
            touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Building") || EventSystem.current.IsPointerOverGameObject(0) == true)   //게임오브젝트 or UI터치
                {
                    if (hit.collider.gameObject.CompareTag("Building"))
                    {
                        housingData = hit.collider.gameObject.GetComponent<HousingDrag>().data;
                        window.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
                else if (!hit.collider.gameObject.CompareTag("Building") && EventSystem.current.IsPointerOverGameObject(0) == false)
                {
                    window.transform.GetChild(0).gameObject.SetActive(false);
                }
                /*gameTime += Time.deltaTime;
                if(gameTime > 0.5f)
                {
                }*/
            }
        }
        else
        {
            gameTime = 0;
        }
    }

}
