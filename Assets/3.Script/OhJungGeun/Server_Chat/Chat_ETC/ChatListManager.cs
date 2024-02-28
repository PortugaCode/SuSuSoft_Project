using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class ChatListManager : MonoBehaviour
{
    [SerializeField] private ChatLineControl chatLineControl;


    //이 곳의 자식으로 말풍선 생성
    public Transform contents;
    [SerializeField] private Transform mainParent;

    public Queue<MyChatList> myChatLists = new Queue<MyChatList>(); 
    public Queue<MyChatList> myChatLists_Whisper = new Queue<MyChatList>();

    public Queue<LocalChatList> localChatLists = new Queue<LocalChatList>();
    public Queue<LocalChatList> localChatLists_Whisper = new Queue<LocalChatList>();

    public Queue<JoinUI> joinUIs = new Queue<JoinUI>();
    public Queue<JoinUI> errorUIs = new Queue<JoinUI>();


    [Header("ChatList Prefab")]
    public MyChatList myChatListPrefab;
    public LocalChatList localChatListPrefab;
    public MyChatList whisperChatListPrefab_My;
    public LocalChatList whisperChatListPrefab_Local;
    public JoinUI joinUIPrefab;
    public JoinUI errorUIPrefab;



    [SerializeField] private GameObject matchInvitedUI;
    [SerializeField] private GameObject matchInvitedCheckUI;
    public GameObject matchInvitedFail;


    private int listCounts;


    private void Awake()
    {
        listCounts = 0;

        for(int i = 0; i < 20; i++)
        {
            myChatLists.Enqueue(MyChatList.Instantiate(myChatListPrefab, myChatListPrefab.transform.position, Quaternion.identity, mainParent));
            myChatLists_Whisper.Enqueue(MyChatList.Instantiate(whisperChatListPrefab_My, whisperChatListPrefab_My.transform.position, Quaternion.identity, mainParent));

            localChatLists.Enqueue(LocalChatList.Instantiate(localChatListPrefab, localChatListPrefab.transform.position, Quaternion.identity, mainParent));
            localChatLists_Whisper.Enqueue(LocalChatList.Instantiate(whisperChatListPrefab_Local, whisperChatListPrefab_Local.transform.position, Quaternion.identity, mainParent));

            joinUIs.Enqueue(JoinUI.Instantiate(joinUIPrefab, joinUIPrefab.transform.position, Quaternion.identity, mainParent));
            errorUIs.Enqueue(JoinUI.Instantiate(errorUIPrefab, errorUIPrefab.transform.position, Quaternion.identity, mainParent));
        }
    }

    private void Start()
    {
        BackEndManager.Instance.GetChatManager().chatListManager = this;
        chatLineControl.GetChatUI().SetActive(true);
        //여기에 조건 추가하기
        BackEndManager.Instance.GetChatManager().GetRecentChat();
        chatLineControl.GetChatUI().SetActive(false);
        BackEndManager.Instance.GetMatchSystem().OnMatchMakingRoomSomeoneInvited(SetMatchInvitedUI_True);
        BackEndManager.Instance.GetMatchSystem().OnMatchInviteUI = SetMatchInvitedCheckUI_True;
        BackEndManager.Instance.GetMatchSystem().OnMatchInviteUI_Error = InviteUserError;
        SpawnJoinUI(Backend.UserNickName);
    }

    private void EraseChatList()
    {
        if(listCounts >= 19)
        {
            if (contents.GetChild(0).TryGetComponent<LocalChatList>(out LocalChatList local))
            {
                if(local.IsWhisper)
                {
                    local.gameObject.SetActive(false);
                    local.transform.SetParent(mainParent);
                    localChatLists_Whisper.Enqueue(local);
                }
                else
                {
                    local.gameObject.SetActive(false);
                    local.transform.SetParent(mainParent);
                    localChatLists.Enqueue(local);
                }
            }
            else if(contents.GetChild(0).TryGetComponent<MyChatList>(out MyChatList my))
            {
                if (my.IsWhisper)
                {
                    my.gameObject.SetActive(false);
                    my.transform.SetParent(mainParent);
                    myChatLists_Whisper.Enqueue(my);
                }
                else
                {
                    my.gameObject.SetActive(false);
                    my.transform.SetParent(mainParent);
                    myChatLists.Enqueue(my);
                }
            }
            else if(contents.GetChild(0).TryGetComponent<JoinUI>(out JoinUI msg))
            {
                if (msg.IsError)
                {
                    msg.gameObject.SetActive(false);
                    msg.transform.SetParent(mainParent);
                    errorUIs.Enqueue(msg);
                }
                else
                {
                    msg.gameObject.SetActive(false);
                    msg.transform.SetParent(mainParent);
                    joinUIs.Enqueue(msg);
                }
            }
        }
    }

    public void SpawnJoinUI(string name)
    {
        JoinUI joinUI = joinUIs.Dequeue();
        joinUI.SetJoinUserName(name);
        joinUI.SetState(false);

        joinUI.gameObject.transform.SetParent(contents);
        joinUI.gameObject.SetActive(true);
        listCounts++;
        EraseChatList();
    }

    public void SpawnErrorUI(string msg)
    {
        JoinUI errorUI = errorUIs.Dequeue();
        errorUI.SetErrorMessage(msg);
        errorUI.SetState(true);

        errorUI.gameObject.transform.SetParent(contents);
        errorUI.gameObject.SetActive(true);
        listCounts++;
        EraseChatList();
    }


    public void SpawnMyChatList(string username, string chat)
    {
        MyChatList myChatList = myChatLists.Dequeue();
        myChatList.SetMyChatList(username, chat);
        myChatList.SetIsWhisper(false);

        myChatList.gameObject.transform.SetParent(contents);
        myChatList.gameObject.SetActive(true);
        listCounts++;
        chatLineControl.SetChatLine(myChatList.gameObject, false, false);
    }

    public void SpawnLocalChatList(string username, string chat)
    {
        LocalChatList localChatList = localChatLists.Dequeue();
        localChatList.SetLocalChatList(username, chat);
        localChatList.SetIsWhisper(false);

        localChatList.gameObject.transform.SetParent(contents);
        localChatList.gameObject.SetActive(true);
        listCounts++;
        chatLineControl.SetChatLine(localChatList.gameObject, true, false);
    }

    public void SpawnMyChatList_Whisper(string username, string chat)
    {
        MyChatList myChatList = myChatLists_Whisper.Dequeue();
        myChatList.SetMyChatList(username, chat);
        myChatList.SetIsWhisper(true);

        myChatList.gameObject.transform.SetParent(contents);
        myChatList.gameObject.SetActive(true);
        listCounts++;
        chatLineControl.SetChatLine(myChatList.gameObject, false, true);
    }

    public void SpawnLocalChatList_Whisper(string username, string chat)
    {
        LocalChatList localChatList = localChatLists_Whisper.Dequeue();
        localChatList.SetLocalChatList(username, chat);
        localChatList.SetIsWhisper(true);

        localChatList.gameObject.transform.SetParent(contents);
        localChatList.gameObject.SetActive(true);
        listCounts++;
        chatLineControl.SetChatLine(localChatList.gameObject, true, true);
    }

    public void SetMatchInvitedUI_True()
    {
        matchInvitedUI.gameObject.SetActive(true);
    }

    public void SetMatchInvitedCheckUI_True(object sender, EventArgs args)
    {
        matchInvitedCheckUI.gameObject.SetActive(true);
    }

    public void InviteUserError(object sender, EventArgs args)
    {
        string errorMsg = "매칭 서버 초대에 실패하셨습니다.";

        SpawnErrorUI(errorMsg);
    }

}
