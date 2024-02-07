using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalyerProperty : MonoBehaviour
{
    private float originalPlayer;
    [SerializeField] Rigidbody2D rigidbody2D;

    private int hp;
    [SerializeField] private int sightRange;     //시야범위
    private bool isMagnetism;     //자성
    private bool isGiant;
    //+speed

    private void Start()
    {
        hp = 100;
        sightRange = 10;
        isMagnetism = false;
        isGiant = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Item"))
        {
            isGiant = collision.gameObject.name.Contains("Gient");

            if(isGiant)
            {
                //Face SetActive Add

                transform.localScale = new Vector2(originalPlayer * 3f, originalPlayer * 3f);

            }
        }
    }

    private IEnumerator Giant_Co(float delay)
    {
        yield return new WaitForSeconds(delay);
        //ri
    }
}
