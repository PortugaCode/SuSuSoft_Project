using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarTailSpriteSet : MonoBehaviour
{
    [SerializeField] private Sprite tailSprite;


    private void Awake()
    {
        SetSprite();
    }

    private void SetSprite()
    {
        int index = DBManager.instance.user.currentTailIndex;

        tailSprite = SpriteManager.instance.startailes[index];
    }
}
