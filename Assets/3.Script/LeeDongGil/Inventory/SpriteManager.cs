using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance = null;

    [Header("Image Index  == SpriteManager List Index")]
    public List<Sprite> sprites = new List<Sprite>();

    [Header("Stage Index  == SpriteManager List Index")]
    public List<Sprite> tokenSprites = new List<Sprite>();


    [Header("StarTail Index  == SpriteManager List Index")]
    public List<Sprite> startailes = new List<Sprite>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
