using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System;

public class CurrentPlayerSprite : MonoBehaviour
{
    [Header("Body & Face Sprite")]
    [SerializeField] private Sprite[] bodies;
    [SerializeField] private Sprite[] basefaces;
    [SerializeField] private Sprite[] blinkfaces;
    [SerializeField] private Sprite[] slowfaces;
    [SerializeField] private Sprite[] poisonfaces;
    [SerializeField] private Sprite[] giantfaces;
    [SerializeField] private Sprite[] hurtfaces;

    [Header("User Data")]
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private SpriteRenderer baseface;
    [SerializeField] private SpriteRenderer blinkface;
    [SerializeField] private SpriteRenderer slowface;
    [SerializeField] private SpriteRenderer poisonface;
    [SerializeField] private SpriteRenderer giantface;
    [SerializeField] private SpriteRenderer hurtface;


    private void Awake()
    {
        SetUserCharacter();
    }

    private void Start()
    {
        UIManager.OnChangeCurrentCharacter += ChangeCurrentCharacter;
    }

    private void ChangeCurrentCharacter(object sender, EventArgs e)
    {
        Debug.Log("현재 캐릭터 바뀜");
        SetUserCharacter();
    }

    public void SetUserCharacter()
    {
        int index = DBManager.instance.user.currentCharacterIndex;
        body.sprite = bodies[index];
        baseface.sprite = basefaces[(int)(index / 4)];
        blinkface.sprite = blinkfaces[(int)(index / 4)];
        slowface.sprite = slowfaces[(int)(index / 4)];
        poisonface.sprite = poisonfaces[(int)(index / 4)];
        giantface.sprite = giantfaces[(int)(index / 4)];
        hurtface.sprite = hurtfaces[(int)(index / 4)];
    }

    public void SetUserCharacter(string name)
    {
        var n_bro = Backend.Social.GetUserInfoByNickName(name);
        string n_inDate = n_bro.GetReturnValuetoJSON()["row"]["inDate"].ToString();

        var bro = Backend.PlayerData.GetOtherData("User", n_inDate);
        int index = int.Parse(bro.GetReturnValuetoJSON()["rows"][0]["CurrentCharacterIndex"][0].ToString());
        body.sprite = bodies[index];
        baseface.sprite = basefaces[(int)(index / 4)];
        blinkface.sprite = blinkfaces[(int)(index / 4)];
        slowface.sprite = slowfaces[(int)(index / 4)];
        poisonface.sprite = poisonfaces[(int)(index / 4)];
        giantface.sprite = giantfaces[(int)(index / 4)];
        hurtface.sprite = hurtfaces[(int)(index / 4)];
    }

    private void OnDestroy()
    {
        UIManager.OnChangeCurrentCharacter -= ChangeCurrentCharacter;
    }
}
