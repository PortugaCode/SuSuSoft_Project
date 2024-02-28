using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;

public class ChatManager
{
    private bool isChatServerLink = false;
    private string normalChatList = "Public Chat";
    private List<ChatGroup> publicGroupsList = new List<ChatGroup>();


    public ChatListManager chatListManager;

    //private List<string[]> a = new List<string[]>();

    // 뒤끝챗 활성화 되어 있는지 확인
    public void GetChatStatus()
    {
        Backend.Chat.GetChatStatus((callback) =>
        {
            if(callback.IsSuccess())
            {
                isChatServerLink = true;
            }
            
            if(isChatServerLink)
            {
                Backend.Chat.SetFilterUse(true);
                GetGroupChannelList(normalChatList);
                JoinChannel();
            }

        });
    }

    // 매개변수 그룹 명에 채널 리스트 불러오기
    public void GetGroupChannelList(string c)
    {
        string a = c;

        var callback = Backend.Chat.GetGroupChannelList(a);

        if (!callback.IsSuccess())
        {
            Debug.LogError($"{a} 채팅 채널 불러오기 실패 : {callback.ToString()}");
            return;
        }

        LitJson.JsonData groupListJson = callback.FlattenRows();

        //초기화
        publicGroupsList.Clear();

        for (int i = 0; i < groupListJson.Count; i++)
        {
            ChatGroup chatGroup = new ChatGroup();

            chatGroup.groupName = groupListJson[i]["groupName"].ToString();
            chatGroup.alias = groupListJson[i]["alias"].ToString();
            chatGroup.inDate = groupListJson[i]["inDate"].ToString();
            chatGroup.maxUserCount = int.Parse(groupListJson[i]["maxUserCount"].ToString());
            chatGroup.joinedUserCount = int.Parse(groupListJson[i]["joinedUserCount"].ToString());
            chatGroup.serverAddress = groupListJson[i]["serverAddress"].ToString();
            chatGroup.serverPort = ushort.Parse(groupListJson[i]["serverPort"].ToString());

            publicGroupsList.Add(chatGroup);
        }

        string str = "Backend.Chat.GetGroupChannelList : \n";
        foreach (var li in publicGroupsList)
        {
            str += li.ToString() + "\n";
        }
        Debug.Log(str);
    }


    //채널 타입 public | Guild의 소켓 접속 여부 확인
    public bool IsChatConnect(ChannelType channelType)
    {
        bool isConnect = Backend.Chat.IsChatConnect(channelType);
        return isConnect;
    }

    //채널에 입장 요청
    public void JoinChannel()
    {
        ChatGroup joinGroupData = publicGroupsList[0];

        for (int i = 0; i < publicGroupsList.Count; i++)
        {
            if(publicGroupsList[i].joinedUserCount < 190)
            {
                joinGroupData = publicGroupsList[i];
                break;
            }
            joinGroupData = publicGroupsList[i];
        }

        ErrorInfo errorInfo;
        //입장 요청
        Backend.Chat.JoinChannel(ChannelType.Public, joinGroupData.serverAddress, joinGroupData.serverPort, joinGroupData.groupName, joinGroupData.inDate, out errorInfo);

        ReceiveJoin();

    }

    //입장되었는지 이벤트 확인
    public void ReceiveJoin()
    {
        Backend.Chat.OnJoinChannel = (JoinChannelEventArgs args) =>
        {
            Debug.Log($"OnJoinChannel {args.ErrInfo}");

            if (args.ErrInfo == ErrorInfo.Success)
            {
                // 내가 접속한 경우
                if (!args.Session.IsRemote)
                {
                    Utils.Instance.LoadScene(SceneNames.Chatting);
                    Backend.Chat.OnLeaveChannel = (LeaveChannelEventArgs args) =>
                    {
                        if(args.ErrInfo.Detail == ErrorCode.DisconnectFromRemote)
                        {
                            JoinChannel();
                        }
                    };
                }
                //다른 유저가 접속한 경우
                else
                {
                    chatListManager.SpawnJoinUI(args.Session.NickName);
                    Debug.Log($"{args.Session.NickName}님이 접속했습니다");
                }
            }
            else
            {
                //에러가 발생했을 경우
                Debug.Log($"입장 도중 에러가 발생했습니다 : {args.ErrInfo.Reason}");

                if(args.ErrInfo.Category == ErrorCode.Exception)
                {
                    JoinChannel();
                }
            }
        };
    }

    //해당 채널에 채팅 전송
    public void ChatToChannel(string chat)
    {
        Backend.Chat.ChatToChannel(ChannelType.Public, chat);
    }

    //채팅 받기
    public void ReceiveChat()
    {
        Backend.Chat.OnChat = (ChatEventArgs args) =>
        {
            //Debug.Log(string.Format("OnChat {0}", args.ErrInfo));

            if (args.ErrInfo == ErrorInfo.Success)
            {
                // 자신의 메시지일 경우
                if (!args.From.IsRemote)
                {
                    chatListManager.SpawnMyChatList(Backend.UserNickName, args.Message);
                    Debug.Log("나 : " + args.Message);
                }
                // 다른 유저의 메시지일 경우
                else
                {
                    chatListManager.SpawnLocalChatList(args.From.NickName, args.Message);
                    Debug.Log($"{args.From.NickName}이 {args.Message}를 입력했습니다.");
                }
            }
            else if (args.ErrInfo.Category == ErrorCode.BannedChat)
            {
                // 도배방지 메세지
                if (args.ErrInfo.Detail == ErrorCode.BannedChat)
                {
                    Debug.Log("메시지를 너무 많이 입력하였습니다. 일정 시간 후에 다시 시도해 주세요");
                }
            }
        };
    }

    //귓속말 채팅 전송
    public void Whisper(string ToNickname, string Message)
    {
        if(ToNickname == Backend.UserNickName)
        {
            chatListManager.SpawnErrorUI($"자신한테 보낼 수 없습니다.");
            return;
        }
        else if(!CheckUser(ToNickname))
        {
            return;
        }
        else
        {
            Backend.Chat.Whisper(ToNickname, Message);
        }

    }

    //귓속말 받기
    public void ReceiveWhisperChat()
    {
        Backend.Chat.OnWhisper = (WhisperEventArgs args) =>
        {
            Debug.Log(string.Format("OnWhisper {0}", args.ErrInfo));

            if (args.ErrInfo == ErrorInfo.Success)
            {
                //Debug.Log(string.Format("OnWhisper: from {0} to {1} : message {2}", args.From.NickName, args.To.NickName, args.Message));

                // 내가 보낸 귓속말인 경우
                if (!args.From.IsRemote)
                {
                    chatListManager.SpawnMyChatList_Whisper(Backend.UserNickName, args.Message);
                    Debug.Log("나 : " + args.Message);
                }
                // 내가 받은 귓속말인 경우
                else
                {
                    chatListManager.SpawnLocalChatList_Whisper(args.From.NickName, args.Message);
                    Debug.Log(string.Format("{0}님 : {1}", args.From.NickName, args.Message));
                }
            }
            else if (args.ErrInfo.Category == ErrorCode.BannedChat)
            {
                // 도배방지 메세지
                if (args.ErrInfo.Detail == ErrorCode.BannedChat)
                {
                    Debug.Log("메시지를 너무 많이 입력하였습니다. 일정 시간 후에 다시 시도해 주세요");
                }
            }
        };
    }

    //유저 상태 확인 (1. 닉네임 확인 2. 해당 유저 접속 확인)
    public bool CheckUser(string nickName)
    {
        //실시간 알림 서버에 연결합니다.  
        Backend.Notification.Connect();

        //"a1"의 닉네임을 가진 유저의 inDate를 찾는다
        BackendReturnObject bro = Backend.Social.GetUserInfoByNickName(nickName);
        if(bro.GetFlattenJSON() == null)
        {
            Backend.Notification.DisConnect();
            Debug.Log("존재하지 않는 닉네임입니다.");
            chatListManager.SpawnErrorUI($"[{nickName}]님은 존재하지 않는 닉네임입니다.");
            return false;
        }
        else
        {
            string gamerIndate = bro.GetFlattenJSON()["row"]["inDate"].ToString();

            // UserIsConnectByIndate 함수 호출 시 반응하는 OnIsConnectUser 핸들러를 설정한다.  
            Backend.Notification.OnIsConnectUser = (bool isConnect, string nickName, string gamerIndate) => {
                Debug.Log($"{nickName} / {gamerIndate} 접속 여부 확인 : " + isConnect);
            };

            // 함수 호출 시, 위에서 설정한 OnIsConnectUser 핸들러가 호출된다.  
            Backend.Notification.UserIsConnectByIndate(gamerIndate);

            Backend.Notification.DisConnect();
            return true;
        }

    }

    //채팅 최근 내역 불러오기
    public void GetRecentChat()
    {
        //채팅 채널 리스트 가져오기
        BackendReturnObject bro = Backend.Chat.GetGroupChannelList(normalChatList);

        //채팅 채널 uuid 받아오기
        string channelIndate = bro.GetReturnValuetoJSON()["rows"][0]["inDate"].ToString();

        //uuid를 이용하여 해당 일반 채널의 최근 채팅 내역 가져오기(25개만)
        BackendReturnObject result = Backend.Chat.GetRecentChat(ChannelType.Public, channelIndate, 15);


         for (int i = 0; i < result.Rows().Count; i++)
         {
             string nickname = result.Rows()[i]["nickname"].ToString();
             string message = result.Rows()[i]["message"].ToString();

            if (nickname.Equals(Backend.UserNickName))
            {
                 Debug.Log("최근 채팅 내역 불러오기");
                 chatListManager.SpawnMyChatList(nickname, message);
            }
            else
            {
                Debug.Log("최근 채팅 내역 불러오기");
                chatListManager.SpawnLocalChatList(nickname, message);
            }
        }
    }

}
