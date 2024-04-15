using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadHousing : MonoBehaviour
{
    public static LoadHousing instance = null;

    [Header("Housing System")]
    //key : primaryIndex    value : HousingObject 구조체 (팝업 설명용)
    public Dictionary<int, HousingObject> localHousingObject = new Dictionary<int, HousingObject>();


    public int primaryKey = 0;                      //Dictionary Key
    public List<int> tempKey = new List<int>();     //오브젝트 넣을 때 임시 키값
    public Transform buildSpace;
    public Transform nowBuilding;
    public GameObject housingGameObj;
    public GameObject thisBuilding;

    public bool isLoading = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadHousingData()
    {
        buildSpace = FindObjectOfType<DrawingGrid>().transform;
        nowBuilding = FindObjectOfType<NowBuilding>().transform;
        
        foreach (var myhousing in DBManager.instance.user.myHousingObject)
        {
            Debug.Log($"({myhousing.x}, {myhousing.y})");
            thisBuilding = Instantiate(housingGameObj, new Vector3(myhousing.x, myhousing.y, 0), Quaternion.identity, buildSpace);
            HousingDrag housing = thisBuilding.GetComponent<HousingDrag>();
            housing.housingObject = ChartManager.instance.housingObjectDatas[myhousing.index];
            housing.id = myhousing.index;
            housing.buildSprite.sprite = SpriteManager.instance.sprites[ChartManager.instance.housingObjectDatas[myhousing.index].imageIndex];
            housing.previousParent = nowBuilding;
            housing.moveX = myhousing.x;
            housing.moveY = myhousing.y;
            housing.clampX = myhousing.x;
            housing.clampY = myhousing.y;
            //Debug.Log("순서 0");
        }
    }
}
