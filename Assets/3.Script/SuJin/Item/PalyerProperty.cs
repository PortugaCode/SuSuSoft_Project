using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalyerProperty : MonoBehaviour
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

    [Header("Magnetism")]
    public float magneticTime = 10f;
    public float maxMagnetismRange = 30f;
    public float minMagnetismRange;

    [Header("Skill")]
    //private int maxCoolTime;

    [Header("Sight")]
    public int maxSightRange;
    public int minSightRange;

    [Header("GetStars")]
    [SerializeField] GameObject[] stars;
    [SerializeField] int getStarCount;
    //private float getStarPercent;

    private void Start()
    {
        level = 1;
        maxHealth = 100;
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
    }

    //플레이어는 패시브로 자력을 가지고 있음

    /*
    1. 아이템 태그를 끌어오는 코드 
    2. maxRange를 설정해서 자력범위를 정해주고, 자력 유지 시간 정하기

     */
}
