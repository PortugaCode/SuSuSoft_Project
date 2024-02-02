using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class ChatListManager : MonoBehaviour
{
    //�� ���� �ڽ����� ��ǳ�� ����
    public Transform contents;


    public GameObject myChatListPrefab;
    public GameObject localChatListPrefab;
    public GameObject whisperChatListPrefab;
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
}
