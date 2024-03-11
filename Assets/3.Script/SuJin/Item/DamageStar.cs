using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageStar : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private float speed;
    private bool isMoveOn = false;


    private void Awake()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
    }

    private void Update()
    {
        if(isMoveOn)
        {
            transform.position = Vector2.Lerp(this.transform.position, boss.transform.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //별이 플레이어에 닿았을 때 
        if (collision.CompareTag("Player"))    //&& Utils.Instance.currentLevel == Level.Level_5 
        {
            Debug.Log("OnTriggerEnter2D");
            isMoveOn = true;
           
        }
    }
}
