using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpenHousingInventory : MonoBehaviour
{
    [SerializeField] private Button editButton;
    [SerializeField] private Image editImage;
    public TextMeshProUGUI arrowText;

    private void Start()
    {
        editButton = GetComponent<Button>();
    }
    private void Update()
    {
        if(TestManager.instance.isEditMode)
        {
            editImage.color = new Color32(255, 255, 255, 0);
            arrowText.color = new Color(arrowText.color.r, arrowText.color.g, arrowText.color.b, 0);
            editButton.interactable = false;
        }
        else
        {
            editImage.color = Color.white;
            arrowText.color = new Color(arrowText.color.r, arrowText.color.g, arrowText.color.b, 1);
            editButton.interactable = true;
        }
    }
}
