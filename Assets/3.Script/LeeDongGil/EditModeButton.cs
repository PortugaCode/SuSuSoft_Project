using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditModeButton : MonoBehaviour
{
    public GameObject editModeBtn;
    public GameObject HousingInventory;
    public Transform cannotBuild;
    public Button button;
    public TextMeshProUGUI btnTxt;
    public bool isEdit;

    [SerializeField] private GameObject lobyPlayer;

    [Header("Housing Inventory")]
    public RectTransform housingRect;

    private void Start()
    {
        button = editModeBtn.GetComponent<Button>();
    }

    private void Update()
    {
        if (TestManager.instance.isEditMode)
        {
            TestManager.instance.isShowUI = false;
            while(housingRect.anchoredPosition.y > 0)
            {
                housingRect.anchoredPosition -= new Vector2(0, 2);
            }
            if (cannotBuild.childCount > 0)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
            isEdit = true;
            btnTxt.text = "편집모드 종료";
            if (lobyPlayer != null)
            {
                lobyPlayer.SetActive(false);
            }
        }
        else
        {
            Debug.Log(housingRect.anchoredPosition);
            TestManager.instance.isShowUI = true;
            while (housingRect.anchoredPosition.y < 350)
            {
                housingRect.anchoredPosition += new Vector2(0, 2);
            }
            isEdit = false;
            btnTxt.text = "편집모드 시작";
            if (lobyPlayer != null)
            {
                lobyPlayer.SetActive(true);
            }
        }
    }


    public void EditMode()
    {
        TestManager.instance.isEditMode = !isEdit;
    }
}
