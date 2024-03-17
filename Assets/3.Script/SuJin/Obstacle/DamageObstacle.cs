using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageObstacle : MonoBehaviour
{
    [SerializeField] private float speed;

    BossControll boss;

    public EventHandler onBossHPSlide;

    private void Awake()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossControll>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            boss.animator.SetTrigger("BossHurt");
            boss.OtherDamage();
            onBossHPSlide?.Invoke(this, EventArgs.Empty);
        }
    }
}
