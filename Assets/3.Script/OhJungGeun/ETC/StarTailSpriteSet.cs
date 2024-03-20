using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarTailSpriteSet : MonoBehaviour
{
    [SerializeField] private SpriteRenderer tailSprite;


    private void Awake()
    {
        SetSprite();
    }

    private void SetSprite()
    {
        int index = DBManager.instance.user.currentTailIndex;

        tailSprite.sprite = SpriteManager.instance.startailes[index];
    }
}
