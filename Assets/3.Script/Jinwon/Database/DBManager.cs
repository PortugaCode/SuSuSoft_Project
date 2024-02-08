using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

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
    public int lookImageIndex; // 표정 이미지 인덱스 (추가 필요할수도)
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
    public int maxReinforceLevel; // 최대 강화 수치
    public int level; // 강화 단계
    public float increaseRate; // 강화당 능력치 상승량
    public int imageIndex; // 오브젝트 이미지 인덱스
    public int interactType; // 상호작용 타입 (0: Touch, 1: Drag & Drop)
    public int price; // 판매가격
    public string reinforceText_e; // 강화 시 출력 텍스트 (영문)
    public string reinforceText_k; // 강화 시 출력 텍스트 (한글)
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
    public string receiverIndate; // 받은 유저의 inDate
    //public Dictionary<string, string> item; // 보낸 아이템 정보
    //public Dictionary<string, string> itemLocation // 해당 아이템이 위치해있던 테이블 정보
    public string receiverNickname; // 받을 유저 닉네임
    public string receivedDate; // 수령한 날짜 (수령한 경우에만 보임)
    public string sender; // 보낸 유저의 uuid
    public string inDate; // 우편의 inDate
    public string senderNickname; // 보낸 유저의 닉네임
    public string senderIndate; // 보낸 유저의 inDate
    public string sentDate; // 보낸 날짜
    public string title; // 우편 제목
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

public class User
{
    public string userID { get; set; } // 유저 아이디
    public string password { get; set; } // 유저 비밀번호
    public string userName { get; set; } // 유저 이름
    public List<Character> character { get; set; } // 보유한 캐릭터 리스트
    public Dictionary<string, int> goods { get; set; } // 보유한 재화의 종류와 수량
    public List<HousingObject> housingObject { get; set; } // 보유한 하우징 오브젝트 리스트
    public List<Friend> friend { get; set; } // 친구 리스트
    public List<int> guestBook { get; set; } // 방명록 리스트
    public List<Mail> mail { get; set; } // 우편 리스트

    public User() // 생성자에서 초기화
    {
        userID = "";
        password = "";
        userName = "";
        character = new List<Character>();
        goods = new Dictionary<string, int> { { "friendshipPoint", 0 }, { "ruby", 0 }, { "gold", 0 } };
        housingObject = new List<HousingObject>();
        friend = new List<Friend>();
        guestBook = new List<int>();
        mail = new List<Mail>();
    }

    // + 스테이지 클리어 정보 추가 필요
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

    private void Start()
    {
        InitializeServer();
    }

    private void InitializeServer() // 초기 뒤끝 서버 접속
    {
        var bro = Backend.Initialize(true);

        if (bro.IsSuccess())
        {
            Debug.Log($"서버 접속 성공 : {bro}");
        }
        else
        {
            Debug.LogError($"서버 접속 실패 : {bro}");
        }
    }

    private void SaveDataExample() // DB에 데이터 저장하기 예시
    {
        Param param = new Param(); // DB에 저장할 데이터들

        Dictionary<string, int> testDictionary = new Dictionary<string, int>
        {
            { "num1", 1 },
            { "num2", 2 },
            { "num3", 3 }
        };

        string[] testList = { "string1", "string2" };

        Param doubleParam = new Param();
        doubleParam.Add("내부1", 1234);
        doubleParam.Add("내부2", "abcd");
        doubleParam.Add("내부3", "내부 매개변수 테스트입니다.");

        param.Add("이름", "테스트2");
        param.Add("dictionary", testDictionary);
        param.Add("list", testList);
        param.Add("param", doubleParam);

        // [ 동기 ]
        Backend.GameData.Insert("dbTest", param); // '테이블명'에 삽입

        // [ 비동기 ]
        /*Backend.GameData.Insert("tableName", param, (callback) => {
            // 콜백
        });*/
    }

    private void LoadDataExample() // DB에서 데이터 불러오기 예시
    {
        // 1. 자신의 데이터만 조회 (select절의 유무와 상관없이 처리하는 데이터양은 동일)
        // public BackendReturnObject GetMyData(string tableName, Where where, string[] select, int limit, string firstKey, TableSortOrder sortOrder);

        // tableName : 테이블명
        // where : 검색할 Where 절
        // select : (Optional) row 내 존재하는 컬럼 중 포함 시키고자 하는 컬럼 (default = 모든 컬럼)
        // limit : (Optional) 불러올 게임 정보 row 수. 최소 1, 최대 100. (default = 10)
        // firstKey : (Optional) 데이터를 조회하기 위한 시작점 (default = 제일 마지막에 insert 된 데이터)
        // sortOrder : (Optional) TableSortOrder.DESC(내림차순) or TableSortOrder.ASC(오름차순) (default = TableSortOrder.DESC(내림차순))
        // * 조건 없이 데이터 조회 시 Where절을 new Where() 로 초기화 후 조회.

        // 2. 전체 유저 데이터 조회
        // public BackendReturnObject Get(string tableName, Where where, string[] select, int limit, string firstKey, TableSortOrder tableSortOrder);

        // where.Equal("owner_inDate", "유저의 gamerInDate"); 로 특정 유저의 데이터 조회
        // gamerIndate는 GetUserInfoByNickName 함수를 통해 확인하거나, 친구, 길드원 조회 등의 함수에서 리턴되는 유저의 정보에서 확인할 수 있습니다.

        // -----------------------------------------------------------------------------------------------------------------

        //데이터 추출 방법 1 : 검색한 데이터 중 첫번째 데이터의 name 컬럼 출력
        //string name = bro.Rows()[0]["name"]["S"].ToString();
        //int level = int.Parse(bro.Rows()[0]["level"]["N"].ToString());

        //데이터 추출 방법 2(언마샬) : 검색한 데이터 중 첫번째 데이터의 name 컬럼 출력
        //string name = bro.FlattenRows()[0]["name"].ToString();
        //int level = int.Parse(bro.FlattenRows()[0]["level"].ToString());

        // Where 메서드 목록 : https://docs.thebackend.io/sdk-docs/backend/base/game-information/clause-where/basic

        Where where = new Where();
        where.Equal("이름", "테스트2");

        var bro = Backend.GameData.GetMyData("dbTest", where);

        if (bro.IsSuccess() == false)
        {
            // 요청 실패 처리
            Debug.LogError("데이터 불러오기 실패");
            return;
        }

        if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
        {
            // 요청이 성공해도 where 조건에 부합하는 데이터가 없을 수 있기 때문에 데이터가 존재하는지 확인
            // 위와 같은 new Where() 조건의 경우 테이블에 row가 하나도 없으면 Count가 0 이하 일 수 있다.  
            Debug.Log("검색 조건에 해당하는 데이터가 존재하지 않음");
            return;
        }

        Debug.Log("데이터 불러오기 성공");

        // 0 번째 인덱스 데이터의 해당 컬럼 접근
        // string 형식 접근 : bro.FlattenRows()[0]["이름"].ToString(); (0번째 인덱스 데이터의 "이름" 컬럼)
        // list 형식 접근 : bro.FlattenRows()[0]["list"][0].ToString(); (0번째 인덱스 데이터의 "list" 컬럼의 0번째 인덱스 데이터)
        // dictionary 형식 접근 : bro.FlattenRows()[0]["dictionary"]["num1"].ToString(); (0번째 인덱스 데이터의 "dictionary" 컬럼의 "num1" 키의 밸류값)
        // param 형식 접근 : bro.FlattenRows()[0]["param"]["내부1"].ToString(); (0번째 인덱스 데이터의 "param" 컬럼의 "내부1" 파라미터의 값)
    }

    public void DB_Init(string idText, string pwText) // 로그인 시 DB 초기설정
    {
        Where where = new Where();
        where.Equal("owner_inDate", Backend.UserInDate); // 로그인 한 유저의 owner_inDate로 User DB 조회

        var bro = Backend.GameData.GetMyData("User", where);

        user.userID = idText;
        user.password = pwText;
        user.userName = Backend.UserNickName; // 닉네임 수정 필요

        if (bro.IsSuccess() == false || bro.GetReturnValuetoJSON()["rows"].Count <= 0) // 새로운 유저인 경우
        {
            // 데이터 초기값 삽입

            Param param = new Param(); // DB에 저장할 데이터들

            param.Add("UserID", user.userID);
            param.Add("Password", user.password);
            param.Add("UserName", user.userName);
            param.Add("Goods", user.goods); // 재화 무엇 있는지 파악하여 0 할당 필요

            Backend.GameData.Insert("User", param); // User 테이블에 데이터 삽입

            Debug.Log("새로운 유저 데이터 초기값 설정 완료");
        }
        else // 기존 유저인 경우
        {
            // 저장된 데이터를 불러와 user 클래스에 할당
            JsonData json = bro.FlattenRows(); // 캐싱

            // [보유한 재화]
            var keys = json[0]["Goods"].Keys; // JsonData를 딕셔너리 키로 변환하는 과정

            string[] goodsArray = new string[keys.Count];
            keys.CopyTo(goodsArray, 0);

            for (var i = 0; i < keys.Count; i++)
            {
                var key = goodsArray[i];
                user.goods[key] = int.Parse(json[0]["Goods"][key].ToString());
            }

            // [친구] (뒤끝 내장 친구 목록에서 불러오기)
            CommunityManager.instance.GetFriendsList();

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

            Debug.Log("기존 유저 데이터 불러오기 완료");
        }
    }

    public void AddCharacter(int index) // Character 테이블에 Chart에서 가져온 기본값을 입력 (Index로 구분)
    {
        // Chart 불러오는 과정 추가 필요

        List<Character> characters = ChartManager.instance.characterDatas; // 캐싱

        Param characterParam = new Param(); // Character 정보

        for (int i = 0; i < characters.Count; i++)
        {
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
    }
}
