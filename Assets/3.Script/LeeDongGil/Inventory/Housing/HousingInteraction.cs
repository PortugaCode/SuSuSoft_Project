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
    private float lastTouchTime = 0;
    private float doubleTouchTime = 0.5f;
    [SerializeField] private bool isFirstTouch = false;
    [SerializeField] private bool isDoubleTouch = false;
    public GameObject housing;
    private void Start()
    {
        if (Utils.Instance.nowScene == SceneNames.MatchRoom) return;
        window = FindObjectOfType<WindowCanvas>().transform.GetComponentInChildren<HousingInterationWindow>().gameObject;
        drag = GetComponent<HousingDrag>();
        lastTouchTime = Time.time;
    }
    private void Update()
    {

        if (Utils.Instance.nowScene == SceneNames.MatchRoom) return;
        if (TestManager.instance.isEditMode)
        {
            InteractionWindow();
        }

        if(isFirstTouch)
        {
            gameTime += Time.deltaTime;
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
                if (hit.collider.gameObject.CompareTag("Building") || EventSystem.current.IsPointerOverGameObject(0) == false)   //게임오브젝트 or UI터치
                {
                    if (hit.collider.gameObject.CompareTag("Building"))
                    {
                        housing = hit.collider.gameObject;
                        isFirstTouch = true;
                        if(gameTime < doubleTouchTime && isFirstTouch)
                        {
                            isFirstTouch = false;
                            isDoubleTouch = true;
                        }
                        housingObject = hit.collider.gameObject.GetComponent<HousingDrag>().housingObject;
                        if (isDoubleTouch)
                        {
                            housing.GetComponent<HousingDrag>().space.SetActive(true);
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
                else
                {
                    if (housing == null) return;
                    else
                    {
                        housing.GetComponent<HousingDrag>().space.SetActive(false);
                        isFirstTouch = false;
                        isDoubleTouch = false;
                    }
                }

            }
        }
    }

}
