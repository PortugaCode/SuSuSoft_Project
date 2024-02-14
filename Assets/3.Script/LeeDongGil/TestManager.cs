using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public static TestManager instance = null;

    public ItemData testData;
    public ItemData testData1;
    public List<HousingItemData> testHousing = new List<HousingItemData>();

    public InventorySystem invenSys;
    public InventorySystem housingInvenSys;

    public bool isEditMode = false;
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

    public Ray TouchRay()
    {
        Touch touch = Input.GetTouch(0);
        Camera cam = Camera.main;
        return cam.ScreenPointToRay(touch.position);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Test1");
            invenSys.GetItem(testData, 10);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Test2");
            invenSys.GetItem(testData1, 5);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            Debug.Log("Test3");
            housingInvenSys.GetHousingItem_test(testHousing[0], 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Debug.Log("Test4");
            housingInvenSys.GetHousingItem_test(testHousing[1], 1);
        }
    }

    public void TestButton()
    {
        housingInvenSys.GetHousingItem_test(testHousing[0], 1);
    }

    public void TestButton_1()
    {
        housingInvenSys.GetHousingItem_test(testHousing[1], 1);
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
public enum HousingType
{
    front = 1,
    back = 2,
    building = 4,
    interactionable = 8
}
