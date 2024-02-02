using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    private static TestManager instance = null;

    public ItemData testData;
    public ItemData testData1;
    public InventorySystem invenSys;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            invenSys.GetItem(testData, 10);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            invenSys.GetItem(testData1, 5);
        }
    }
}
public class TestItem
{
    public int itemCount;
    public string itemName;
    public ItemType type;
}

public enum ItemType
{
    housing = 1,
    material = 2
}
