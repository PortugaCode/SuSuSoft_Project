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

    private void Start()
    {
        PlayerProperty player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerProperty>();
        boss = player.Boss;

        bossControll = boss.transform;
    }

    private void Update()
    {
        AttackStar();
    }

    public void AttackStar()
    {
        if (isMoveOn && Utils.Instance.isBossStage)
        {
            transform.position = Vector3.Slerp(this.transform.position, bossControll.position, speed * Time.deltaTime);
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
        else if(collision.CompareTag("Boss") && isMoveOn && Utils.Instance.isBossStage)
        {
            Destroy(gameObject);
            boss.animator.SetTrigger("BossHurt");
            boss.BossDamage();
            onBossHPSlide?.Invoke(this, EventArgs.Empty);
        }
    }
}
