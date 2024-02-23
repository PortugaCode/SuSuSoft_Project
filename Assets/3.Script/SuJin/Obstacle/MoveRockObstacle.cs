using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explodable))]
public class MoveRockObstacle : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private ItemEventControl itemEventControl;

    //Elploable
     Explodable explodable;

    private bool nowDestroy = false;

    private void Start()
    {
        explodable = GetComponent<Explodable>();
        itemEventControl.onItemEquip += OnSimulation;       //메서드를 여러 개 담을 때 +=   //단일 매서드는 =  //  Destroy 될 때는 -= 로 구독해지 해주기
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !nowDestroy)
        {
            Onexploable();
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
