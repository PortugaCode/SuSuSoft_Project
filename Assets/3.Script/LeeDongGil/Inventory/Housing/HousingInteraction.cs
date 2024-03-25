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
    [SerializeField] private float lastTouchTime = 0;
    private float doubleTouchTime = 0.5f;
    public GameObject housing;
    private void Start()
    {
        if (Utils.Instance.nowScene == SceneNames.MatchRoom) return;
        window = FindObjectOfType<WindowCanvas>().transform.GetComponentInChildren<HousingInterationWindow>().gameObject;
    }
    private void Update()
    {

        if (Utils.Instance.nowScene == SceneNames.MatchRoom) return;
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

            if (touch.phase == TouchPhase.Began)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.CompareTag("Building") || EventSystem.current.IsPointerOverGameObject(0) == false)   //게임오브젝트 or UI터치
                    {
                        if (hit.collider.gameObject.CompareTag("Building"))             //이거 지우면 크래시 일어나는데 왜지?
                        {
                            Debug.Log("아아아아");
                            housing = hit.collider.gameObject;
                            housingObject = hit.collider.gameObject.GetComponent<HousingDrag>().housingObject;
                            drag = hit.collider.gameObject.GetComponent<HousingDrag>();
                            if (Time.time - lastTouchTime < doubleTouchTime)
                            {
                                drag.isDoubleTouch = true;
                            }
                            lastTouchTime = Time.time;
                            if (drag.isDoubleTouch)
                            {
                                drag.space.SetActive(true);
                                drag.subCollider.enabled = true;
                                hit.collider.gameObject.transform.position = new Vector3(
                                    hit.collider.gameObject.transform.position.x,
                                    hit.collider.gameObject.transform.position.y,
                                    -1);
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
                        else
                        {
                            //window.GetComponent<HousingInterationWindow>().firstWindow.SetActive(false);
                            window.GetComponent<HousingInterationWindow>().window.SetActive(false);
                            if (housing == null) return;
                            else
                            {
                                Debug.Log("여어어어어");
                                drag.space.SetActive(false);
                                drag.subCollider.enabled = false;
                                drag.SetZ(drag.currentLayer_);
                                drag.isDoubleTouch = false;
                            }
                        }
                    }
                }
            }
        }
    }

}
