using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadHousing : MonoBehaviour
{
    public static LoadHousing instance = null;

    [Header("Housing System")]
    public HousingObject[] invenHousing = new HousingObject[20];
    //key : primaryIndex    value : HousingObject 구조체, 좌표
    public Dictionary<int, (HousingObject, Vector3)> localHousing = new Dictionary<int, (HousingObject, Vector3)>();

    //key : primaryIndex    value : 좌표   (클론)
    public Dictionary<int, Vector3> localCloneHousing = new Dictionary<int, Vector3>();

    //key : primaryIndex    value : HousingObject 구조체 (팝업 설명용)
    public Dictionary<int, HousingObject> localHousingObject = new Dictionary<int, HousingObject>();


    public KeyValuePair<int, (HousingObject, Vector3)> keyValue = new KeyValuePair<int, (HousingObject, Vector3)>();
    //public List<(GameObject, Vector3)> saveLocal = new List<(GameObject, Vector3)>();

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
/*
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            LoadHousingData();
        }
        else if(Input.GetKeyDown(KeyCode.W))
        {
            for(int i = 0; i<localHousing.Count;i++)
            {
                Debug.Log($"Housing Name : {localHousing[i].Item1.name_k}, Housing Position : {localHousing[i].Item2}");
            }
        }
    }
*/
    public void LoadHousingData()
    {
        //isLoading = true;
        buildSpace = FindObjectOfType<DrawingGrid>().transform;
        nowBuilding = FindObjectOfType<NowBuilding>().transform;
        
        #region 이전 작업물
        /*for (int i = 0; i < localHousing.Count; i++)
        {
            thisBuilding = Instantiate(housingGameObj, localHousing[i].Item2, Quaternion.identity, buildSpace);
            HousingDrag housing = thisBuilding.GetComponent<HousingDrag>();
            housing.housingObject = localHousing[i].Item1;
            housing.id = localHousing[i].Item1.index;
            housing.primaryIndex = keyValue
            housing.buildSprite.sprite = SpriteManager.instance.sprites[localHousing[i].Item1.imageIndex];
            housing.previousParent = nowBuilding;
            housing.moveX = localHousing[i].Item2.x;
            housing.moveY = localHousing[i].Item2.y;
            housing.clampX = localHousing[i].Item2.x;
            housing.clampY = localHousing[i].Item2.y;
        }*/
        #endregion

        #region 이전 작업물 2
        /*
                foreach (var housingDic in localHousing)
                {
                    thisBuilding = Instantiate(housingGameObj, housingDic.Value.Item2, Quaternion.identity, buildSpace);
                    HousingDrag housing = thisBuilding.GetComponent<HousingDrag>();
                    housing.housingObject = housingDic.Value.Item1;
                    housing.id = housingDic.Value.Item1.index;
                    housing.primaryIndex = housingDic.Key;
                    housing.buildSprite.sprite = SpriteManager.instance.sprites[housingDic.Value.Item1.imageIndex];
                    housing.previousParent = nowBuilding;
                    housing.moveX = housingDic.Value.Item2.x;
                    housing.moveY = housingDic.Value.Item2.y;
                    housing.clampX = housingDic.Value.Item2.x;
                    housing.clampY = housingDic.Value.Item2.y;
                    //Debug.Log("순서 0");
                }
        */
        #endregion

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
