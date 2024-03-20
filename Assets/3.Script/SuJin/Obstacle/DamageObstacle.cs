using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageObstacle : MonoBehaviour
{
    [SerializeField] private float speed;

    BossControll boss;

    public EventHandler onBossHPSlide;


    private void Start()
    {
        PlayerProperty player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerProperty>();
        boss = player.Boss;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            int index = DBManager.instance.user.currentCharacterIndex; // 0~19    0~4
            if ((int)(index / 4) == 4)
            {
                Debug.Log("옵스타클 보스 공격");
                boss.animator.SetTrigger("BossHurt");
                boss.OtherDamage();
                onBossHPSlide?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
