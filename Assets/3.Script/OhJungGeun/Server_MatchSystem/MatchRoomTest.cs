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


    [SerializeField] private TextMeshProUGUI[] textMeshProList;
    [SerializeField] private TouchMove playerPrefab;
    [SerializeField] private List<TouchMove> playerList = new List<TouchMove>();

    [SerializeField] private Dictionary<SessionId, TouchMove> players = new Dictionary<SessionId, TouchMove>();


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
            playerList.Add(TouchMove.Instantiate(playerPrefab, new Vector3(2,2,0), Quaternion.identity));
        }
        Debug.Log(playerList.Count);


        Backend.Match.OnSessionOffline += (MatchInGameSessionEventArgs args) =>
        {
            SetText();
        };

        Backend.Match.OnSessionOnline = (MatchInGameSessionEventArgs args) =>
        {
            SetText();
        };

        SetText();
    }


    private void SetText()
    {
        Debug.Log("SetText");
        for(int i = 0; i < textMeshProList.Length; i++)
        {
            textMeshProList[i].text = "AI";
        }

        players.Clear();
        int count = 0;
        foreach (SessionId a in BackEndManager.Instance.GetMatchSystem().userNickName.Keys)
        {
            players.Add(a, playerList[count]);
            if(a == Backend.Match.GetMySessionId())
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
