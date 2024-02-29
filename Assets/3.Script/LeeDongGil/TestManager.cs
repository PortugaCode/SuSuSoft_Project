using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public static TestManager instance = null;

    public ItemData testData;
    public ItemData testData1;
    public List<HousingItemData> testHousing = new List<HousingItemData>();
    public List<HousingItemData> testHousingV2 = new List<HousingItemData>();
    public GameObject player;

    [Header("Housing Inventory")]
    public InventorySystem invenSys;
    public InventorySystem housingInvenSys;
    public InventorySystem housingInvenSysWindow;
    public bool isHousingInventoryLoad = false;

    [Header("Housing Filter")]
    public bool isAll = true;
    public bool isFront = false;
    public bool isBack = false;
    public bool isBuilding = false;

    [Header("UI")]
    public CanvasGroup canvasGroup;
    public bool isShowUI = true;

    public bool isEditMode = false;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        #region
        /*        if(instance == null)
                {
                    instance = this;
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }*/

        //invenSys = FindObjectOfType<EditModeButton>().transform.GetChild(3).GetChild(1).GetComponent<InventorySystem>();
        //housingInvenSys = FindObjectOfType<EditModeButton>().transform.GetChild(1).GetChild(0)
        //    .GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<InventorySystem>();
        //housingInvenSysWindow = FindObjectOfType<EditModeButton>().transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<InventorySystem>();

        //ChartManager.instance.GetChartData();
        #endregion
    }

    public void SetUI(bool value)
    {
        if (value)
        {
            canvasGroup.alpha = 1.0f;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
    }

    private void Start()
    {
        /*
        #if UNITY_ANDROID
                Application.targetFrameRate = 60;
        #else
                QualitySettings.vSyncCount = 1;
        #endif
        */
        player = FindObjectOfType<TouchMove>().gameObject;
    }


    private void Update()
    {
        #region Develop Mode
        /*
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        Debug.Log("Test1");
                        invenSys.GetItem(testData, 10);
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        Debug.Log("Test2");
                        invenSys.GetItem(testData1, 5);
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha7))
                    {
                        Debug.Log("Test3");
                        housingInvenSys.GetHousingItem_test(testHousing[0], 1);
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha8))
                    {
                        Debug.Log("Test4");
                        housingInvenSys.GetHousingItem_test(testHousing[1], 1);
                    }
        */
        #endregion

        SetUI(isShowUI);

    }

    public void LoadHousingChart()
    {
        int listIndex = GetIndex(5002);
        Debug.Log($"{ChartManager.instance.housingObjectDatas[1].index} + {ChartManager.instance.housingObjectDatas[1].name_k}");
        Debug.Log($"index ID 1004의 List Index = {ChartManager.instance.housingObjectDatas.FindIndex(listIndex => listIndex.index == 1004)}");
        Debug.Log($"index ID 3002의 List Index = {ChartManager.instance.housingObjectDatas.FindIndex(listIndex => listIndex.index == 3002)}");
        Debug.Log($"index ID 5002의 List Index = {listIndex}");
    }


    public int GetIndex(int indexNum)
    {
        return ChartManager.instance.housingObjectDatas.FindIndex(listIndex => listIndex.index == indexNum);
    }



    #region Test Button
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

    public void TestButton_Interactionable()
    {
        housingInvenSys.GetHousingItem_test(testHousing[13], 1);
    }

    public void RandomGatcha()
    {
        int range = ChartManager.instance.housingObjectDatas.Count;
        int randomNum = Random.Range(0, range);
        Debug.Log("획득한 아이템");
        Debug.Log($"Index : {ChartManager.instance.housingObjectDatas[randomNum].index}");
        Debug.Log($"Name_EN : {ChartManager.instance.housingObjectDatas[randomNum].name_e}");
        Debug.Log($"Name_KR : {ChartManager.instance.housingObjectDatas[randomNum].name_k}");
        Debug.Log($"SetType : {ChartManager.instance.housingObjectDatas[randomNum].setType}");
        Debug.Log($"Type : {ChartManager.instance.housingObjectDatas[randomNum].type}");
        Debug.Log($"Price : {ChartManager.instance.housingObjectDatas[randomNum].price}");
        Debug.Log($"Text_EN : {ChartManager.instance.housingObjectDatas[randomNum].text_e}");
        Debug.Log($"Text_KR : {ChartManager.instance.housingObjectDatas[randomNum].text_k}");
    }
    #endregion

    #region Test Button 2
    public void TestV2_AppleTree()
    {
        //housingInvenSys.GetHousingItem_test(testHousingV2[0], 1);
        housingInvenSys.GetHousingItem(7, 1);
    }
    public void TestV2_Fence()
    {
        //housingInvenSys.GetHousingItem_test(testHousingV2[1], 1);
        housingInvenSys.GetHousingItem(5, 1);
    }
    public void TestV2_Bush()
    {
        //housingInvenSys.GetHousingItem_test(testHousingV2[2], 1);
        housingInvenSys.GetHousingItem(6, 1);
    }

    public void TestV2_TwinTree()
    {
        //housingInvenSys.GetHousingItem_test(testHousingV2[3], 1);
        housingInvenSys.GetHousingItem(0, 1);
    }
    public void TestV2_Tree()
    {
        //housingInvenSys.GetHousingItem_test(testHousingV2[4], 1);
        housingInvenSys.GetHousingItem(1, 1);
    }

    public void TestV2_PullBuilding()
    {
        //housingInvenSys.GetHousingItem_test(testHousingV2[5], 1);
        housingInvenSys.GetHousingItem(2, 1);
    }
    public void TestV2_CraftBuilding()
    {
        //housingInvenSys.GetHousingItem_test(testHousingV2[6], 1);
        housingInvenSys.GetHousingItem(3, 1);
    }
    #endregion
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
