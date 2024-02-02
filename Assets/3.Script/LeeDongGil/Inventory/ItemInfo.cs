using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemInfo : MonoBehaviour
{
    public int _itemCount;
    public string _itemName;
    public ItemType _type;
    public TextMeshProUGUI countText;

    private void Start()
    {
        TestItem test = new TestItem();
        test.itemCount = _itemCount;
        test.itemName = _itemName;
        test.type = _type;

        
    }

    private void Update()
    {
        if (_itemCount < 100)
        {
            countText.text = string.Format("{0}", _itemCount);
        }
        else
        {
            countText.text = "99+";
        }
    }
}
