using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;

public class ChatManager
{
    private bool isChatServerLink = false;
    private string normalChatList = "Normal Chat";
    List<ChatGroup> publicGroupsList = new List<ChatGroup>();

    // 뒤끝챗 활성화 되어 있는지 확인
    public void GetChatStatus()
    {
        Backend.Chat.GetChatStatus((callback) =>
        {
            if(callback.IsSuccess())
            {
                isChatServerLink = true;
            }

            Debug.Log(isChatServerLink);
            GetGroupChannelList(normalChatList);
            JoinChannel();
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
        }

        ErrorInfo errorInfo;
        //입장 요청
        Backend.Chat.JoinChannel(ChannelType.Public, joinGroupData.serverAddress, joinGroupData.serverPort, joinGroupData.groupName, joinGroupData.inDate, out errorInfo);


        //입장되었는지 이벤트 확인
        Backend.Chat.OnJoinChannel = (JoinChannelEventArgs args) =>
        {
            Debug.Log($"OnJoinChannel {args.ErrInfo}");

            if (args.ErrInfo == ErrorInfo.Success)
            {
                // 내가 접속한 경우
                if (!args.Session.IsRemote)
                {
                    Debug.Log("채널에 접속했습니다");
                }
                //다른 유저가 접속한 경우
                else
                {
                    Debug.Log($"{args.Session.NickName}님이 접속했습니다");
                }
            }
            else
            {
                //에러가 발생했을 경우
                Debug.Log($"입장 도중 에러가 발생했습니다 : {args.ErrInfo.Reason}");
            }
        };
    }

}
