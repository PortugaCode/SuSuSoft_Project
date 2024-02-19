using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRockObstacle : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private ItemEventControl itemEventControl;

    private void Start()
    {
        itemEventControl.onItemEquip += OnSimulation;
    }

    public void OnSimulation(object sender, EventArgs args)
    {
        rigid.simulated = true;
    }
}
