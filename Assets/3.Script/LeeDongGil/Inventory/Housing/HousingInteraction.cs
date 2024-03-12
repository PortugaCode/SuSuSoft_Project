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
                        gameTime += Time.deltaTime;
                        housingObject = hit.collider.gameObject.GetComponent<HousingDrag>().housingObject;
                        if (gameTime > 0.5f)
                        {
                            TestManager.instance.isShowUI = false;
                            TestManager.instance.housingInven.SetActive(false);
                            if (window.GetComponent<HousingInterationWindow>().isFirstSet)
                            {
                                window.GetComponent<HousingInterationWindow>().firstWindow.SetActive(true);
                                window.GetComponent<HousingInterationWindow>().housingToggle.openButton.interactable = false;
                            }
                            else
                            {
                                window.GetComponent<HousingInterationWindow>().window.SetActive(true);
                                window.GetComponent<HousingInterationWindow>().housingToggle.openButton.interactable = false;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            gameTime = 0;
        }
    }

}
