using BackEnd;
using BackEnd.Tcp;
using System;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class MatchSystem
{
    private bool isCanInviteUser;
    public bool IsCanInviteUser => isCanInviteUser;




    public EventHandler OnMatchInviteUI;
    public EventHandler OnMatchInviteUI_Error;


    [Header("Invited Data")]
    private SessionId roomId;
    private string roomToken;
    private MatchMakingUserInfo inviteUserInfo;
    public string inviteUserNickName; // 방장 닉네임 => 나중에 여기 DB를 이용해서 하우징 오브젝트 동기화

    public MatchMakingUserInfo InviteUserInfo => inviteUserInfo;
    public SessionId RoomID => roomId;
    public string RoomToken => roomToken;



    //초대 요청 시 제한 시간
    float timer = 15.0f;
    bool isTimerOn = false;


    //게임룸 정보
    public MatchInGameRoomInfo roomInfo; // 접속한 룸의 정보
    public Dictionary<SessionId, string> userNickName = new Dictionary<SessionId, string>(); // 현재 게임방에 접속해 있는 유저들의 닉네임
    public Dictionary<string, byte> userTeam = new Dictionary<string, byte>(); // 현재 게임방에 접속해 있는 유저들의 팀


    //매칭 서버 리스트 인덱스 접근
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


    //우리 매칭 서버 리스트 조회
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


    //매칭 조인이 가능한지  체크
    public void JoinMatchMakingCheck()
    {
        ErrorInfo errorInfo;
        Backend.Match.JoinMatchMakingServer(out errorInfo);
        Debug.Log(errorInfo);
    }

    //매치 메이킹 서버에 연결신청
    public void JoinMatchMaking()
    {


        ErrorInfo errorInfo;


        Backend.Match.JoinMatchMakingServer(out errorInfo);
        Backend.Match.OnJoinMatchMakingServer = (JoinChannelEventArgs args) =>
        {
            
            Debug.Log("JoinMatchMaking : " + args.ErrInfo);

            if(args.ErrInfo == ErrorInfo.Success)
            {
                Backend.Match.OnLeaveMatchMakingServer = (LeaveChannelEventArgs args) =>
                {
                    if(args.ErrInfo.Category == ErrorCode.Exception || args.ErrInfo.Category == ErrorCode.NetworkTimeout)
                    {
                        JoinMatchMaking();
                    }
                };
            }


            //CreateMatchRoom();
            //RequestMatchMaking(0);
        };
    }

    //매칭 서버에 연결됐을 시 호출할 대기방 생성 메서드
    public void CreateMatchRoom(string nickName)
    {
        if (timer <= 14.9f)
        {
            Debug.Log("현재 초대 중입니다.");
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

    //해당 유저 초대
    public void InviteUser(string nickName)
    {

        Backend.Match.InviteUser(nickName);
        Backend.Match.OnMatchMakingRoomInvite = (MatchMakingInteractionEventArgs args) => 
        {
            Debug.Log("InviteUser : " + args.ErrInfo);
            if (args.ErrInfo != ErrorCode.Success)
            {
                LeaveMatchRoom();
                OnMatchInviteUI_Error?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                inviteUserNickName = nickName;
                OnMatchInviteUI?.Invoke(this, EventArgs.Empty);
                isTimerOn = true;
            }
        };
    }

    //초대 수신 이벤트
    public void OnMatchMakingRoomSomeoneInvited(Action Todo)
    {
        Backend.Match.OnMatchMakingRoomSomeoneInvited = (MatchMakingInvitedRoomEventArgs args) => 
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

    //초대 요청이 수신이 잘 됐다면 시간 흐르기
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

    //초대 수락 Or 거절 이벤트
    public void AreYouAccept(bool isAccept)
    {
        if (isAccept)
        {
            Backend.Match.AcceptInvitation(roomId, roomToken);
            Backend.Match.OnMatchMakingRoomUserList = (MatchMakingGamerInfoListInRoomEventArgs args) => 
            {
                Debug.Log("대기 방 초대 수락 입장");
            };
        }
        else
        {
            Backend.Match.DeclineInvitation(roomId, roomToken);
            {
                Debug.Log("대기방 초대 거절");
            }
        }

        Backend.Match.OnMatchMakingRoomInviteResponse = (MatchMakingInteractionEventArgs args) => 
        {
            // TODO
            Debug.Log(args.ErrInfo);
        };
    }

    //유저 입장 이벤트
    public void OnMatchMakingRoomJoin()
    {
        Backend.Match.OnMatchMakingRoomJoin = (MatchMakingGamerInfoInRoomEventArgs args) => 
        {
            Debug.Log("유저 들어옴");
            RequestMatchMaking(0);
            Utils.Instance.LoadScene(SceneNames.MatchLoad);
        };
    }



    //대기방 퇴장
    public void LeaveMatchRoom()
    {
        isTimerOn = false;
        timer = 15.0f;
        Backend.Match.LeaveMatchRoom();
        //Backend.Match.LeaveMatchMakingServer();
    }













    //=========이후 매칭 신청 + 인게임 서버 ================//

    //해당 인덱스의 매칭 서버를 통해 매칭 신청
    public void RequestMatchMaking(int index)
    {
        Backend.Match.RequestMatchMaking(GetMatchList(index).matchTypeEnum, GetMatchList(index).matchModeTypeEnum, GetMatchList(index).inDate);
        Backend.Match.OnMatchMakingResponse = (MatchMakingResponseEventArgs args) =>
        {
            Debug.Log(args.ErrInfo);
            if(args.ErrInfo == ErrorCode.Success)
            {
                //연결 됐다면 JoinGameServer 호출
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
            //에러 확인
            Debug.LogError(errorInfo);
            return;
        }
        else if(Backend.Match.JoinGameServer(serverAddress, serverPort, isReconnect, out errorInfo))
        {
            Backend.Match.OnSessionJoinInServer = (args) =>
            {
                Debug.Log(errorInfo);
                JoinInGameRoom(roomToken);
            };
        }
    }

    private void JoinInGameRoom(string token)
    {
        Backend.Match.JoinGameRoom(token);



        //게임방 최초 접속 시 1번 호출되는 이벤트
        Backend.Match.OnSessionListInServer = (MatchInGameSessionListEventArgs args) =>
        {
            roomInfo = args.RoomInfo;
            userNickName.Clear();
            for(int i = 0; i < args.GameRecords.Count; i++)
            {
                userNickName.Add(args.GameRecords[i].m_sessionId, args.GameRecords[i].m_nickname);
                userTeam.Add(args.GameRecords[i].m_nickname, args.GameRecords[i].m_teamNumber);
            }


            //게임방에 유저가 접속 시 모든 클라이언트에게 호출되는 이벤트
            Backend.Match.OnMatchInGameAccess = (MatchInGameSessionEventArgs args) =>
            {

                if (args.GameRecord.m_nickname != Backend.UserNickName)
                {
                    userNickName.Add(args.GameRecord.m_sessionId, args.GameRecord.m_nickname);
                    userTeam.Add(args.GameRecord.m_nickname, args.GameRecord.m_teamNumber);
                    Debug.Log(args.GameRecord);
                }
            };

            //누군가 게임방에 나갔을 때 모두에게 호출되는 이벤트
            Backend.Match.OnSessionOffline = (MatchInGameSessionEventArgs args) =>
            {
                userNickName.Remove(args.GameRecord.m_sessionId);
                MatchRoomTest.Instance.LeaveIDObjectDestory(args.GameRecord.m_sessionId);
                Debug.Log(args.GameRecord.m_nickname + "님이 나가셨습니다.");

                Debug.Log(userNickName.Count);
            };

            //게임방에 모두가 들어오고 게임이 시작했을 때 호출되는 이벤트
            Backend.Match.OnMatchInGameStart = () =>
            {
                Backend.Match.OnMatchRelay += (args) =>
                {
                    // 각 클라이언트들이 서버를 통해 주고받은 패킷들
                    // 서버는 단순 브로드캐스팅만 지원 (서버에서 어떠한 연산도 수행하지 않음)
                    OnRecieve(args);
                };

                Utils.Instance.LoadScene(SceneNames.MatchRoom);
            };
        };
    }

    public void LeaveGameServer()
    {
        Backend.Match.LeaveGameServer();
        LeaveMatchRoom();

        Backend.Match.OnLeaveInGameServer = (MatchInGameSessionEventArgs args) => 
        {
            Debug.Log($"{args.GameRecord.m_nickname}님이 인 게임 서버를 나가셨습니다.");
            Utils.Instance.LoadScene(SceneNames.Chatting);

            if (args.ErrInfo == ErrorCode.Exception)
            {
                Debug.Log("재접속 시도중");
                Backend.Match.IsGameRoomActivate((callback) =>
                {
                    var bro = Backend.Match.IsGameRoomActivate();
                    var roomInfo = bro.GetReturnValuetoJSON();
                    var addr = roomInfo["serverPublicHostName"].ToString();
                    var port = Convert.ToUInt16(roomInfo["serverPort"].ToString());
                    ErrorInfo errorInfo = null;

                    if (callback.GetStatusCode() == "200")
                    {
                        if (Backend.Match.JoinGameServer(addr, port, true, out errorInfo) == false)
                        {
                            // 에러 확인
                            return;
                        }

                        Backend.Match.OnSessionOnline += (MatchInGameSessionEventArgs args) =>
                        {
                            // TODO
                            userNickName.Add(args.GameRecord.m_sessionId, args.GameRecord.m_nickname);
                            Debug.Log(args.GameRecord.m_nickname + "님이 재접속 하셨습니다.");

                            Debug.Log(userNickName.Count);
                        };
                    }
                    Debug.Log("재접속 불가합니다.");
                });
            }
            else
            {
                Debug.Log("게임룸 정보 초기화");
                roomInfo = null;
                userNickName.Clear();
            }
        };
    }





    //=========MatchInRoom==============
    public void SendDataToInGame<T>(T msg)
    {
        var byteArray = DataParser.DataToJsonData<T>(msg);
        Backend.Match.SendDataToInGameRoom(byteArray);
    }

    public void OnRecieve(MatchRelayEventArgs args)
    {
        if (args.BinaryUserData == null)
        {
            Debug.LogWarning(string.Format("빈 데이터가 브로드캐스팅 되었습니다.\n{0} - {1}", args.From, args.ErrInfo));
            // 데이터가 없으면 그냥 리턴
            return;
        }

        Message msg = DataParser.ReadJsonData<Message>(args.BinaryUserData);
        if (msg == null)
        {
            return;
        }

        if (args.From.SessionId == Backend.Match.GetMySessionId())
        {
            return;
        }

        if(userTeam[args.From.NickName] != userTeam[Backend.UserNickName])
        {
            return;
        }

        switch (msg.type)
        {
            case Protocol.Type.PlayerMove:
                if(Utils.Instance.nowScene == SceneNames.MatchRoom)
                {
                    PlayerMoveMessage moveMessage = DataParser.ReadJsonData<PlayerMoveMessage>(args.BinaryUserData);
                    MatchRoomTest.Instance.ProcessPlayerData(moveMessage);
                }
                break;
            case Protocol.Type.PlayerChat:
                if (Utils.Instance.nowScene == SceneNames.MatchRoom)
                {
                    PlayerChatMessage chatMessage = DataParser.ReadJsonData<PlayerChatMessage>(args.BinaryUserData);
                    MatchRoomTest.Instance.ProcessPlayerData(chatMessage);
                }
                break;
            default:
                Debug.Log("Unknown protocol type");
                return;
        }
    }

}
