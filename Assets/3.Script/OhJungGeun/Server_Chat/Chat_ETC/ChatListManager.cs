using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class ChatListManager : MonoBehaviour
{
    //이 곳의 자식으로 말풍선 생성
    public Transform contents;


    public GameObject myChatListPrefab;
    public GameObject localChatListPrefab;
    public GameObject whisperChatListPrefab_My;
    public GameObject whisperChatListPrefab_Local;
    public GameObject joinUIPrefab;


    private void Start()
    {
        BackEndManager.Instance.GetChatManager().chatListManager = this;
        SpawnJoinUI(Backend.UserNickName);
    }

    public void SpawnJoinUI(string name)
    {
        GameObject joinUIClone = GameObject.Instantiate(joinUIPrefab, joinUIPrefab.transform.position, Quaternion.identity);
        JoinUI joinUI = joinUIClone.GetComponent<JoinUI>();
        joinUI.SetJoinUserName(name);
        joinUIClone.transform.SetParent(contents);
    }


    public void SpawnMyChatList(string username, string chat)
    {
        GameObject myChatListClone = GameObject.Instantiate(myChatListPrefab, myChatListPrefab.transform.position, Quaternion.identity);
        MyChatList myChatList = myChatListClone.GetComponent<MyChatList>();
        myChatList.SetMyChatList(username, chat);
        myChatListClone.transform.SetParent(contents);
    }

    public void SpawnLocalChatList(string username, string chat)
    {
        GameObject localChatListClone = GameObject.Instantiate(localChatListPrefab, localChatListPrefab.transform.position, Quaternion.identity);
        LocalChatList localChatList = localChatListClone.GetComponent<LocalChatList>();
        localChatList.SetLocalChatList(username, chat);
        localChatListClone.transform.SetParent(contents);
    }

    public void SpawnMyChatList_Whisper(string username, string chat)
    {
        GameObject myChatListClone = GameObject.Instantiate(whisperChatListPrefab_My, whisperChatListPrefab_My.transform.position, Quaternion.identity);
        MyChatList myChatList = myChatListClone.GetComponent<MyChatList>();
        myChatList.SetMyChatList(username, chat);
        myChatListClone.transform.SetParent(contents);
    }

    public void SpawnLocalChatList_Whisper(string username, string chat)
    {
        GameObject localChatListClone = GameObject.Instantiate(whisperChatListPrefab_Local, whisperChatListPrefab_Local.transform.position, Quaternion.identity);
        LocalChatList localChatList = localChatListClone.GetComponent<LocalChatList>();
        localChatList.SetLocalChatList(username, chat);
        localChatListClone.transform.SetParent(contents);
    }

}
