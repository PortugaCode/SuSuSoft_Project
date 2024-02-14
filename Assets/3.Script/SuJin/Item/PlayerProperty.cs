using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperty : MonoBehaviour
{
    public int level;

    [Header("Player")]
    [SerializeField] private Transform player;
    public string PlayerColor;
    public int activeSkill;
    public int passiveSkill;

    [Header("HP")]
    public int currentHealth;
    public int maxHealth;
    private int damage = 0;
    public int HealthIncreaseRate = 10;

    [Header("Giant")]
    private Vector2 originalSize;
    private bool isGiant;
    [SerializeField] private int sizeDuration = 3;
    public float bigSpeed; //커질 때의 속도

    private bool isSmaller;



    #region [나중에 구현]
    /*[Header("Magnetism")]
    public float magneticTime = 10f;
    public float maxMagnetismRange = 30f;
    public float minMagnetismRange;

    [Header("Skill")]
    //private int maxCoolTime;

    [Header("Sight")]
    public int maxSightRange;
    public int minSightRange;*/
    #endregion

    [Header("GetStars")]
    [SerializeField] GameObject[] stars;
    [SerializeField] int getStarCount;
    //private float getStarPercent;

    private void Start()
    {
        level = 1;
        maxHealth = 100;

        originalSize = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Obstacles"))      
        {
            //Player Damage
            damage = 10; 
            currentHealth -= damage;

            Debug.Log($" currentHP: {currentHealth} ");
        }
        if(collision.gameObject.CompareTag("Star"))
        {
            getStarCount++;
            Instantiate(stars[0], transform.position, Quaternion.identity);     
            Destroy(collision.gameObject);
            Debug.Log($" getStarCount : {getStarCount}");
        }
        if(collision.gameObject.CompareTag("Item"))
        {
            //Giant
            isGiant = collision.gameObject.name.Contains("Giant");

            if(isGiant)
            {
                transform.localScale = new Vector2(player.localScale.x * 2f, player.localScale.y * 2f);
                Destroy(collision.gameObject);
                StartCoroutine(Giant_Co());
            }

            //Smaller
            isSmaller = collision.gameObject.name.Contains("Smaller");

            if(isSmaller)
            {
                transform.localScale = new Vector2(player.localScale.x / (float) 1.3f, player.localScale.y / (float)1.3f);
                Destroy(collision.gameObject);
                StartCoroutine(Smaller_Co());
            }
        }
    }

    private IEnumerator Giant_Co()
    {
        yield return new WaitForSeconds(sizeDuration);
        isGiant = false;
        transform.localScale = new Vector2(player.localScale.x /2f, player.localScale.y /2f);
    }

    private IEnumerator Smaller_Co()
    {
        yield return new WaitForSeconds(sizeDuration);
        isSmaller = false;
        transform.localScale = new Vector2(player.localScale.x * (float)1.3f, player.localScale.y * (float) 1.3f);
    }

}
