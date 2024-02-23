using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadHousing : MonoBehaviour
{
    public static LoadHousing instance = null;

    [Header("Housing System")]
    public HousingObject[] invenHousing = new HousingObject[20];
    //key : primaryIndex   value : HousingObject 구조체, 좌표
    public Dictionary<int, (HousingObject, Vector3)> localHousing = new Dictionary<int, (HousingObject, Vector3)>();
    public KeyValuePair<int, (HousingObject, Vector3)> keyValue = new KeyValuePair<int, (HousingObject, Vector3)>();
    public List<(GameObject, Vector3)> saveLocal = new List<(GameObject, Vector3)>();
    public int primaryKey = 0;
    public List<int> tempKey = new List<int>();
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

    public void LoadHousingData()
    {
        //isLoading = true;
        buildSpace = FindObjectOfType<DrawingGrid>().transform;
        nowBuilding = FindObjectOfType<NowBuilding>().transform;      //나중에 머지할 때 NowBuilding에 스크립트 넣기
        
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
            Debug.Log("순서 0");
        }
    }
}
