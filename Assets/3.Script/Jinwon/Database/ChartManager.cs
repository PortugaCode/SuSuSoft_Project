using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public class ChartManager : MonoBehaviour
{
    public static ChartManager instance;

    // 차트로 관리할 데이터 목록
    // 1. Character
    // 2. Housing Object

    public List<Character> characterDatas = new List<Character>();
    public List<HousingObject> housingObjectDatas = new List<HousingObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GetChartData() // 차트 데이터를 가져와 로컬에 저장
    {
        var bro = Backend.Chart.GetOneChartAndSave("107981", "Character"); // Character 차트

        if (bro.IsSuccess())
        {
            JsonData chartJson = JsonMapper.ToObject(Backend.Chart.GetLocalChartData("Character"));
            var rows = chartJson["rows"];

            for (int i = 0; i < rows.Count; i++)
            {
                Character currentCharacter = new Character();

                currentCharacter.index = int.Parse(rows[i]["Index"]["S"].ToString());
                currentCharacter.imageIndex = int.Parse(rows[i]["ImageIndex"]["S"].ToString());
                currentCharacter.lookImageIndex = int.Parse(rows[i]["LookImageIndex"]["S"].ToString());
                currentCharacter.name = rows[i]["Name"]["S"].ToString();
                currentCharacter.color = rows[i]["Color"]["S"].ToString();
                currentCharacter.level = int.Parse(rows[i]["Level"]["S"].ToString());
                currentCharacter.healthIncreaseRate = float.Parse(rows[i]["HealthIncreaseRate"]["S"].ToString());
                currentCharacter.maxHealth = float.Parse(rows[i]["MaxHealth"]["S"].ToString());
                currentCharacter.maxSpeed = float.Parse(rows[i]["MaxSpeed"]["S"].ToString());
                currentCharacter.minSpeed = float.Parse(rows[i]["MinSpeed"]["S"].ToString());
                currentCharacter.maxSightRange = float.Parse(rows[i]["MaxSightRange"]["S"].ToString());
                currentCharacter.minSightRange = float.Parse(rows[i]["MinSightRange"]["S"].ToString());
                currentCharacter.activeSkill = int.Parse(rows[i]["ActiveSkill"]["S"].ToString());
                currentCharacter.passiveSkill = int.Parse(rows[i]["PassiveSkill"]["S"].ToString());

                characterDatas.Add(currentCharacter);
            }
        }
        else
        {
            Debug.Log("캐릭터 차트 불러오기 실패");
            return;
        }

        var h_bro = Backend.Chart.GetOneChartAndSave("108124", "HousingObject"); // Housing Object 차트

        if (h_bro.IsSuccess())
        {
            JsonData chartJson = JsonMapper.ToObject(Backend.Chart.GetLocalChartData("HousingObject"));
            var rows = chartJson["rows"];

            for (int i = 0; i < rows.Count; i++)
            {
                HousingObject housingObject = new HousingObject();

                housingObject.index = int.Parse(rows[i]["Index"]["S"].ToString());
                housingObject.imageIndex = int.Parse(rows[i]["ImageIndex"]["S"].ToString());
                housingObject.name_e = rows[i]["Name_E"]["S"].ToString();
                housingObject.name_k = rows[i]["Name_K"]["S"].ToString();
                housingObject.type = rows[i]["Type"]["S"].ToString();
                housingObject.setType = rows[i]["SetType"]["S"].ToString();
                housingObject.effect = int.Parse(rows[i]["Effect"]["S"].ToString());
                housingObject.increaseRate = float.Parse(rows[i]["IncreaseRate"]["S"].ToString());
                housingObject.price = int.Parse(rows[i]["Price"]["S"].ToString());
                housingObject.maxLevel = int.Parse(rows[i]["MaxLevel"]["S"].ToString());
                housingObject.level = int.Parse(rows[i]["Level"]["S"].ToString());
                housingObject.interactType = int.Parse(rows[i]["InteractType"]["S"].ToString());
                housingObject.layer = int.Parse(rows[i]["Layer"]["S"].ToString());
                housingObject.text_e = rows[i]["Text_E"]["S"].ToString();
                housingObject.text_k = rows[i]["Text_K"]["S"].ToString();

                housingObjectDatas.Add(housingObject);
            }
        }
        else
        {
            Debug.Log("하우징 오브젝트 차트 불러오기 실패");
            return;
        }
    }
}
