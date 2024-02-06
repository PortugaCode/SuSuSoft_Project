using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailItem : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float fllowSpeed;

    private float distance;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        ChasePlayer();
    }

    private void Update()
    {
        ItemRotate();
    }

    private void ChasePlayer()
    {
        transform.position = Vector2.Lerp(this.transform.position, player.transform.position + Vector3.down * 0.8f, fllowSpeed * Time.deltaTime);
    }

    private void ItemRotate()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = (player.transform.position - transform.position).normalized;

        float directionZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, directionZ - 90f);
    }
}
