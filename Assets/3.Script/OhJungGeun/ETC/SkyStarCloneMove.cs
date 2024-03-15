using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyStarCloneMove : MonoBehaviour
{
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private bool isRightClone;

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
