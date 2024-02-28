using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchClonePlayer : MonoBehaviour
{
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private bool isRightClone;
    [SerializeField] private MatchChat matchChat;

    private bool isCanMove;


    private void Update()
    {
        CloneMove();
    }


    private void CloneMove()
    {
        if (!isCanMove) return;

        if(isRightClone)
        {
            transform.position = new Vector2(targetPlayer.position.x + 25.6f, targetPlayer.position.y);
            transform.rotation = targetPlayer.rotation;
        }
        else
        {
            transform.position = new Vector2(targetPlayer.position.x - 25.6f, targetPlayer.position.y);
            transform.rotation = targetPlayer.rotation;
        }
    }

    public void SetTargetPlayer(Transform target, bool isRightClone)
    {
        targetPlayer = target;
        this.isRightClone = isRightClone;
        isCanMove = true;

        targetPlayer.gameObject.GetComponent<MatchChat>().DoChat += matchChat.SetChatInGame;
        targetPlayer.gameObject.GetComponent<MatchChat>().DoDie += DoDie;
    }

    private void DoDie()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Utils.Instance.nowScene == SceneNames.MatchRoom)
        {
            matchChat.DestroyChatBox();
        }
    }
}
