using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageStar : MonoBehaviour
{
    [SerializeField] private Transform bossControll;
    [SerializeField] private float speed;
    private bool isMoveOn = false;

    public EventHandler onBossHPSlide;
    BossControll boss;

    private void Awake()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossControll>();
    }

    private void FixedUpdate()
    {
        AttackStar();
    }

    public void AttackStar()
    {
        if (isMoveOn)
        {
            transform.position = Vector2.LerpUnclamped(this.transform.position, bossControll.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //별이 플레이어에 닿았을 때 
        if (collision.CompareTag("Player"))    //&& Utils.Instance.currentLevel == Level.Level_5 
        {
            isMoveOn = true;
            gameObject.layer = 0;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else if(collision.CompareTag("Boss") && isMoveOn)
        {
            Destroy(gameObject);
            boss.animator.SetTrigger("BossHurt");
            boss.BossDamage();
            onBossHPSlide?.Invoke(this, EventArgs.Empty);
        }
    }
}
