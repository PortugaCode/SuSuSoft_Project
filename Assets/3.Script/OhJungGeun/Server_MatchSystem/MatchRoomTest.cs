using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;
using TMPro;
using Protocol;
using System;

public class MatchRoomTest : MonoBehaviour
{
    public static MatchRoomTest Instance = null; 

    [Header("User Info")]
    [SerializeField] private TextMeshProUGUI[] textMeshProList;
    [SerializeField] private Dictionary<SessionId, TouchMove> players = new Dictionary<SessionId, TouchMove>();
    [SerializeField] private List<TouchMove> playerList = new List<TouchMove>();

    [Header("User Prefeb")]
    [SerializeField] private TouchMove playerPrefab;
    [SerializeField] private GameObject chatBox;





    [SerializeField] private TMP_InputField textInput;



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
            SetRoomInfo();
        };

        Backend.Match.OnSessionOnline = (MatchInGameSessionEventArgs args) =>
        {
            SetRoomInfo();
        };

        SetRoomInfo();
    }

    public void LeaveIDObjectDestory(SessionId sessionId)
    {
        playerList.Remove(players[sessionId]);
        Destroy(players[sessionId].gameObject);
    }


    private void SetRoomInfo()
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
        foreach (SessionId a in BackEndManager.Instance.GetMatchSystem().userNickName.Keys)
        {
            GameObject cloneChatBox = GameObject.Instantiate(chatBox, chatBox.transform.position, Quaternion.Euler(0f,0f,-90f));


            players.Add(a, playerList[count]);
            players[a].gameObject.GetComponent<MatchChat>().SetChatBox(cloneChatBox);
            players[a].gameObject.GetComponent<MatchChat>().SetNickName(BackEndManager.Instance.GetMatchSystem().userNickName[a]);
            players[a].gameObject.GetComponent<MatchChat>().SetChatOrder(order1, order2);

            order1 += 2;
            order2 += 2;

            if (a == Backend.Match.GetMySessionId())
            {
                playerList[count].isHost = true;
                CameraController cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
                cameraController.SetPlayer(playerList[count].gameObject);
            }
            Debug.Log(a.ToString());
            count++;
        }



        count = 0;
        foreach (string a in BackEndManager.Instance.GetMatchSystem().userNickName.Values)
        {
            textMeshProList[count].text = a;
            count++;
        }
    }

    public void SendChat_InGame()
    {
        if (textInput.text.Length <= 1)
        {
            textInput.text = "";
            textInput.ActivateInputField();
            return;
        }

        players[Backend.Match.GetMySessionId()].GetComponent<MatchChat>().SetChatInGame(textInput.text);
        PlayerChatMessage msg = new PlayerChatMessage(Backend.Match.GetMySessionId(), textInput.text);
        BackEndManager.Instance.GetMatchSystem().SendDataToInGame<PlayerChatMessage>(msg);

        textInput.text = "";
        textInput.ActivateInputField();
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



        Vector3 movePosition = new Vector3(data.xPos, data.yPos, data.zPos);
        Vector3 moveDirection = new Vector3(data.xDir, data.yDir, data.zDir);
        players[data.playerSession].SetDirection(moveDirection);
        players[data.playerSession].SetPosition(movePosition);
        Debug.Log("상대방 움직임");
    }

    public void LeaveGameServer()
    {
        BackEndManager.Instance.GetMatchSystem().LeaveGameServer();
    }
}
