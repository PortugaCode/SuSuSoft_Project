using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class ChatListManager : MonoBehaviour
{
    [SerializeField] private ChatLineControl chatLineControl;


    //이 곳의 자식으로 말풍선 생성
    public Transform contents;


    public GameObject myChatListPrefab;
    public GameObject localChatListPrefab;
    public GameObject whisperChatListPrefab_My;
    public GameObject whisperChatListPrefab_Local;
    public GameObject joinUIPrefab;
    public GameObject errorUIPrefab;


    private void Start()
    {
        BackEndManager.Instance.GetChatManager().chatListManager = this;
        chatLineControl.GetChatUI().SetActive(true);
        BackEndManager.Instance.GetChatManager().GetRecentChat();
        chatLineControl.GetChatUI().SetActive(false);
        SpawnJoinUI(Backend.UserNickName);
    }

    private void ChatDestroyer()
    {
        if(contents.childCount >= 25)
        {
            Destroy(contents.GetChild(0).gameObject);
        }
    }


    public void SpawnJoinUI(string name)
    {
        GameObject joinUIClone = GameObject.Instantiate(joinUIPrefab, joinUIPrefab.transform.position, Quaternion.identity);
        JoinUI joinUI = joinUIClone.GetComponent<JoinUI>();
        joinUI.SetJoinUserName(name);
        joinUIClone.transform.SetParent(contents);
        joinUIClone.SetActive(true);
        ChatDestroyer();
        //chatLineControl.SetChatLine(joinUIClone);
    }

    public void SpawnErrorUI(string msg)
    {
        GameObject errorUIClone = GameObject.Instantiate(errorUIPrefab, errorUIPrefab.transform.position, Quaternion.identity);
        JoinUI errorUI = errorUIClone.GetComponent<JoinUI>();
        errorUI.SetErrorMessage(msg);
        errorUIClone.transform.SetParent(contents);
        errorUIClone.SetActive(true);
        ChatDestroyer();
        //chatLineControl.SetChatLine(errorUIClone);
    }


    public void SpawnMyChatList(string username, string chat)
    {
        GameObject myChatListClone = GameObject.Instantiate(myChatListPrefab, myChatListPrefab.transform.position, Quaternion.identity);
        MyChatList myChatList = myChatListClone.GetComponent<MyChatList>();
        myChatList.SetMyChatList(username, chat);
        myChatListClone.transform.SetParent(contents);
        myChatListClone.SetActive(true);
        chatLineControl.SetChatLine(myChatListClone, false, false);
        ChatDestroyer();
    }

    public void SpawnLocalChatList(string username, string chat)
    {
        GameObject localChatListClone = GameObject.Instantiate(localChatListPrefab, localChatListPrefab.transform.position, Quaternion.identity);
        LocalChatList localChatList = localChatListClone.GetComponent<LocalChatList>();
        localChatList.SetLocalChatList(username, chat);
        localChatListClone.transform.SetParent(contents);
        localChatListClone.SetActive(true);
        chatLineControl.SetChatLine(localChatListClone, true, false);
        ChatDestroyer();
    }

    public void SpawnMyChatList_Whisper(string username, string chat)
    {
        GameObject myChatListClone = GameObject.Instantiate(whisperChatListPrefab_My, whisperChatListPrefab_My.transform.position, Quaternion.identity);
        MyChatList myChatList = myChatListClone.GetComponent<MyChatList>();
        myChatList.SetMyChatList(username, chat);
        myChatListClone.transform.SetParent(contents);
        myChatListClone.SetActive(true);
        chatLineControl.SetChatLine(myChatListClone, false, true);
        ChatDestroyer();
    }

    public void SpawnLocalChatList_Whisper(string username, string chat)
    {
        GameObject localChatListClone = GameObject.Instantiate(whisperChatListPrefab_Local, whisperChatListPrefab_Local.transform.position, Quaternion.identity);
        LocalChatList localChatList = localChatListClone.GetComponent<LocalChatList>();
        localChatList.SetLocalChatList(username, chat);
        localChatListClone.transform.SetParent(contents);
        localChatListClone.SetActive(true);
        chatLineControl.SetChatLine(localChatListClone, true, true);
        ChatDestroyer();
    }

}
