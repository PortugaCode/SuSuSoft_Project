using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchChat : MonoBehaviour
{
    [SerializeField] private GameObject chatBox;
    [SerializeField] private TextMeshPro chatInGame;

    [SerializeField] private SpriteRenderer chatBoxRenderer;


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

    public void DestroyChatBox()
    {
        Destroy(chatBox);
    }

    private void Update()
    {
        chatBox.transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
    }


    public void SetChatInGame(string chat)
    {
        chatInGame.text = chat;

        StopCoroutine(ChatBoxActive_Co());
        StartCoroutine(ChatBoxActive_Co());
    }


    private IEnumerator ChatBoxActive_Co()
    {
        chatBox.SetActive(true);
        yield return new WaitForSeconds(7f);
        chatBox.SetActive(false);
    }
}
