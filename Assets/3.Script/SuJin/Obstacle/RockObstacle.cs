using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockObstacle : MonoBehaviour
{
    Explodable explodable;
    PolygonCollider2D polygonCollider2D;

    private void Start()
    {
        explodable = GetComponent<Explodable>();
        //gameObject.GetComponent<PolygonCollider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            OnExplode();
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        }
    }

    public void OnExplode()
    {
        explodable.explode();

        ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
        ef.doExplosion(transform.position);
    }

}
