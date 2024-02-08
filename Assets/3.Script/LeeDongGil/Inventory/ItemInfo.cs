using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfo : MonoBehaviour
{

    public ItemData _itemData;
    public int _itemCount;
    public string _itemName;
    public ItemType _type;

    [Header("Inventory Display")]
    public TextMeshProUGUI countText;
    public GameObject countObject;
    public Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (_itemCount < 100)
        {
            if(_itemCount <= 0)
            {
                image.color = new Color(1, 1, 1, 0);
                countObject.SetActive(false);
            }
            else
            {
                image.color = new Color(1, 1, 1, 1);
                countObject.SetActive(true);
            }
            countText.text = string.Format("{0}", _itemCount);
        }
        else
        {
            countText.text = "99+";
        }
    }
}
