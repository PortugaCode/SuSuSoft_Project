using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using BackEnd.Tcp;
using TMPro;
using Protocol;
using System;

public class MatchRoomTest : MonoBehaviour
{
    public static MatchRoomTest Instance = null;

    [Header("UI Goods")]
    [SerializeField] private TextMeshProUGUI friendshipPointText;
    [SerializeField] private TextMeshProUGUI rubyText;
    [SerializeField] private TextMeshProUGUI goldText_1;

    [Header("Body & Face Sprite")]
    public Sprite[] bodys;
    public Sprite[] faces;


    [Header("User Info")]
    [SerializeField] private TextMeshProUGUI[] textMeshProList;
    [SerializeField] private Dictionary<SessionId, TouchMove> players = new Dictionary<SessionId, TouchMove>();
    [SerializeField] private List<TouchMove> playerList = new List<TouchMove>();

    [Header("User Prefab")]
    [SerializeField] private TouchMove playerPrefab;
    [SerializeField] private GameObject chatBox;
    [SerializeField] private GameObject playerNickName;

    [Header("User Clone Prefab")]
    [SerializeField] private MatchClonePlayer matchClonePlayerPrefab;

    [Header("ChatInput")]
    [SerializeField] private TMP_InputField textInput;



    [Header("MasterInfo")]
    [SerializeField] private TextMeshProUGUI master_NickName;
    [SerializeField] private Image master_Body;
    [SerializeField] private Image master_Face;

    [Header("Master Housing")]
    [SerializeField] private Transform buildSpace;
    [SerializeField] private HousingDrag housingGameObj;
    [SerializeField] private GameObject thisBuilding;

    



    private void Awake()
    {
        #region [싱글톤]
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
        #endregion
    }

    private void Start()
    {
        for(int i =0; i < BackEndManager.Instance.GetMatchSystem().userNickName.Count; i++)
        {
            playerList.Add(TouchMove.Instantiate(playerPrefab, new Vector3(0f, -2f, 0f), Quaternion.identity));
        }
        Debug.Log(playerList.Count);


        Backend.Match.OnSessionOffline += (MatchInGameSessionEventArgs args) =>
        {
            SetRoomInfo(false);
        };

        Backend.Match.OnSessionOnline = (MatchInGameSessionEventArgs args) =>
        {
            SetRoomInfo(true);
        };

        SetRoomInfo(true);
        SetMasterInfo();
        UpdateGoods();
        SetHousing();
    }

    public void UpdateGoods()
    {
        // Friendship Point
        if (DBManager.instance.user.goods["friendshipPoint"] >= 10000000)
        {
            friendshipPointText.text = String.Format("{0:0,0}M", DBManager.instance.user.goods["friendshipPoint"] / 1000000);
        }
        else if (DBManager.instance.user.goods["friendshipPoint"] >= 10000)
        {
            friendshipPointText.text = String.Format("{0:0,0}K", DBManager.instance.user.goods["friendshipPoint"] / 1000);
        }
        else
        {
            friendshipPointText.text = DBManager.instance.user.goods["friendshipPoint"].ToString();
        }

        // Ruby
        if (DBManager.instance.user.goods["ruby"] >= 10000000)
        {
            rubyText.text = String.Format("{0:0,0}M", DBManager.instance.user.goods["ruby"] / 1000000);
        }
        else if (DBManager.instance.user.goods["ruby"] >= 10000)
        {
            rubyText.text = String.Format("{0:0,0}K", DBManager.instance.user.goods["ruby"] / 1000);
        }
        else
        {
            rubyText.text = DBManager.instance.user.goods["ruby"].ToString();
        }

        // Gold
        if (DBManager.instance.user.goods["gold"] >= 10000000)
        {
            goldText_1.text = String.Format("{0:0,0}M", DBManager.instance.user.goods["gold"] / 1000000);
        }
        else if (DBManager.instance.user.goods["gold"] >= 10000)
        {
            goldText_1.text = String.Format("{0:0,0}K", DBManager.instance.user.goods["gold"] / 1000);
        }
        else
        {
            goldText_1.text = DBManager.instance.user.goods["gold"].ToString();
        }
    }



    private void SetMasterInfo()
    {
        //마스터_인포 세팅하기
        var n_bro = Backend.Social.GetUserInfoByNickName(BackEndManager.Instance.GetMatchSystem().masterUser_NickName);
        string n_inDate = n_bro.GetReturnValuetoJSON()["row"]["inDate"].ToString();

        var bro = Backend.PlayerData.GetOtherData("User", n_inDate);
        int index = int.Parse(bro.GetReturnValuetoJSON()["rows"][0]["CurrentCharacterIndex"][0].ToString());
        master_Body.sprite = bodys[index];
        master_Face.sprite = faces[(int)(index / 4)];


        master_NickName.text = $"{BackEndManager.Instance.GetMatchSystem().masterUser_NickName}";
    }

    private void SetHousing()
    {
        //마스터 DB의 하우징 데이터 가지고 오기
        var n_bro = Backend.Social.GetUserInfoByNickName(BackEndManager.Instance.GetMatchSystem().masterUser_NickName);
        string n_inDate = n_bro.GetReturnValuetoJSON()["row"]["inDate"].ToString();

        var bro = Backend.PlayerData.GetOtherData("Housing", n_inDate);

        for (int i = 0; i < bro.GetReturnValuetoJSON()["rows"].Count; i++)
        {
            int index = int.Parse(bro.GetReturnValuetoJSON()["rows"][i]["Index"][0].ToString());
            float x = float.Parse(bro.GetReturnValuetoJSON()["rows"][i]["X"][0].ToString());
            float y = float.Parse(bro.GetReturnValuetoJSON()["rows"][i]["Y"][0].ToString());
            Debug.Log($"{index} : ({x}, {y})");
            HousingDrag cloneBuild = Instantiate(housingGameObj, new Vector3(x, y, 0), Quaternion.identity, buildSpace);
            cloneBuild.housingObject = ChartManager.instance.housingObjectDatas[index];
            cloneBuild.id = index;
            cloneBuild.buildSprite.sprite = SpriteManager.instance.sprites[index];
        }
    }


    public void LeaveIDObjectDestory(SessionId sessionId)
    {
        playerList.Remove(players[sessionId]);
        Destroy(players[sessionId].gameObject);
    }


    private void SetRoomInfo(bool isFirst)
    {
        Debug.Log("SetText");
        for(int i = 0; i < textMeshProList.Length; i++)
        {
            textMeshProList[i].text = "AI";
        }

        players.Clear();
        int count = 0;

        int order1 = 100;
        int order2 = 101;

        bool isMyteam = true;
        foreach (SessionId a in BackEndManager.Instance.GetMatchSystem().userNickName.Keys)
        {
            players.Add(a, playerList[count]);
            if (isFirst)
            {
                #region [메인 캐릭터 생성]
                GameObject cloneChatBox = GameObject.Instantiate(chatBox, chatBox.transform.position, Quaternion.Euler(0f, 0f, -90f));
                GameObject cloneNickName = GameObject.Instantiate(playerNickName, playerNickName.transform.position, Quaternion.identity);

                players[a].gameObject.GetComponent<MatchChat>().SetChatBox(cloneChatBox);
                players[a].gameObject.GetComponent<MatchChat>().SetChatOrder(order1, order2);
                players[a].gameObject.GetComponent<MatchChat>().SetNickName(cloneNickName.GetComponent<TextMeshPro>());
                players[a].gameObject.GetComponent<MatchChat>().SetNickName(BackEndManager.Instance.GetMatchSystem().userNickName[a]);
                players[a].gameObject.GetComponent<CurrentPlayerSprite>().SetUserCharacter(BackEndManager.Instance.GetMatchSystem().userNickName[a]);
                #endregion

                #region [클론 캐릭터 생성_Left Clone]

                Vector2 clonePosition = new Vector2(players[a].gameObject.transform.position.x - 25.6f, players[a].gameObject.transform.position.y);

                MatchClonePlayer matchClonePlayer_Left = MatchClonePlayer.Instantiate(matchClonePlayerPrefab, clonePosition, Quaternion.identity);
                matchClonePlayer_Left.SetTargetPlayer(players[a].gameObject.transform, false);

                GameObject cloneChatBox_Left = GameObject.Instantiate(chatBox, chatBox.transform.position, Quaternion.Euler(0f, 0f, -90f));
                GameObject cloneNickName_Left = GameObject.Instantiate(playerNickName, playerNickName.transform.position, Quaternion.identity);

                matchClonePlayer_Left.gameObject.GetComponent<MatchChat>().SetChatBox(cloneChatBox_Left);
                matchClonePlayer_Left.gameObject.GetComponent<MatchChat>().SetChatOrder(order1, order2);
                matchClonePlayer_Left.gameObject.GetComponent<MatchChat>().SetNickName(cloneNickName_Left.GetComponent<TextMeshPro>());
                matchClonePlayer_Left.gameObject.GetComponent<MatchChat>().SetNickName(BackEndManager.Instance.GetMatchSystem().userNickName[a]);
                matchClonePlayer_Left.gameObject.GetComponent<CurrentPlayerSprite>().SetUserCharacter(BackEndManager.Instance.GetMatchSystem().userNickName[a]);
                players[a].gameObject.GetComponent<InteractionControl>().doCloneAnimation = matchClonePlayer_Left.gameObject.GetComponent<InteractionControl>().PlayAnimation;
                #endregion

                #region [클론 캐릭터 생성_Right Clone]

                clonePosition = new Vector2(players[a].gameObject.transform.position.x + 25.6f, players[a].gameObject.transform.position.y);

                MatchClonePlayer matchClonePlayer_Right = MatchClonePlayer.Instantiate(matchClonePlayerPrefab, clonePosition, Quaternion.identity);
                matchClonePlayer_Right.SetTargetPlayer(players[a].gameObject.transform, true);

                GameObject cloneChatBox_Right = GameObject.Instantiate(chatBox, chatBox.transform.position, Quaternion.Euler(0f, 0f, -90f));
                GameObject cloneNickName_Right = GameObject.Instantiate(playerNickName, playerNickName.transform.position, Quaternion.identity);

                matchClonePlayer_Right.gameObject.GetComponent<MatchChat>().SetChatBox(cloneChatBox_Right);
                matchClonePlayer_Right.gameObject.GetComponent<MatchChat>().SetChatOrder(order1, order2);
                matchClonePlayer_Right.gameObject.GetComponent<MatchChat>().SetNickName(cloneNickName_Right.GetComponent<TextMeshPro>());
                matchClonePlayer_Right.gameObject.GetComponent<MatchChat>().SetNickName(BackEndManager.Instance.GetMatchSystem().userNickName[a]);
                matchClonePlayer_Right.gameObject.GetComponent<CurrentPlayerSprite>().SetUserCharacter(BackEndManager.Instance.GetMatchSystem().userNickName[a]);
                players[a].gameObject.GetComponent<InteractionControl>().doCloneAnimation = matchClonePlayer_Right.gameObject.GetComponent<InteractionControl>().PlayAnimation;
                #endregion



                if (BackEndManager.Instance.GetMatchSystem().userTeam[Backend.UserNickName] != BackEndManager.Instance.GetMatchSystem().userTeam[BackEndManager.Instance.GetMatchSystem().userNickName[a]])
                {
                    players[a].gameObject.SetActive(false);
                    matchClonePlayer_Left.gameObject.SetActive(false);
                    matchClonePlayer_Right.gameObject.SetActive(false);

                    cloneNickName.SetActive(false);
                    cloneNickName_Left.SetActive(false);
                    cloneNickName_Right.SetActive(false);

                    isMyteam = false;
                }

                order1 += 2;
                order2 += 2;
            }


            if (a == Backend.Match.GetMySessionId())
            {
                playerList[count].isHost = true;
                TouchMove cameraController = playerList[count].GetComponent<TouchMove>();
                cameraController.SetPlayer(playerList[count].gameObject);
            }

            Debug.Log(a.ToString());



            count++;
        }



        count = 0;
        foreach (string a in BackEndManager.Instance.GetMatchSystem().userNickName.Values)
        {
            textMeshProList[count].text = a;
            if (!isMyteam) textMeshProList[count].gameObject.SetActive(false);
            count++;
            
        }
    }

    public void SendChat_InGame()
    {
        if (textInput.text.Length <= 0)
        {
            textInput.text = "";
            return;
        }

        players[Backend.Match.GetMySessionId()].GetComponent<MatchChat>().SetChatInGame(textInput.text);
        PlayerChatMessage msg = new PlayerChatMessage(Backend.Match.GetMySessionId(), textInput.text);
        BackEndManager.Instance.GetMatchSystem().SendDataToInGame<PlayerChatMessage>(msg);

        textInput.text = "";
    }


    public void ProcessPlayerData(PlayerChatMessage data)
    {
        Debug.Log(data.playerSession.ToString());
        if (data.playerSession == Backend.Match.GetMySessionId())
        {
            Debug.Log("내가 입력함");
            return;
        }

        Debug.Log("상대방 채팅 입력함 : " + data.chat);
        players[data.playerSession].GetComponent<MatchChat>().SetChatInGame(data.chat);
    }


    public void ProcessPlayerData(PlayerMoveMessage data)
    {
        Debug.Log(data.playerSession.ToString());
        if(data.playerSession == Backend.Match.GetMySessionId())
        {
            Debug.Log("내가 움직임");
            return;
        }

        //첫 번째 방법 => position과 move position이 너무 멀면 순간이동 시키기
        //두 번째 방법 isRight bool값 지정해주기

        Vector3 movePosition = new Vector3(data.xPos, data.yPos, data.zPos);
        Vector3 moveDirection = new Vector3(data.xDir, data.yDir, data.zDir);
        players[data.playerSession].SetDirection(moveDirection);
        players[data.playerSession].SetPosition(movePosition);
        players[data.playerSession].SetIsRight();
        Debug.Log("상대방 움직임");
    }

    public void ProcessPlayerData(PlayerAnimationMessage data)
    {
        Debug.Log(data.playerSession.ToString());
        if (data.playerSession == Backend.Match.GetMySessionId())
        {
            Debug.Log("내가 애니메이션");
            return;
        }


        int index = data.index;
        players[data.playerSession].PlayAnimation_Index(index);
        Debug.Log("상대방 애니메이션");
    }

    public void LeaveGameServer()
    {
        BackEndManager.Instance.GetMatchSystem().LeaveGameServer();
    }
}
