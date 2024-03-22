using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditModeButton : MonoBehaviour
{
    public Transform cannotBuild;

    [Header("EditButton Setting")]
    public GameObject editModeBtn;
    public GameObject HousingInventory;
    public Button button;
    public TextMeshProUGUI btnTxt;
    public bool isEdit;
    public Image currentImage;
    public Sprite[] buttonImages = new Sprite[2];
    public RectTransform buttonRect;
    public GameObject transparencyBG;

    [Header("Hide Button")]
    public GameObject hideButton;

    [SerializeField] private GameObject lobyPlayer;

    [Header("Housing Inventory")]
    public GameObject housingInven;
    public RectTransform housingRect;
    public float moveSpeed = 0.5f;

    private void Start()
    {
        button = editModeBtn.GetComponent<Button>();
    }

    private void Update()
    {
        if (TestManager.instance.isEditMode)
        {
            housingRect.anchoredPosition = Vector2.MoveTowards(housingRect.anchoredPosition, Vector2.zero, moveSpeed);
            TestManager.instance.isShowUI = false;
            hideButton.SetActive(true);
            if (cannotBuild.childCount > 0)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
            isEdit = true;
            currentImage.sprite = buttonImages[1];
            btnTxt.text = "편집모드 종료";
            if (lobyPlayer != null)
            {
                lobyPlayer.SetActive(false);
            }
        }
        else
        {
            housingRect.anchoredPosition = Vector2.MoveTowards(housingRect.anchoredPosition, new Vector2(0, 350), moveSpeed);
            hideButton.SetActive(false);
            isEdit = false;
            currentImage.sprite = buttonImages[0];
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
        housingInven.SetActive(true);
        if (transparencyBG.activeSelf)
        {
            transparencyBG.SetActive(false);
        }
        else
        {
            transparencyBG.SetActive(true);
        }
        buttonRect.anchoredPosition = new Vector2(200, 200);
        if (!TestManager.instance.isEditMode)
        {
            TestManager.instance.isShowUI = true;
        }
    }
}
