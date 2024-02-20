using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class FriendManager : MonoBehaviour
{
    [Header("Tab Button")]
    [SerializeField] private TMP_Text tabText;
    [SerializeField] private Button friendListTabButton;
    [SerializeField] private Button friendRequestTabButton;
    [SerializeField] private Button friendAddTabButton;

    [Header("Tab")]
    [SerializeField] private GameObject friendListTab;
    [SerializeField] private GameObject friendRequestTab;
    [SerializeField] private GameObject friendAddTab;

    [Header("Friend List")]
    [SerializeField] private Transform content_friendList;
    [SerializeField] private GameObject friendTabPrefab;

    [Header("Request List")]
    [SerializeField] private Transform content_requestList;
    [SerializeField] private GameObject requestTabPrefab;

    [Header("Add Friend")]
    [SerializeField] private TMP_InputField searchInputField;
    [SerializeField] private GameObject searchTab;
    [SerializeField] private Button sendRequestButton;

    [Header("Sprite")]
    [SerializeField] Sprite[] characterBodyImages; // 캐릭터 Body 이미지 배열
    [SerializeField] Sprite[] characterFaceImages; // 캐릭터 Face 이미지 배열

    private List<Friend> friends = new List<Friend>();
    private List<Friend> requests = new List<Friend>();

    private void Start()
    {
        // For Test
        /*var bro = Backend.Initialize(true);

        var login = Backend.BMember.CustomLogin("jinwon", "1234");

        if (login.IsSuccess())
        {
            GetFriendsData();
            ShowFriendsList();
        }*/
    }

    private void OnEnable()
    {
        //GetFriendsData();
        //ShowFriendsList();

        // + UI들 초기화 필요 (interactable 등)
    }

    public void GetFriendsData() // 친구 목록 조회
    {
        // 전체 친구 리스트 조회
        var bro = Backend.Friend.GetFriendList();

        // limit, offset 사용하여 친구 리스트 조회
        //Backend.Friend.GetFriendList(5); // 5명 친구 조회(1-5)
        //Backend.Friend.GetFriendList(10, 5); // 처음 5명 이후의 10명 친구 조회(6-15)

        LitJson.JsonData json = bro.FlattenRows();

        //User user = DBManager.instance.user; // -> 나중에 user 클래스에 할당 필요

        for (int i = 0; i < json.Count; i++)
        {
            Friend currentFriend = new Friend();

            currentFriend.name = json[i]["nickname"].ToString();
            currentFriend.inDate = json[i]["inDate"].ToString();
            currentFriend.lastLogin = json[i]["lastLogin"].ToString();
            currentFriend.createdAt = json[i]["createdAt"].ToString();

            //user.friend.Add(currentFriend);
            friends.Add(currentFriend);
        }
    }

    public void ShowFriendsList()
    {
        for (int i = 0; i < friends.Count; i++)
        {
            GameObject friendTab = Instantiate(friendTabPrefab);
            friendTab.transform.SetParent(content_friendList);

            friendTab.transform.GetChild(1).GetComponent<Image>().sprite = characterBodyImages[0]; // 해당 친구가 사용중인 캐릭터로 매칭 필요
            friendTab.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = characterFaceImages[0]; // 해당 친구가 사용중인 캐릭터로 매칭 필요
            friendTab.transform.GetChild(2).GetComponent<TMP_Text>().text = $"{friends[i].name}";

            int temp = i;
            friendTab.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { OpenWhisperTab(temp, friends[temp].inDate); });
            friendTab.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(delegate { SendStar(temp, friends[temp].inDate); });
            friendTab.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(delegate { DeleteFriend(temp, friends[temp].inDate); });
        }
    }

    public void GetRequestData() // 받은 친구 요청 목록 조회
    {
        requests = GetReceivedRequestList();
    }

    public void ShowRequestsList()
    {
        for (int i = 0; i < requests.Count; i++)
        {
            GameObject requestTab = Instantiate(requestTabPrefab);
            requestTab.transform.SetParent(content_requestList);

            requestTab.transform.GetChild(1).GetComponent<Image>().sprite = characterBodyImages[0]; // 해당 친구가 사용중인 캐릭터로 매칭 필요
            requestTab.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = characterFaceImages[0]; // 해당 친구가 사용중인 캐릭터로 매칭 필요
            requestTab.transform.GetChild(2).GetComponent<TMP_Text>().text = $"{friends[i].name}";

            int temp = i;
            requestTab.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { RejectRequest(temp, friends[temp].inDate); });
            requestTab.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(delegate { AcceptRequest(temp, friends[temp].inDate); });
        }
    }

    public void SendFriendsRequest(string nickname) // 친구 요청 전송
    {
        // 닉네임으로 gamer inDate 가져오기
        var n_bro = Backend.Social.GetUserInfoByNickName(nickname);
        string n_inDate = n_bro.GetReturnValuetoJSON()["row"]["inDate"].ToString();

        // gamer inDate 로 친구 요청 보내기
        var bro = Backend.Friend.RequestFriend(n_inDate);

        if (!bro.IsSuccess())
        {
            string message = string.Empty;

            switch (int.Parse(bro.GetStatusCode()))
            {
                case 403:
                    message = "뒤끝 콘솔 소셜관리 메뉴의 친구 최대보유수 설정값이 0입니다.";
                    break;
                case 409:
                    message = "이미 요청한 유저입니다.";
                    break;
                case 412:
                    message = bro.GetMessage().Contains("Send") ? "보낸 유저의 요청이 가득 찼습니다." : "받는 유저의 요청이 가득 찼습니다.";
                    break;
                default:
                    message = bro.GetMessage();
                    break;
            }
            Debug.Log(message);
        }
    }

    public List<Friend> GetSentFriendRequestList() // 보낸 친구요청 목록 조회 (수락, 거절 시 리스트에서 제거됨)
    {
        List<Friend> list = new List<Friend>();

        // 친구 요청을 보낸 리스트 전체 조회
        var bro = Backend.Friend.GetSentRequestList();
        // limit, offset 사용하여 친구 요청을 보낸 리스트 조회
        //Backend.Friend.GetSentRequestList(5); // 친구 요청을 보낸 5명 리스트 조회(1-5)
        //Backend.Friend.GetSentRequestList(5, 5); // 친구 요청을 보낸 처음 5명 이후의 5명 리스트 조회(6-10)

        LitJson.JsonData json = bro.FlattenRows();

        for (int i = 0; i < json.Count; i++)
        {
            Friend currentFriend = new Friend();

            currentFriend.name = json[i]["nickname"].ToString();
            currentFriend.inDate = json[i]["inDate"].ToString();
            currentFriend.lastLogin = json[i]["lastLogin"].ToString();
            currentFriend.createdAt = json[i]["createdAt"].ToString();

            list.Add(currentFriend);
        }

        return list;
    }

    public List<Friend> GetReceivedRequestList() // 받은 친구요청 목록 조회 (수락, 거절 시 리스트에서 제거됨)
    {
        List<Friend> list = new List<Friend>();

        // 친구 요청을 받은 리스트 전체 조회
        var bro = Backend.Friend.GetReceivedRequestList();
        // limit, offset 사용하여 친구 요청을 받은 리스트 조회
        //Backend.Friend.GetReceivedRequestList(5); // 친구 요청을 받은 5명 리스트 조회(1-5)
        //Backend.Friend.GetReceivedRequestList(5, 5); // 친구 요청을 받은 처음 5명 이후의 5명 리스트 조회(6-10)

        LitJson.JsonData json = bro.FlattenRows();

        for (int i = 0; i < json.Count; i++)
        {
            Friend currentFriend = new Friend();

            currentFriend.name = json[i]["nickname"].ToString();
            currentFriend.inDate = json[i]["inDate"].ToString();
            currentFriend.lastLogin = json[i]["lastLogin"].ToString();
            currentFriend.createdAt = json[i]["createdAt"].ToString();

            list.Add(currentFriend);
        }

        return list;
    }

    public void AcceptFriendRequest(string n_inDate) // 친구 요청 수락
    {
        // 닉네임으로 gamer inDate 가져오기
        //var n_bro = Backend.Social.GetUserInfoByNickName(nickname);
        //string n_inDate = n_bro.GetReturnValuetoJSON()["row"]["inDate"].ToString();

        // gamer inDate 로 친구 요청 수락
        var bro = Backend.Friend.AcceptFriend(n_inDate);

        if (!bro.IsSuccess())
        {
            string message = string.Empty;

            switch (int.Parse(bro.GetStatusCode()))
            {
                case 412:
                    message = bro.GetMessage().Contains("Requested") ? "요청한 유저의 친구 목록이 가득 찼습니다." : "친구 목록이 가득 찼습니다.";
                    break;
                default:
                    message = bro.GetMessage();
                    break;
            }
            Debug.Log(message);
        }
    }

    public void RejectFriendRequest(string n_inDate) // 친구 요청 거절
    {
        // 닉네임으로 gamer inDate 가져오기
        //var n_bro = Backend.Social.GetUserInfoByNickName(nickname);
        //string n_inDate = n_bro.GetReturnValuetoJSON()["row"]["inDate"].ToString();

        // gamer inDate 로 친구 요청 거절
        var bro = Backend.Friend.RejectFriend(n_inDate);
    }

    public void DeleteFriend(string n_inDate) // 친구 삭제
    {
        // 닉네임으로 gamer inDate 가져오기
        //var n_bro = Backend.Social.GetUserInfoByNickName(nickname);
        //string n_inDate = n_bro.GetReturnValuetoJSON()["row"]["inDate"].ToString();

        // gamer inDate 로 친구 삭제
        var bro = Backend.Friend.BreakFriend(n_inDate);

        if (!bro.IsSuccess())
        {
            string message = string.Empty;

            switch (int.Parse(bro.GetStatusCode()))
            {
                case 404:
                    message = "gamerIndate가 올바르지 않거나 해당 유저와 친구가 아닙니다.";
                    break;
                default:
                    message = bro.GetMessage();
                    break;
            }
            Debug.Log(message);
        }
    }

    public void OpenWhisperTab(int index, string inDate)
    {
        // 해당 유저와 귓속말 탭 구현 필요
    }

    public void SendStar(int index, string inDate)
    {
        // 해당 유저에게 별 전송하기 구현 필요

        content_friendList.GetChild(index).GetChild(4).GetChild(1).GetComponent<TMP_Text>().text = $"전송완료";
    }

    public void DeleteFriend(int index, string inDate) // 해당 친구 삭제
    {
        friends.RemoveAt(index);
        Destroy(content_friendList.GetChild(index).gameObject);
        DeleteFriend(inDate);
    }

    public void RejectRequest(int index, string inDate) // 친구 요청 거절
    {
        requests.RemoveAt(index);
        Destroy(content_requestList.GetChild(index).gameObject);
        RejectFriendRequest(inDate);
    }

    public void AcceptRequest(int index, string inDate) // 친구 요청 수락
    {
        requests.RemoveAt(index);
        Destroy(content_requestList.GetChild(index).gameObject);
        AcceptFriendRequest(inDate);
    }

    public void SearchUser()
    {
        searchTab.SetActive(true);

        var n_bro = Backend.Social.GetUserInfoByNickName(searchInputField.text);
        string n_inDate = n_bro.GetReturnValuetoJSON()["row"]["inDate"].ToString();

        // 해당 닉네임을 사용중인 유저 존재 시에만 표시!!

        // 닉네임 & InDate로 유저 조회 후 표시 구현 필요
        searchTab.transform.GetChild(1).GetComponent<Image>().sprite = characterBodyImages[0]; // 해당 친구가 사용중인 캐릭터로 매칭 필요
        searchTab.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = characterFaceImages[0]; // 해당 친구가 사용중인 캐릭터로 매칭 필요
        searchTab.transform.GetChild(2).GetComponent<TMP_Text>().text = $"해당친구닉네임";
    }

    public void SendAddRequest()
    {
        sendRequestButton.interactable = false;
        sendRequestButton.gameObject.GetComponent<TMP_Text>().text = "전송 완료";

        SendFriendsRequest(searchInputField.text); // 또는 searchTab.transform.GetChild(2).GetComponent<TMP_Text>().text;
    }

    public void OpenFriendListTab()
    {
        tabText.text = $"친구 목록";
        friendListTabButton.interactable = false;
        friendRequestTabButton.interactable = true;
        friendAddTabButton.interactable = true;

        friendListTab.SetActive(true);
        friendRequestTab.SetActive(false);
        friendAddTab.SetActive(false);

        GetFriendsData();
        ShowFriendsList();
    }

    public void OpenFriendRequestTab()
    {
        tabText.text = $"친구 요청";
        friendListTabButton.interactable = true;
        friendRequestTabButton.interactable = false;
        friendAddTabButton.interactable = true;

        friendListTab.SetActive(false);
        friendRequestTab.SetActive(true);
        friendAddTab.SetActive(false);

        GetRequestData();
        ShowRequestsList();
    }

    public void OpenFriendAddTab()
    {
        tabText.text = $"친구 추가";
        friendListTabButton.interactable = true;
        friendRequestTabButton.interactable = true;
        friendAddTabButton.interactable = false;

        friendListTab.SetActive(false);
        friendRequestTab.SetActive(false);
        friendAddTab.SetActive(true);
    }

    public void CloseFriendPopup()
    {
        OpenFriendListTab();
        gameObject.SetActive(false);
    }
}
