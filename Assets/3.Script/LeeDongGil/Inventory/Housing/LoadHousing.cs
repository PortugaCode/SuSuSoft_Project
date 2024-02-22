using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadHousing : MonoBehaviour
{
    public static LoadHousing instance = null;

    [Header("Housing System")]
    public HousingObject[] invenHousing = new HousingObject[20];
    public Dictionary<int, (HousingObject, Vector3)> localHousing = new Dictionary<int, (HousingObject, Vector3)>();
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

    private void LoadHousingData()
    {
        isLoading = true;
        buildSpace = FindObjectOfType<PolygonCollider2D>().transform;
        nowBuilding = FindObjectOfType<NowBuilding>().transform;
        for (int i = 0; i < localHousing.Count; i++)
        {
            thisBuilding = Instantiate(housingGameObj, localHousing[i].Item2, Quaternion.identity, buildSpace);
            HousingDrag housing = thisBuilding.GetComponent<HousingDrag>();
            housing.housingObject = localHousing[i].Item1;
            housing.id = localHousing[i].Item1.index;
            housing.buildSprite.sprite = SpriteManager.instance.sprites[localHousing[i].Item1.imageIndex];
            housing.previousParent = nowBuilding;
            housing.moveX = localHousing[i].Item2.x;
            housing.moveY = localHousing[i].Item2.y;
            housing.clampX = localHousing[i].Item2.x;
            housing.clampY = localHousing[i].Item2.y;
        }
    }
}
