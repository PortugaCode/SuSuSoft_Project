using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explodable))]
public class MoveRockObstacle : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private ItemEventControl itemEventControl;
    [SerializeField] private ItemEventControl itemEventControl_3;

    //Elploable
    Explodable explodable;

    private bool nowDestroy = false;
    public bool isGreen = false;

    private void Start()
    {
        explodable = GetComponent<Explodable>();



        if (Utils.Instance.currentLevel == Level.Level_5) return;   //Level 예외 처리 해주기
        if (!Utils.Instance.isBossStage)
        {
            //itemEventControl.onItemEquip += OnSimulation;       //메서드를 여러 개 담을 때 +=   //단일 매서드는 =  //  Destroy 될 때는 -= 로 구독해지 해주기

            if (itemEventControl != null)
            {
                itemEventControl.onItemEquip += OnSimulation;
            }
            if (itemEventControl_3 != null)
            {
                itemEventControl_3.onItemEquip += OnSimulation;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !nowDestroy)
        {
            Onexploable();

            //Audio
            AudioManager.Instance.PlaySFX(SFX_Name.Crash1);
        }
    }

    public void OnSimulation(object sender, EventArgs args)
    {
        rigid.simulated = true;
    }

    public void Onexploable()
    {
        nowDestroy = true;
        explodable.explode();
        ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
        ef.doExplosion(transform.position);
    }

}
