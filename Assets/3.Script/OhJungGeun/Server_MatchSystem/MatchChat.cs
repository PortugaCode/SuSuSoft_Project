using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchChat : MonoBehaviour
{
    [SerializeField] private GameObject chatBox;
    [SerializeField] private TextMeshPro chatInGame;

    [SerializeField] private SpriteRenderer chatBoxRenderer;
    [SerializeField] private TextMeshPro nickName;

    private Coroutine chat;

    public void SetChatOrder(int a, int b)
    {
        chatBoxRenderer.sortingOrder = a;
        chatInGame.sortingOrder = b;
    }

    public void SetChatBox(GameObject chatBox)
    {
        this.chatBox = chatBox;
        chatInGame = chatBox.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();

        chatBoxRenderer = chatBox.GetComponent<SpriteRenderer>();
    }

    public void SetNickName(TextMeshPro nickName)
    {
        this.nickName = nickName;
    }

    public void DestroyChatBox()
    {
        Destroy(chatBox);
        Destroy(nickName.gameObject);
    }

    private void Update()
    {
        chatBox.transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
        nickName.transform.position = new Vector2(transform.position.x, transform.position.y - 0.5f);
    }

    public void SetNickName(string name)
    {
        nickName.text = name;
    }


    public void SetChatInGame(string chat)
    {
        chatInGame.text = chat;

        if(this.chat != null)
        {
            StopCoroutine(this.chat);
        }
        this.chat = StartCoroutine(ChatBoxActive_Co());
    }


    private IEnumerator ChatBoxActive_Co()
    {
        chatBox.SetActive(true);
        yield return new WaitForSeconds(7f);
        chatBox.SetActive(false);
    }
}
