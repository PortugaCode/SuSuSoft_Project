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

    // �ڳ�ê Ȱ��ȭ �Ǿ� �ִ��� Ȯ��
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

    // �Ű����� �׷� �� ä�� ����Ʈ �ҷ�����
    public void GetGroupChannelList(string c)
    {
        string a = c;

        var callback = Backend.Chat.GetGroupChannelList(a);

        if (!callback.IsSuccess())
        {
            Debug.LogError($"{a} ä�� ä�� �ҷ����� ���� : {callback.ToString()}");
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


    //ä�� Ÿ�� public | Guild�� ���� ���� ���� Ȯ��
    public bool IsChatConnect(ChannelType channelType)
    {
        bool isConnect = Backend.Chat.IsChatConnect(channelType);
        return isConnect;
    }

    //ä�ο� ���� ��û
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
        //���� ��û
        Backend.Chat.JoinChannel(ChannelType.Public, joinGroupData.serverAddress, joinGroupData.serverPort, joinGroupData.groupName, joinGroupData.inDate, out errorInfo);


        //����Ǿ����� �̺�Ʈ Ȯ��
        Backend.Chat.OnJoinChannel = (JoinChannelEventArgs args) =>
        {
            Debug.Log($"OnJoinChannel {args.ErrInfo}");

            if (args.ErrInfo == ErrorInfo.Success)
            {
                // ���� ������ ���
                if (!args.Session.IsRemote)
                {
                    Debug.Log("ä�ο� �����߽��ϴ�");
                }
                //�ٸ� ������ ������ ���
                else
                {
                    Debug.Log($"{args.Session.NickName}���� �����߽��ϴ�");
                }
            }
            else
            {
                //������ �߻����� ���
                Debug.Log($"���� ���� ������ �߻��߽��ϴ� : {args.ErrInfo.Reason}");
            }
        };
    }

}
