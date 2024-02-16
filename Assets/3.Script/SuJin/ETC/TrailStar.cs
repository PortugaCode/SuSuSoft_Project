using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailStar : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float fllowSpeed;
    private float distance;

    private Vector3 previousStarPosition;
    [SerializeField] private GameObject target;

    PlayerProperty playerProperty;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerProperty = player.GetComponent<PlayerProperty>();
    }

    private void Start()
    {
        SetTarget();
    }

    private void FixedUpdate()
    {
        ChasePlayer();
    }

    private void ChasePlayer()
    {
        transform.position = Vector2.Lerp(this.transform.position, target.transform.position + Vector3.down * 0.5f, fllowSpeed * Time.deltaTime);
    }

    private void Update()
    {
        ItemRotate();
    }
    
    public void SetTarget()   // Follow Player
    {
        //transform.position = Vector2.Lerp(this.transform.position, player.transform.position + Vector3.down * 0.8f, fllowSpeed * Time.deltaTime);
        if(playerProperty.stars.Count <= 0)
        {
            playerProperty.stars.Add(this.gameObject);
            target = player;
        }
        else
        {
            target = playerProperty.stars[playerProperty.stars.Count - 1];
            playerProperty.stars.Add(this.gameObject);
        }
    }

    private void ItemRotate()   // Rotation Star
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = (player.transform.position - transform.position).normalized;

        float directionZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, directionZ - 90f);
    }
}
