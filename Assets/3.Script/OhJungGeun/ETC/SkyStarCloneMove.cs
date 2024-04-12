using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyStarCloneMove : MonoBehaviour
{
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private bool isRightClone;


    // 매칭 서버 별자리 이동 메서드
    private void CloneMove()
    {
        if (isRightClone)
        {
            transform.position = new Vector2(targetPlayer.position.x + 30.00733f, targetPlayer.position.y);
        }
        else
        {
            transform.position = new Vector2(targetPlayer.position.x - 30.00733f, targetPlayer.position.y);
        }
    }
}
