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

    [SerializeField] private GameObject GiantFace;

    [Header("HP")]
    public int currentHealth;
    public int maxHealth;
    public int damage;
    private int HealthIncreaseRate;   //+

    //Attack nullified 공격 무효화
    public float ignoreAttack = 0.1f;
    private bool GodMode = false;

    [Header("Giant")]
    private Vector2 originalSize;
    private bool isGiant;
    private bool isSmaller;
    [SerializeField] private int sizeDuration = 3;



    #region [나중에 구현]
    /*[Header("Magnetism")]
    public float magneticTime = 10f;
    public float maxMagnetismRange = 30f;
    public float minMagnetismRange;


    [Header("Sight")]
    public int maxSightRange;
    public int minSightRange;*/
    #endregion

    [Header("GetStars")]
    public List<GameObject> stars = new List<GameObject>();
    Vector3 starsScale;

    [SerializeField] int getStarCount;
    [SerializeField] GameObject starPrefebs;
    //private float getStarPercent;

    [Header("Skill")]
    [SerializeField]private float maxCoolTime = 30;   //현재 남은 시간 
    private float leftcoolTime;   // 쿨타임 남은 시간

    private bool coolGiant;

    private void Start()
    {
        level = 1;
        currentHealth = maxHealth;
        originalSize = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //장애물
        if(collision.gameObject.CompareTag("Obstacles"))      
        {
            //Player Damage
            PassiveAttackNull();
            Debug.Log($" currentHP: {currentHealth} ");
        }

        //별
        if(collision.gameObject.CompareTag("Star"))
        {
            getStarCount++;
            Instantiate(starPrefebs, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Debug.Log($" getStarCount : {getStarCount}");
        }

        //거대화, 최소화
        if(collision.gameObject.CompareTag("Item"))
        {
            //Giant
            isGiant = collision.gameObject.name.Contains("Giant");

            if(isGiant)
            {
                transform.localScale = new Vector2(player.localScale.x * 2f, player.localScale.y * 2f);
               /* foreach(GameObject obj in stars)
                {
                    Transform sss  = obj.transform;
                    sss.localScale = starsScale * 2f;
                }*/

                ignoreAttack = 1f;
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


    #region [Attack nullified 공격 무효화]
    private void PassiveAttackNull()    //10% 확률로 데미지 무효화
    {
        float randomoValue = Random.value;

        if (randomoValue >= ignoreAttack)
        {
            SetDamage();
        }
    }

    private void SetDamage()     //Damage 입는 메서드
    {
        currentHealth -= damage;
    }
    #endregion //Attack nullified 공격 무효화


    #region  [IEnumerator]
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

    IEnumerator CoolTime_Co()
    {
        while(leftcoolTime > 1)
        {
            maxCoolTime -= Time.deltaTime;
            leftcoolTime = leftcoolTime / maxCoolTime;

            Debug.Log($"{leftcoolTime} : ");

            yield return new WaitForFixedUpdate();
        }
    }
    #endregion
}
