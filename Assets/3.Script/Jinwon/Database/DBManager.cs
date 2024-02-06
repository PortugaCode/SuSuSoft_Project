using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public struct Friend
{
    int index; // 인덱스
    string id; // 아이디
    string name; // 이름
}

public struct Character
{
    int index; // 인덱스
    int imageIndex; // 이미지 인덱스
    int lookImageIndex; // 표정 이미지 인덱스
    string name; // 이름
    string color; // 색상
    int level; // 레벨
    float levelIncreaseRate; // 레벨 당 증가량
    float maxHealth; // 최대 체력
    float maxSpeed; // 최대 속도
    float minSpeed; // 최소 속도
    float maxSightRange; // 최대 시야 범위
    float minSightRange; // 최소 시야 범위
    float hitboxRange; // 히트박스 범위
    float magnetRange; // 자성 범위
    float damageReductionRate; // 피해 감소율
    float recoveryRate; // 회복량
    float naturalRecoveryRate; // 자연 회복량
    float itemAttackPower; // 아이템 공격력
    float goldAddtionalRate; // 골드 추가 획득률
    float starAddtionalRate; // 별 추가 획득률
    List<Skill> skill; // 보유한 스킬 리스트
}

public struct HousingObject
{
    int index; // 인덱스(아이디)
    string name; // 이름
    string type; // 타입
    string setType; // 세트효과 타입
    string interactType; // 상호작용 타입
    int reinforceLevel; // 강화 단계
    int maxReinforceLevel; // 최대 강화 단계
    float statIncreaseRate; // 능력치 상승량
    int layer; // 레이어

    // + 세트효과 상승 능력치 추가 필요
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
    int index; // 인덱스
    string id; // 작성자 아이디
    string name; // 작성자 이름
    string content; // 내용
}

public struct Skill
{
    int index; // 인덱스
    int iconImageIndex; // 아이콘 이미지 인덱스
    string name; // 이름
    bool isActiveSkill; // 액티브 스킬 여부
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
    public List<int> character { get; set; } // 보유한 캐릭터 리스트
    public Dictionary<string, int> goods { get; set; } // 보유한 재화의 종류와 수량
    public List<int> housingObject { get; set; } // 보유한 하우징 오브젝트 리스트
    public List<string> friend { get; set; } // 친구 리스트
    public List<int> guestBook { get; set; } // 방명록 리스트
    public List<int> mail { get; set; } // 우편 리스트

    public User() // 생성자에서 초기화
    {
        userID = "";
        password = "";
        userName = "";
        character = new List<int>();
        goods = new Dictionary<string, int> { { "friendshipPoint", 0 }, { "ruby", 0 }, { "gold", 0 } };
        housingObject = new List<int>();
        friend = new List<string>();
        guestBook = new List<int>();
        mail = new List<int>();
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
        where.Equal("owner_inDate", Backend.UserInDate); // 로그인 한 유저의 owner_inDate로 DB 조회

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
            param.Add("Character", user.character);
            param.Add("Goods", user.goods); // 재화 무엇 있는지 파악하여 0 할당 필요
            param.Add("HousingObject", user.housingObject);
            param.Add("Friend", user.friend);
            param.Add("GuestBook", user.guestBook);
            param.Add("Mail", user.mail);

            Backend.GameData.Insert("User", param); // User 테이블에 데이터 삽입

            Debug.Log("새로운 유저 데이터 초기값 설정 완료");
        }
        else // 기존 유저인 경우
        {
            // 저장된 데이터를 불러와 user 클래스에 할당

            // [보유한 캐릭터]
            for (int i = 0; i < bro.FlattenRows()[0]["Character"].Count; i++)
            {
                user.character[i] = int.Parse(bro.FlattenRows()[0]["Character"][i].ToString());
            }

            // [보유한 재화]
            var keys = bro.FlattenRows()[0]["Goods"].Keys; // JsonData를 딕셔너리 키로 변환하는 과정

            string[] goodsArray = new string[keys.Count];
            keys.CopyTo(goodsArray, 0);

            for (var i = 0; i < keys.Count; i++)
            {
                var key = goodsArray[i];
                user.goods[key] = int.Parse(bro.FlattenRows()[0]["Goods"][key].ToString());
            }

            // [보유한 하우징 오브젝트]
            for (int i = 0; i < bro.FlattenRows()[0]["HousingObject"].Count; i++)
            {
                user.housingObject[i] = int.Parse(bro.FlattenRows()[0]["HousingObject"][i].ToString());
            }

            // [친구]
            for (int i = 0; i < bro.FlattenRows()[0]["Friend"].Count; i++)
            {
                user.friend[i] = bro.FlattenRows()[0]["Friend"][i].ToString();
            }

            // [방명록]
            for (int i = 0; i < bro.FlattenRows()[0]["GuestBook"].Count; i++)
            {
                user.guestBook[i] = int.Parse(bro.FlattenRows()[0]["GuestBook"][i].ToString());
            }

            // [우편]
            for (int i = 0; i < bro.FlattenRows()[0]["Mail"].Count; i++)
            {
                user.mail[i] = int.Parse(bro.FlattenRows()[0]["Mail"][i].ToString());
            }

            Debug.Log("기존 유저 데이터 불러오기 완료");
        }
    }
}
