using BackEnd;
using BackEnd.Tcp;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MatchSystem
{
    private bool isCanInviteUser;

    public bool IsCanInviteUser => isCanInviteUser;

    [Header("Invited Data")]
    private SessionId roomId;
    private string roomToken;
    private MatchMakingUserInfo inviteUserInfo;


    public MatchMakingUserInfo InviteUserInfo => inviteUserInfo;
    public SessionId RoomID => roomId;
    public string RoomToken => roomToken;

    //�ʴ� ��û �� ���� �ð�
    float timer = 50.0f;
    bool isTimerOn = false;


    //���ӷ� ����
    public MatchInGameRoomInfo roomInfo; // ������ ���� ����
    public List<string> gameRecords = new List<string>(); // ���� ���ӹ濡 ������ �ִ� �������� ����


    //��Ī ���� ����Ʈ �ε��� ����
    public MatchCard GetMatchList(int index)
    {
        var callback = Backend.Match.GetMatchList();

        if (!callback.IsSuccess())
        {
            Debug.LogError("Backend.Match.GetMatchList Error : " + callback);
            return null;
        }

        List<MatchCard> matchCardList = new List<MatchCard>();

        LitJson.JsonData matchCardListJson = callback.FlattenRows();



        for (int i = 0; i < matchCardListJson.Count; i++)
        {
            MatchCard matchCard = new MatchCard();

            matchCard.inDate = matchCardListJson[i]["inDate"].ToString();
            matchCard.result_processing_type = matchCardListJson[i]["result_processing_type"].ToString();
            matchCard.version = int.Parse(matchCardListJson[i]["version"].ToString());
            matchCard.matchTitle = matchCardListJson[i]["matchTitle"].ToString();
            matchCard.enable_sandbox = matchCardListJson[i]["enable_sandbox"].ToString() == "true" ? true : false;
            matchCard.matchType = matchCardListJson[i]["matchType"].ToString();
            matchCard.matchModeType = matchCardListJson[i]["matchModeType"].ToString();

            foreach (LitJson.JsonData row in callback.Rows())
            {

                foreach (MatchType type in Enum.GetValues(typeof(MatchType)))
                {
                    if (type.ToString().ToLower().Equals(row["matchType"]["S"].ToString().ToLower()))
                    {
                        matchCard.matchTypeEnum = type;
                    }
                }

                foreach (MatchModeType type in Enum.GetValues(typeof(MatchModeType)))
                {
                    if (type.ToString().ToLower().Equals(row["matchModeType"]["S"].ToString().ToLower()))
                    {
                        matchCard.matchModeTypeEnum = type;
                    }
                }
            }

            matchCard.matchHeadCount = int.Parse(matchCardListJson[i]["matchHeadCount"].ToString());
            matchCard.enable_battle_royale = matchCardListJson[i]["enable_battle_royale"].ToString() == "true" ? true : false;
            matchCard.match_timeout_m = int.Parse(matchCardListJson[i]["match_timeout_m"].ToString());
            matchCard.transit_to_sandbox_timeout_ms = int.Parse(matchCardListJson[i]["transit_to_sandbox_timeout_ms"].ToString());
            matchCard.match_start_waiting_time_s = int.Parse(matchCardListJson[i]["match_start_waiting_time_s"].ToString());

            if (matchCardListJson[i].ContainsKey("match_increment_time_s"))
            {
                matchCard.match_increment_time_s = int.Parse(matchCardListJson[i]["match_increment_time_s"].ToString());
            }
            if (matchCardListJson[i].ContainsKey("maxMatchRange"))
            {
                matchCard.maxMatchRange = int.Parse(matchCardListJson[i]["maxMatchRange"].ToString());
            }
            if (matchCardListJson[i].ContainsKey("increaseAndDecrease"))
            {
                matchCard.increaseAndDecrease = int.Parse(matchCardListJson[i]["increaseAndDecrease"].ToString());
            }
            if (matchCardListJson[i].ContainsKey("initializeCycle"))
            {
                matchCard.initializeCycle = matchCardListJson[i]["initializeCycle"].ToString();
            }
            if (matchCardListJson[i].ContainsKey("defaultPoint"))
            {
                matchCard.defaultPoint = int.Parse(matchCardListJson[i]["defaultPoint"].ToString());
            }

            if (matchCardListJson[i].ContainsKey("savingPoint"))
            {
                if (matchCardListJson[i]["savingPoint"].IsArray)
                {
                    for (int listNum = 0; listNum < matchCardListJson[i]["savingPoint"].Count; listNum++)
                    {
                        var keyList = matchCardListJson[i]["savingPoint"][listNum].Keys;
                        foreach (var key in keyList)
                        {
                            matchCard.savingPoint.Add(key, int.Parse(matchCardListJson[i]["savingPoint"][listNum][key].ToString()));
                        }
                    }
                }
                else
                {
                    foreach (var key in matchCardListJson[i]["savingPoint"].Keys)
                    {
                        matchCard.savingPoint.Add(key, int.Parse(matchCardListJson[i]["savingPoint"][key].ToString()));
                    }
                }
            }
            matchCardList.Add(matchCard);
        }

        return matchCardList[index];
    }


    //�츮 ��Ī ���� ����Ʈ ��ȸ
    public void GetMatchList()
    {
        var callback = Backend.Match.GetMatchList();

        if (!callback.IsSuccess())
        {
            Debug.LogError("Backend.Match.GetMatchList Error : " + callback);
            return;
        }

        List<MatchCard> matchCardList = new List<MatchCard>();

        LitJson.JsonData matchCardListJson = callback.FlattenRows();

        Debug.Log("Backend.Match.GetMatchList : " + callback);

        for (int i = 0; i < matchCardListJson.Count; i++)
        {
            MatchCard matchCard = new MatchCard();

            matchCard.inDate = matchCardListJson[i]["inDate"].ToString();
            matchCard.result_processing_type = matchCardListJson[i]["result_processing_type"].ToString();
            matchCard.version = int.Parse(matchCardListJson[i]["version"].ToString());
            matchCard.matchTitle = matchCardListJson[i]["matchTitle"].ToString();
            matchCard.enable_sandbox = matchCardListJson[i]["enable_sandbox"].ToString() == "true" ? true : false;


            matchCard.matchType = matchCardListJson[i]["matchType"].ToString();
            matchCard.matchModeType = matchCardListJson[i]["matchModeType"].ToString();

            foreach (LitJson.JsonData row in callback.Rows())
            {

                foreach (MatchType type in Enum.GetValues(typeof(MatchType)))
                {
                    if (type.ToString().ToLower().Equals(row["matchType"]["S"].ToString().ToLower()))
                    {
                        matchCard.matchTypeEnum = type;
                    }
                }

                foreach (MatchModeType type in Enum.GetValues(typeof(MatchModeType)))
                {
                    if (type.ToString().ToLower().Equals(row["matchModeType"]["S"].ToString().ToLower()))
                    {
                        matchCard.matchModeTypeEnum = type;
                    }
                }
            }

            matchCard.matchHeadCount = int.Parse(matchCardListJson[i]["matchHeadCount"].ToString());
            matchCard.enable_battle_royale = matchCardListJson[i]["enable_battle_royale"].ToString() == "true" ? true : false;
            matchCard.match_timeout_m = int.Parse(matchCardListJson[i]["match_timeout_m"].ToString());
            matchCard.transit_to_sandbox_timeout_ms = int.Parse(matchCardListJson[i]["transit_to_sandbox_timeout_ms"].ToString());
            matchCard.match_start_waiting_time_s = int.Parse(matchCardListJson[i]["match_start_waiting_time_s"].ToString());

            if (matchCardListJson[i].ContainsKey("match_increment_time_s"))
            {
                matchCard.match_increment_time_s = int.Parse(matchCardListJson[i]["match_increment_time_s"].ToString());
            }
            if (matchCardListJson[i].ContainsKey("maxMatchRange"))
            {
                matchCard.maxMatchRange = int.Parse(matchCardListJson[i]["maxMatchRange"].ToString());
            }
            if (matchCardListJson[i].ContainsKey("increaseAndDecrease"))
            {
                matchCard.increaseAndDecrease = int.Parse(matchCardListJson[i]["increaseAndDecrease"].ToString());
            }
            if (matchCardListJson[i].ContainsKey("initializeCycle"))
            {
                matchCard.initializeCycle = matchCardListJson[i]["initializeCycle"].ToString();
            }
            if (matchCardListJson[i].ContainsKey("defaultPoint"))
            {
                matchCard.defaultPoint = int.Parse(matchCardListJson[i]["defaultPoint"].ToString());
            }

            if (matchCardListJson[i].ContainsKey("savingPoint"))
            {
                if (matchCardListJson[i]["savingPoint"].IsArray)
                {
                    for (int listNum = 0; listNum < matchCardListJson[i]["savingPoint"].Count; listNum++)
                    {
                        var keyList = matchCardListJson[i]["savingPoint"][listNum].Keys;
                        foreach (var key in keyList)
                        {
                            matchCard.savingPoint.Add(key, int.Parse(matchCardListJson[i]["savingPoint"][listNum][key].ToString()));
                        }
                    }
                }
                else
                {
                    foreach (var key in matchCardListJson[i]["savingPoint"].Keys)
                    {
                        matchCard.savingPoint.Add(key, int.Parse(matchCardListJson[i]["savingPoint"][key].ToString()));
                    }
                }
            }
            matchCardList.Add(matchCard);
        }

        foreach (var matchCard in matchCardList)
        {
            Debug.Log(matchCard.ToString());
        }
    }


    //��Ī ������ ��������  üũ
    public void JoinMatchMakingCheck()
    {
        ErrorInfo errorInfo;
        Backend.Match.JoinMatchMakingServer(out errorInfo);
        Debug.Log(errorInfo);
    }

    //��ġ ����ŷ ������ �����û
    public void JoinMatchMaking()
    {


        ErrorInfo errorInfo;


        Backend.Match.JoinMatchMakingServer(out errorInfo);
        Backend.Match.OnJoinMatchMakingServer = (JoinChannelEventArgs args) =>
        {
            
            Debug.Log("JoinMatchMaking : " + args.ErrInfo);

            //CreateMatchRoom();
            //RequestMatchMaking(0);
        };
    }

    //��Ī ������ ������� �� ȣ���� ���� ���� �޼���
    public void CreateMatchRoom(string nickName)
    {
        if (timer <= 9.9f)
        {
            Debug.Log("���� �ʴ� ���Դϴ�.");
            return;
        }
        Backend.Match.CreateMatchRoom();
        Backend.Match.OnMatchMakingRoomCreate = (MatchMakingInteractionEventArgs args) =>
        {
            Debug.Log("CreateMatchRoom : " + args.ErrInfo);

            if(args.ErrInfo == ErrorCode.Success)
            {
                isCanInviteUser = true;
                InviteUser(nickName);
            }
            Debug.Log(isCanInviteUser);
        };
    }

    //�ش� ���� �ʴ�
    public void InviteUser(string nickName)
    {

        Backend.Match.InviteUser(nickName);
        Backend.Match.OnMatchMakingRoomInvite = (MatchMakingInteractionEventArgs args) => 
        {
            Debug.Log("InviteUser : " + args.ErrInfo);
            if (args.ErrInfo != ErrorCode.Success) LeaveMatchRoom();
            else
            {
                isTimerOn = true;
            }
        };
    }

    //�ʴ� ���� �̺�Ʈ
    public void OnMatchMakingRoomSomeoneInvited(Action Todo)
    {
        Backend.Match.OnMatchMakingRoomSomeoneInvited += (MatchMakingInvitedRoomEventArgs args) => 
        {
            if(args.ErrInfo == ErrorCode.Success)
            {
                roomId = args.RoomId;
                roomToken = args.RoomToken;
                inviteUserInfo = args.InviteUserInfo;
                Todo();
            }
        };
    }

    //�ʴ� ��û�� ������ �� �ƴٸ� �ð� �帣��
    public void SetTimer()
    {
        if(isTimerOn)
        {
            Debug.Log(timer);
            this.timer -= Time.deltaTime;
            if (timer <= 0)
            {
                LeaveMatchRoom();
            }
        }
    }

    //�ʴ� ���� Or ���� �̺�Ʈ
    public void AreYouAccept(bool isAccept)
    {
        if (isAccept)
        {
            Backend.Match.AcceptInvitation(roomId, roomToken);
            Backend.Match.OnMatchMakingRoomUserList = (MatchMakingGamerInfoListInRoomEventArgs args) => 
            {
                Debug.Log("��� �� �ʴ� ���� ����");
            };
        }
        else
        {
            Backend.Match.DeclineInvitation(roomId, roomToken);
            {
                Debug.Log("���� �ʴ� ����");
            }
        }

        Backend.Match.OnMatchMakingRoomInviteResponse = (MatchMakingInteractionEventArgs args) => 
        {
            // TODO
            Debug.Log(args.ErrInfo);
        };
    }

    //���� ���� �̺�Ʈ
    public void OnMatchMakingRoomJoin()
    {
        Backend.Match.OnMatchMakingRoomJoin = (MatchMakingGamerInfoInRoomEventArgs args) => 
        {
            Debug.Log("���� ����");
            RequestMatchMaking(0);
        };
    }



    //���� ����
    public void LeaveMatchRoom()
    {
        isTimerOn = false;
        timer = 10f;
        Backend.Match.LeaveMatchRoom();
        //Backend.Match.LeaveMatchMakingServer();
    }













    //=========���� ��Ī ��û + �ΰ��� ���� ================//

    //�ش� �ε����� ��Ī ������ ���� ��Ī ��û
    public void RequestMatchMaking(int index)
    {
        Backend.Match.RequestMatchMaking(GetMatchList(index).matchTypeEnum, GetMatchList(index).matchModeTypeEnum, GetMatchList(index).inDate);
        Backend.Match.OnMatchMakingResponse = (MatchMakingResponseEventArgs args) =>
        {
            Debug.Log(args.ErrInfo);
            if(args.ErrInfo == ErrorCode.Success)
            {
                //���� �ƴٸ� JoinGameServer ȣ��
                string severAddress = args.RoomInfo.m_inGameServerEndPoint.m_address;
                ushort serverPort = args.RoomInfo.m_inGameServerEndPoint.m_port;
                string roomToken = args.RoomInfo.m_inGameRoomToken;
                JoinInGameServer(severAddress, serverPort, roomToken);
            }
        };
    }

    private void JoinInGameServer(string serverAddress, ushort serverPort, string roomToken)
    {
        bool isReconnect = false;
        ErrorInfo errorInfo = null;

        if(Backend.Match.JoinGameServer(serverAddress, serverPort, isReconnect, out errorInfo) == false)
        {
            //���� Ȯ��
            Debug.LogError(errorInfo);
            return;
        }
        else if(Backend.Match.JoinGameServer(serverAddress, serverPort, isReconnect, out errorInfo))
        {
            Backend.Match.OnSessionJoinInServer += (args) =>
            {
                Debug.Log(errorInfo);
                JoinInGameRoom(roomToken);
            };
        }
    }

    private void JoinInGameRoom(string token)
    {
        Backend.Match.JoinGameRoom(token);



        //���ӹ� ���� ���� �� 1�� ȣ��Ǵ� �̺�Ʈ
        Backend.Match.OnSessionListInServer = (MatchInGameSessionListEventArgs args) =>
        {
            roomInfo = args.RoomInfo;
            for(int i = 0; i < args.GameRecords.Count; i++)
            {
                gameRecords.Add(args.GameRecords[i].m_nickname);
            }
            Debug.Log(gameRecords);
        };

        //���ӹ濡 ������ ���� �� ��� Ŭ���̾�Ʈ���� ȣ��Ǵ� �̺�Ʈ
        Backend.Match.OnMatchInGameAccess += (MatchInGameSessionEventArgs args) =>
        {
            if(args.GameRecord.m_nickname != Backend.UserNickName)
            {
                gameRecords.Add(args.GameRecord.m_nickname);
                Debug.Log(args.GameRecord);
            }
            Debug.Log(gameRecords.Count);
        };

        //���ӹ濡 ��ΰ� ������ ������ �������� �� ȣ��Ǵ� �̺�Ʈ
        Backend.Match.OnMatchInGameStart = () => 
        {
            Utils.Instance.LoadScene(SceneNames.MatchRoom);
        };

        //������ ���ӹ濡 ������ �� ��ο��� ȣ��Ǵ� �̺�Ʈ
        Backend.Match.OnSessionOffline += (MatchInGameSessionEventArgs args) => 
        {
            gameRecords.Remove(args.GameRecord.m_nickname);
            Debug.Log(args.GameRecord.m_nickname + "���� �����̽��ϴ�.");

            Debug.Log(gameRecords.Count);
        };
    }

    public void LeaveGameServer()
    {
        Backend.Match.LeaveGameServer();

        Backend.Match.OnLeaveInGameServer = (MatchInGameSessionEventArgs args) => 
        {
            Debug.Log($"{args.GameRecord.m_nickname}���� �� ���� ������ �����̽��ϴ�.");
            Utils.Instance.LoadScene(SceneNames.Chatting);
        };
    }
}
