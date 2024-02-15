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
        else
        {
            //InteractionPopUp();
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

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("Building"))
            {
                housingData = hit.collider.gameObject.GetComponent<HousingDrag>().data;
                gameTime += Time.deltaTime;
                if(gameTime > 0.5f)
                {
                    window.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            gameTime = 0;
        }
    }

    private void InteractionPopUp()
    {
        if (Input.touchCount > 0)
        {
            Touch touch;
            touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("Building"))
            {
                housingData = hit.collider.gameObject.GetComponent<HousingDrag>().data;
                gameTime += Time.deltaTime;
                if (gameTime > 0.5f)
                {
                    window.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
    }
}
