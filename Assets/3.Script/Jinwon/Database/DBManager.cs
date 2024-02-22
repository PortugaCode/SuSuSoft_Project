using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using UnityEngine.SceneManagement;

public struct Friend
{
    public string id; // 아이디
    public string name; // 이름
    public string inDate; // 유저의 inDate
    public string createdAt; // 친구가 된 시각
    public string lastLogin; // 마지막 접속 시각
}

public struct Character
{
    public int index; // 인덱스
    public int imageIndex; // 이미지 인덱스
    public int lookImageIndex; // 표정 이미지 인덱스 배열의 시작 인덱스
    public string name; // 이름
    public string color; // 색상
    public int level; // 레벨
    public float healthIncreaseRate; // 레벨 당 최대 체력 증가량
    public float maxHealth; // 최대 체력
    public float maxSpeed; // 최대 속도
    public float minSpeed; // 최소 속도
    public float maxSightRange; // 최대 시야 범위
    public float minSightRange; // 최소 시야 범위
    public int activeSkill; // 보유한 액티브 스킬 인덱스
    public int passiveSkill; // 보유한 액티브 스킬 인덱스
}

public struct HousingObject
{
    public int index; // 인덱스(식별번호)
    public string name_e; // 이름 (영문)
    public string name_k; // 이름 (한글)
    public string type; // 종류 (배경, 전경, 상호작용..)
    public int layer; // 레이어 순서
    public string setType; // 세트효과 타입
    public int effect; // 효과 종류 (-1: none, 0: gold, 1: maxHP)
    public int maxLevel; // 최대 강화 수치
    public int level; // 강화 단계
    public float increaseRate; // 강화당 능력치 상승량
    public int imageIndex; // 오브젝트 이미지 인덱스
    public int interactType; // 상호작용 타입 (0: Touch, 1: Drag & Drop)
    public int price; // 판매가격
    public string text_e; // 강화 시 출력 텍스트 (영문)
    public string text_k; // 강화 시 출력 텍스트 (한글)
}

public struct GuestBook
{
    int index; // 인덱스
    string id; // 작성자 아이디
    string name; // 작성자 이름
    string content; // 내용
}

public struct Mail
{
    public string content; // 우편 내용
    public string expirationDate; // 만료 날짜
    //public string receiverIndate; // 받은 유저의 inDate
    public Goods goods; // 보낸 재화 정보
    //public Dictionary<string, string> itemLocation // 해당 아이템이 위치해있던 테이블 정보
    //public string receiverNickname; // 받을 유저 닉네임
    //public string receivedDate; // 수령한 날짜 (수령한 경우에만 보임)
    //public string sender; // 보낸 유저의 uuid
    public string inDate; // 우편의 inDate
    //public string senderNickname; // 보낸 유저의 닉네임
    //public string senderIndate; // 보낸 유저의 inDate
    public string sentDate; // 보낸 날짜
    public string title; // 우편 제목
}

public struct Goods
{
    public int index;
    public int imageIndex;
    public string name;
    public int quantity;
}

public struct Skill
{
    int index; // 인덱스
    int iconImageIndex; // 아이콘 이미지 인덱스
    string name; // 이름
    bool isActiveSkill; // 액티브스킬인지, 패시브스킬인지 bool
    float cooldown; // 쿨타임
    float count; // 횟수
    float increaseRate; // 증가량
    float percent; // 확률
    float duration; // 지속 시간
    float activation; // 발동 시간
}

public struct StageInfo
{
    public int index;
    public string name_e;
    public string name_k;
    public int condition_1; // 1별 획득 조건
    public int condition_2; // 2별 획득 조건
    public int condition_3; // 3별 획득 조건
    public int reward_1; // 골드 양
    public int reward_2; // 루비 양
    public int reward_3; // 토큰 인덱스 (하우징 오브젝트 인덱스)
    public int reward_4; // 특수 보상 인덱스 (하우징 오브젝트 인덱스)
    public int reward_repeat; // 반복 획득 골드 양
}

public class User
{
    public string userID { get; set; } // 유저 아이디
    public string password { get; set; } // 유저 비밀번호
    public string userName { get; set; } // 유저 이름
    public List<Character> character { get; set; } // 보유한 캐릭터 리스트
    public int currentCharacterIndex { get; set; } // 현재 사용중인 캐릭터 인덱스
    public Dictionary<string, int> goods { get; set; } // 보유한 재화의 종류와 수량
    public Dictionary<string, int> housingObject { get; set; } // 보유한 하우징 오브젝트 (Key : 이름, Value : 보유수량)
    public int[] tokens { get; set; } // 보유한 토큰 개수 배열
    public List<Friend> friend { get; set; } // 친구 리스트
    public List<int> guestBook { get; set; } // 방명록 리스트
    public List<Mail> mail { get; set; } // 우편 리스트

    public int[,] clearInfo { get; set; } // 최초 보상 획득 정보
    public int[] tokenInfo { get; set; } // 해당 스테이지에서 토큰을 먹었는지 여부

    public User() // 생성자에서 초기화
    {
        userID = "";
        password = "";
        userName = "";
        character = new List<Character>();
        goods = new Dictionary<string, int> { { "friendshipPoint", 0 }, { "ruby", 0 }, { "gold", 0 } };
        housingObject = new Dictionary<string, int>();
        tokens = new int[10];
        friend = new List<Friend>();
        guestBook = new List<int>();
        mail = new List<Mail>();
        clearInfo = new int[10, 4];
        tokenInfo = new int[10];
    }
}

public class DBManager : MonoBehaviour
{
    public static DBManager instance;

    public User user = new User();

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

    private void OnApplicationQuit()
    {
        // 게임 종료 시 user class값과 DB값 동기화
        SaveUserData();
    }

    public void DB_Load(string idText, string pwText) // 로그인 시 DB 초기설정
    {
        Where where = new Where();
        where.Equal("owner_inDate", Backend.UserInDate); // 로그인 한 유저의 owner_inDate로 User DB 조회

        var bro = Backend.GameData.GetMyData("User", where);

        user.userID = idText;
        user.password = pwText;
        user.userName = Backend.UserNickName;

        // 저장된 데이터를 불러와 user 클래스에 할당
        JsonData json = bro.FlattenRows(); // 캐싱

        // [보유한 재화]
        var goods_keys = json[0]["Goods"].Keys; // JsonData를 딕셔너리 키로 변환하는 과정

        string[] goodsArray = new string[goods_keys.Count];
        goods_keys.CopyTo(goodsArray, 0);

        for (var i = 0; i < goods_keys.Count; i++)
        {
            var key = goodsArray[i];
            user.goods[key] = int.Parse(json[0]["Goods"][key].ToString());
        }

        // [보유한 토큰]
        for (int i = 0; i < 10; i++) // (총 토큰 개수 = 10)
        {
            user.tokens[i] = int.Parse(bro.FlattenRows()[0]["Tokens"][i].ToString());
        }

        // [보상 획득 정보 (2차원 배열)]
        for (int i = 0; i < 10; i++) // (총 스테이지 개수 = 10)
        {
            for (int j = 0; j < 4; j++) // (리워드 개수 = 4)
            {
                user.clearInfo[i, j] = int.Parse(bro.FlattenRows()[0]["ClearInfo"][i][j].ToString());
            }
        }

        // [하우징 오브젝트 보유 정보]
        var housing_keys = json[0]["HousingObject"].Keys; // JsonData를 딕셔너리 키로 변환하는 과정

        string[] housingArray = new string[housing_keys.Count];
        housing_keys.CopyTo(housingArray, 0);

        for (var i = 0; i < housing_keys.Count; i++)
        {
            var key = housingArray[i];
            user.housingObject[key] = int.Parse(json[0]["HousingObject"][key].ToString());
        }

        // [토큰 획득 정보]
        for (int i = 0; i < 10; i++) // (총 스테이지 개수 = 10)
        {
            user.tokenInfo[i] = int.Parse(bro.FlattenRows()[0]["TokenInfo"][i].ToString());
        }

        // [캐릭터] (Character 테이블에서 불러오기)
        var c_bro = Backend.GameData.GetMyData("Character", where);

        if (c_bro.GetReturnValuetoJSON()["rows"].Count > 0)
        {
            for (int i = 0; i < c_bro.FlattenRows().Count; i++)
            {
                Character currentCharacter = new Character();

                JsonData c_json = c_bro.GetReturnValuetoJSON()["rows"];

                currentCharacter.index = int.Parse(c_json[i]["Index"][0].ToString());
                currentCharacter.imageIndex = int.Parse(c_json[i]["ImageIndex"][0].ToString());
                currentCharacter.lookImageIndex = int.Parse(c_json[i]["LookImageIndex"][0].ToString());
                currentCharacter.name = c_json[i]["Name"][0].ToString();
                currentCharacter.color = c_json[i]["Color"][0].ToString();
                currentCharacter.level = int.Parse(c_json[i]["Level"][0].ToString());
                currentCharacter.healthIncreaseRate = float.Parse(c_json[i]["HealthIncreaseRate"][0].ToString());
                currentCharacter.maxHealth = float.Parse(c_json[i]["MaxHealth"][0].ToString());
                currentCharacter.maxSpeed = float.Parse(c_json[i]["MaxSpeed"][0].ToString());
                currentCharacter.minSpeed = float.Parse(c_json[i]["MinSpeed"][0].ToString());
                currentCharacter.maxSightRange = float.Parse(c_json[i]["MaxSightRange"][0].ToString());
                currentCharacter.minSightRange = float.Parse(c_json[i]["MinSightRange"][0].ToString());
                currentCharacter.activeSkill = int.Parse(c_json[i]["ActiveSkill"][0].ToString());
                currentCharacter.passiveSkill = int.Parse(c_json[i]["PassiveSkill"][0].ToString());

                user.character.Add(currentCharacter);
            }
        }

        // [사용중인 캐릭터 인덱스]
        user.currentCharacterIndex = int.Parse(bro.GetReturnValuetoJSON()["rows"][0]["CurrentCharacterIndex"][0].ToString());

        Debug.Log("기존 유저 데이터 불러오기 완료");
    }

    public void DB_Add(string idText, string pwText, string userName)
    {
        user.userID = idText;
        user.password = pwText;
        user.userName = userName;

        // 데이터 초기값 삽입

        Param param = new Param(); // DB에 저장할 데이터들

        param.Add("UserID", user.userID);
        param.Add("Password", user.password);
        param.Add("UserName", user.userName);
        param.Add("Goods", user.goods);
        param.Add("ClearInfo", user.clearInfo);
        param.Add("TokenInfo", user.tokenInfo);
        param.Add("Tokens", user.tokens);
        param.Add("HousingObject", user.housingObject);

        AddCharacter(101);

        param.Add("CurrentCharacterIndex", 101);

        Backend.GameData.Insert("User", param); // User 테이블에 데이터 삽입

        Debug.Log("새로운 유저 데이터 초기값 설정 완료");
    }
    
    public void AddCharacter(int index) // Character 테이블에 Chart에서 가져온 기본값을 입력 (Index로 구분)
    {
        // Chart 불러오는 과정 추가 필요

        List<Character> characters = ChartManager.instance.characterDatas; // 캐싱

        Param characterParam = new Param(); // Character 정보

        for (int i = 0; i < characters.Count; i++)
        {
            Debug.Log($"index : {index}");

            if (characters[i].index == index)
            {
                characterParam.Add("Index", characters[i].index);
                characterParam.Add("ImageIndex", characters[i].imageIndex);
                characterParam.Add("LookImageIndex", characters[i].lookImageIndex);
                characterParam.Add("Name", characters[i].name);
                characterParam.Add("Color", characters[i].color);
                characterParam.Add("Level", characters[i].level);
                characterParam.Add("HealthIncreaseRate", characters[i].healthIncreaseRate);
                characterParam.Add("MaxHealth", characters[i].maxHealth);
                characterParam.Add("MaxSpeed", characters[i].maxSpeed);
                characterParam.Add("MinSpeed", characters[i].minSpeed);
                characterParam.Add("MaxSightRange", characters[i].maxSightRange);
                characterParam.Add("MinSightRange", characters[i].minSightRange);
                characterParam.Add("ActiveSkill", characters[i].activeSkill);
                characterParam.Add("PassiveSkill", characters[i].passiveSkill);
                break;
            }
        }

        Backend.GameData.Insert("Character", characterParam); // Character 테이블에 데이터 삽입

        Debug.Log("캐릭터 추가 완료");
    }

    public int CharacterIndexMatching(int index)
    {
        int returnIndex = -1;

        switch (index)
        {
            case 101:
                returnIndex = 0;
                break;

            case 102:
                returnIndex = 1;
                break;

            case 103:
                returnIndex = 2;
                break;

            case 104:
                returnIndex = 3;
                break;

            case 201:
                returnIndex = 4;
                break;

            case 202:
                returnIndex = 5;
                break;

            case 203:
                returnIndex = 6;
                break;

            case 204:
                returnIndex = 7;
                break;

            case 301:
                returnIndex = 8;
                break;

            case 302:
                returnIndex = 9;
                break;

            case 303:
                returnIndex = 10;
                break;

            case 304:
                returnIndex = 11;
                break;

            case 401:
                returnIndex = 12;
                break;

            case 402:
                returnIndex = 13;
                break;

            case 403:
                returnIndex = 14;
                break;

            case 404:
                returnIndex = 15;
                break;

            case 501:
                returnIndex = 16;
                break;

            case 502:
                returnIndex = 17;
                break;

            case 503:
                returnIndex = 18;
                break;

            case 504:
                returnIndex = 19;
                break;

            default:
                break;
        }

        return returnIndex;
    }

    public void SaveUserData()
    {
        Param param = new Param();
        param.Add("CurrentCharacterIndex", user.currentCharacterIndex);
        param.Add("Goods", user.goods);
        param.Add("ClearInfo", user.clearInfo);
        param.Add("TokenInfo", user.tokenInfo);
        param.Add("Tokens", user.tokens);
        param.Add("HousingObject", user.housingObject);

        Backend.PlayerData.UpdateMyLatestData("User", param);
    }
}
