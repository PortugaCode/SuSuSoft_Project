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
                Backend.Chat.SetFilterUse(true);
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

    //�ش� ä�ο� ä�� ����
    public void ChatToChannel(string chat)
    {
        Backend.Chat.ChatToChannel(ChannelType.Public, chat);
    }

    //ä�� �ޱ�
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
                    Debug.Log($"{args.From.NickName}�� {args.Message}�� �Է��߽��ϴ�.");
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

    //�ӼӸ� ä�� ����
    public void Whisper(string ToNickname, string Message)
    {
        if(ToNickname == Backend.UserNickName)
        {
            chatListManager.SpawnErrorUI($"�ڽ����� ���� �� �����ϴ�.");
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

    //�ӼӸ� �ޱ�
    public void ReceiveWhisperChat()
    {
        Backend.Chat.OnWhisper = (WhisperEventArgs args) =>
        {
            Debug.Log(string.Format("OnWhisper {0}", args.ErrInfo));

            if (args.ErrInfo == ErrorInfo.Success)
            {
                //Debug.Log(string.Format("OnWhisper: from {0} to {1} : message {2}", args.From.NickName, args.To.NickName, args.Message));

                // ���� ���� �ӼӸ��� ���
                if (!args.From.IsRemote)
                {
                    chatListManager.SpawnMyChatList_Whisper(Backend.UserNickName, args.Message);
                    Debug.Log("�� : " + args.Message);
                }
                // ���� ���� �ӼӸ��� ���
                else
                {
                    chatListManager.SpawnLocalChatList_Whisper(args.From.NickName, args.Message);
                    Debug.Log(string.Format("{0}�� : {1}", args.From.NickName, args.Message));
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

    //���� ���� Ȯ�� (1. �г��� Ȯ�� 2. �ش� ���� ���� Ȯ��)
    public bool CheckUser(string nickName)
    {
        //�ǽð� �˸� ������ �����մϴ�.  
        Backend.Notification.Connect();

        //"a1"�� �г����� ���� ������ inDate�� ã�´�
        BackendReturnObject bro = Backend.Social.GetUserInfoByNickName(nickName);
        if(bro.GetFlattenJSON() == null)
        {
            Backend.Notification.DisConnect();
            Debug.Log("�������� �ʴ� �г����Դϴ�.");
            chatListManager.SpawnErrorUI($"[{nickName}]���� �������� �ʴ� �г����Դϴ�.");
            return false;
        }
        else
        {
            string gamerIndate = bro.GetFlattenJSON()["row"]["inDate"].ToString();

            // UserIsConnectByIndate �Լ� ȣ�� �� �����ϴ� OnIsConnectUser �ڵ鷯�� �����Ѵ�.  
            Backend.Notification.OnIsConnectUser = (bool isConnect, string nickName, string gamerIndate) => {
                Debug.Log($"{nickName} / {gamerIndate} ���� ���� Ȯ�� : " + isConnect);
            };

            // �Լ� ȣ�� ��, ������ ������ OnIsConnectUser �ڵ鷯�� ȣ��ȴ�.  
            Backend.Notification.UserIsConnectByIndate(gamerIndate);

            Backend.Notification.DisConnect();
            return true;
        }

    }

    //ä�� �ֱ� ���� �ҷ�����
    public void GetRecentChat()
    {
        //ä�� ä�� ����Ʈ ��������
        BackendReturnObject bro = Backend.Chat.GetGroupChannelList(normalChatList);

        //ä�� ä�� uuid �޾ƿ���
        string channelIndate = bro.GetReturnValuetoJSON()["rows"][0]["inDate"].ToString();

        //uuid�� �̿��Ͽ� �ش� �Ϲ� ä���� �ֱ� ä�� ���� ��������(25����)
        BackendReturnObject result = Backend.Chat.GetRecentChat(ChannelType.Public, channelIndate, 20);


         for (int i = 0; i < result.Rows().Count; i++)
         {
             string nickname = result.Rows()[i]["nickname"].ToString();
             string message = result.Rows()[i]["message"].ToString();

            if (nickname.Equals(Backend.UserNickName))
            {
                 Debug.Log("�ֱ� ä�� ���� �ҷ�����");
                 chatListManager.SpawnMyChatList(nickname, message);
            }
            else
            {
                Debug.Log("�ֱ� ä�� ���� �ҷ�����");
                chatListManager.SpawnLocalChatList(nickname, message);
            }
        }
    }

}
