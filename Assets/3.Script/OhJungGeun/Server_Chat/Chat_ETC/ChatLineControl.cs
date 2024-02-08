using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatLineControl : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private GameObject chatUI;

    [SerializeField] private LocalChatList localChatList;
    [SerializeField] private LocalChatList localChatList_Whisper;

    [SerializeField] private MyChatList myChatList;
    [SerializeField] private MyChatList myChatList_Whisper;

    public GameObject GetChatUI()
    {
        return chatUI;
    }

    public void ResetChatUI()
    {
        localChatList.gameObject.SetActive(false);
        localChatList_Whisper.gameObject.SetActive(false);
        myChatList.gameObject.SetActive(false);
        myChatList_Whisper.gameObject.SetActive(false);
    }


    //한 줄씩 뜨게하는 메서드
    public void SetChatLine(GameObject a, bool isLocal, bool isWhisper)
    {
        Debug.Log("SetChatLine");
        ResetChatUI();

        if (isLocal)
        {
            LocalChatList localChatList;
            a.TryGetComponent<LocalChatList>(out localChatList);
            if (isWhisper)
            {
                this.localChatList_Whisper.SetLocalChatList(localChatList.GetUserName(), localChatList.GetUserSpeakText());
                this.localChatList_Whisper.gameObject.SetActive(true);
            }
            else
            {
                this.localChatList.SetLocalChatList(localChatList.GetUserName(), localChatList.GetUserSpeakText());
                this.localChatList.gameObject.SetActive(true);
            }
        }
        else
        {
            MyChatList myChatList;
            a.TryGetComponent<MyChatList>(out myChatList);

            if(isWhisper)
            {
                this.myChatList_Whisper.SetMyChatList(myChatList.GetUserName(), myChatList.GetUserSpeakText());
                this.myChatList_Whisper.gameObject.SetActive(true);
            }
            else
            {
                this.myChatList.SetMyChatList(myChatList.GetUserName(), myChatList.GetUserSpeakText());
                this.myChatList.gameObject.SetActive(true);
            }
        }

        #region [Past Code]
        /*        GameObject clone = GameObject.Instantiate(a, a.transform.position, Quaternion.identity);
                clone.transform.SetParent(pivot);
                RectTransform cloneRect = clone.GetComponent<RectTransform>();

                cloneRect.anchoredPosition = new Vector2(500, 0);
                cloneRect.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);*/
        #endregion
    }
}
