using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HousingInteraction : MonoBehaviour
{
    public GameObject window;
    public HousingItemData housingData;
    public HousingObject housingObject;
    public float gameTime = 0;
    [SerializeField] private HousingDrag drag;

    private void Start()
    {
        window = FindObjectOfType<WindowCanvas>().transform.GetComponentInChildren<HousingInterationWindow>().gameObject;
        drag = GetComponent<HousingDrag>();
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
                        //housingData = hit.collider.gameObject.GetComponent<HousingDrag>().data;
                        Debug.Log("상호작용");
                        housingObject = hit.collider.gameObject.GetComponent<HousingDrag>().housingObject;
                        drag.isSetBuild = false;
                        window.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
                else if (!hit.collider.gameObject.CompareTag("Building") && EventSystem.current.IsPointerOverGameObject(0) == false)
                {
                    Debug.Log("여기가 실행됨?");
                    drag.isSetBuild = true;
                    window.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
        else
        {
            gameTime = 0;
        }
    }

}
