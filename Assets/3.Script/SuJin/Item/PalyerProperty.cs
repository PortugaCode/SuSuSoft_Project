using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalyerProperty : MonoBehaviour
{
    public int level;

    [Header("Player")]
    [SerializeField] private string playerName;
    public string PlayerColor;
    public int activeSkill;
    public int passiveSkill;

    [Header("HP")]
    public int currentHealth;
    public int maxHealth;
    private int damage = 0;
    public int HealthIncreaseRate = 10;

    [Header("Skill")]
    //private int maxCoolTime;

    [Header("Speed")]
    [SerializeField] GameObject speedItem;
    public int maxSpeed;
    public int minSpeed;

    [Header("Sight")]
    public int maxSightRange;
    public int minSightRange;

    [Header("Magnetism")]
    public float maxMagnetismRange;
    public float minMagnetismRange;

    [Header("Weight")]
    public float maxWeight;
    public float minWeight;

    [Header("Shield")]
    public bool isShield = false;

    [Header("GetStars")]
    [SerializeField] GameObject star;
    [SerializeField] int getStarCount;
    //private float getStarPercent;

    // 공격 무효

    HorizontalPlayer horizontalPlayer;

    private void Start()
    {
        level = 1;
        maxSpeed = 300;
        maxHealth = 100;
        //maxCoolTime = 30;

        maxSightRange = 300;
        maxMagnetismRange = 10;
        maxWeight = 10;
        getStarCount = 0;

        horizontalPlayer = GetComponent<HorizontalPlayer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Obstacles"))      
        {
            //Player Damage
            damage = 10; 
            currentHealth = maxHealth - damage;

            Debug.Log($" currentHP: {currentHealth} ");
        }

        if(collision.gameObject.CompareTag("Star"))
        {
            getStarCount++;
            Instantiate(star, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Debug.Log($" getStarCount : {getStarCount}");
        }
        if(collision.gameObject.CompareTag("SpeedItem"))
        {
            //horizontalPlayer  참조하지 말고 이 스크립트에서 관리하기
            Destroy(collision.gameObject);
           //horizontalPlayer. =  horizontalPlayer.maxSpeed++;
        }
    }

    private void PassiveSkill()
    {
        //
    }

}
