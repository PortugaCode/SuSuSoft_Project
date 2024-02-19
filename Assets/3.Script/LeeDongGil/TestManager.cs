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
    public InventorySystem housingInvenSysWindow;

    public bool isEditMode = false;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;


        /*        if(instance == null)
                {
                    instance = this;
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }*/

        invenSys = FindObjectOfType<EditModeButton>().transform.GetChild(3).GetChild(1).GetComponent<InventorySystem>();
        housingInvenSys = FindObjectOfType<EditModeButton>().transform.GetChild(1).GetChild(0)
            .GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<InventorySystem>();
        housingInvenSysWindow = FindObjectOfType<EditModeButton>().transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<InventorySystem>();
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

    public void TestButton_Front()
    {
        housingInvenSys.GetHousingItem_test(testHousing[0], 1);
    }
    public void TestButton_Front2()
    {
        housingInvenSys.GetHousingItem_test(testHousing[5], 1);
    }
    //--------------------------------------------------------------
    public void TestButton_Back()
    {
        housingInvenSys.GetHousingItem_test(testHousing[1], 1);
    }
    public void TestButton_Back2()
    {
        housingInvenSys.GetHousingItem_test(testHousing[6], 1);
    }
    public void TestButton_Back3()
    {
        housingInvenSys.GetHousingItem_test(testHousing[10], 1);
    }
    public void TestButton_Back4()
    {
        housingInvenSys.GetHousingItem_test(testHousing[11], 1);
    }
    public void TestButton_Back5()
    {
        housingInvenSys.GetHousingItem_test(testHousing[12], 1);
    }
    //--------------------------------------------------------------
    public void TestButton_Building()
    {
        housingInvenSys.GetHousingItem_test(testHousing[2], 1);
    }
    public void TestButton_Building2()
    {
        housingInvenSys.GetHousingItem_test(testHousing[7], 1);
    }
    //--------------------------------------------------------------
    public void TestButton_Constellation()
    {
        housingInvenSys.GetHousingItem_test(testHousing[3], 1);
    }
    public void TestButton_Constellation2()
    {
        housingInvenSys.GetHousingItem_test(testHousing[8], 1);
    }
    //--------------------------------------------------------------
    public void TestButton_Special()
    {
        housingInvenSys.GetHousingItem_test(testHousing[4], 1);
    }
    public void TestButton_Special2()
    {
        housingInvenSys.GetHousingItem_test(testHousing[9], 1);
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
    interactionable = 8,
    constellation = 16,
    special = 32
}
