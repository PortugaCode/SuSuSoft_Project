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

    // �ڳ�ê Ȱ��ȭ �Ǿ� �ִ��� Ȯ��
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
                GetGroupChannelList(normalChatList);
                JoinChannel();
            }

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

        //�ʱ�ȭ
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
            joinGroupData = publicGroupsList[i];
        }

        ErrorInfo errorInfo;
        //���� ��û
        Backend.Chat.JoinChannel(ChannelType.Public, joinGroupData.serverAddress, joinGroupData.serverPort, joinGroupData.groupName, joinGroupData.inDate, out errorInfo);

        ReceiveJoin();

    }

    //����Ǿ����� �̺�Ʈ Ȯ��
    public void ReceiveJoin()
    {
        Backend.Chat.OnJoinChannel = (JoinChannelEventArgs args) =>
        {
            Debug.Log($"OnJoinChannel {args.ErrInfo}");

            if (args.ErrInfo == ErrorInfo.Success)
            {
                // ���� ������ ���
                if (!args.Session.IsRemote)
                {
                    Utils.Instance.LoadScene(SceneNames.Chatting);

                }
                //�ٸ� ������ ������ ���
                else
                {
                    chatListManager.SpawnJoinUI(args.Session.NickName);
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


    public void ChatToChannel(string chat)
    {
        Backend.Chat.ChatToChannel(ChannelType.Public, chat);
        ReceiveChat();
    }

    public void ReceiveChat()
    {
        Backend.Chat.OnChat = (ChatEventArgs args) =>
        {
            //Debug.Log(string.Format("OnChat {0}", args.ErrInfo));

            if (args.ErrInfo == ErrorInfo.Success)
            {
                // �ڽ��� �޽����� ���
                if (!args.From.IsRemote)
                {
                    chatListManager.SpawnMyChatList(Backend.UserNickName, args.Message);
                    Debug.Log("�� : " + args.Message);
                }
                // �ٸ� ������ �޽����� ���
                else
                {
                    chatListManager.SpawnLocalChatList(args.From.NickName, args.Message);
                }
            }
            else if (args.ErrInfo.Category == ErrorCode.BannedChat)
            {
                // ������� �޼���
                if (args.ErrInfo.Detail == ErrorCode.BannedChat)
                {
                    Debug.Log("�޽����� �ʹ� ���� �Է��Ͽ����ϴ�. ���� �ð� �Ŀ� �ٽ� �õ��� �ּ���");
                }
            }
        };
    }

}
