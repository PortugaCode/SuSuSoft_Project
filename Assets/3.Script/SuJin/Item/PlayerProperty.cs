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
    public int damage;
    private int HealthIncreaseRate;   //+

    //Attack nullified 공격 무효화
    public float ignoreAttack = 0.1f;
    //private bool GodMode = false;

    [Header("Giant")]
    [SerializeField] private GameObject GiantFace;
    [SerializeField] private int sizeDuration = 3; //크기 유지 시간
    public float maxScale = 0.6f; // 최대 크기
    public float minScale = 0.1f; // 최소 크기
    private float originalScale = 0.3f;
    private float scaleSpeed = 1f; // 초당 크기 증가량

    private bool isGiant;
    private bool isSmaller;


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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //장애물
        if(collision.gameObject.CompareTag("Obstacles"))      
        {
            //Player Damage
            PassiveAttackNull();
            Destroy(collision.gameObject);
            Debug.Log($" currentHP: {currentHealth} ");
        }

        if(collision.gameObject.CompareTag("Breaking"))
        {
            Destroy(collision.gameObject);
        }
        
        if(collision.gameObject.CompareTag("Wall"))
        {
            PassiveAttackNull();
            Debug.Log($" currentHP: {currentHealth} ");
        }

        //HP
        if(collision.gameObject.CompareTag("HPItem"))
        {
            currentHealth += damage;
            Destroy(collision.gameObject);
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
                // transform.localScale = new Vector2(player.localScale.x * 2f, player.localScale.y * 2f);
                // ignoreAttack = 1f;
                StartCoroutine(Giant_Co());
                Destroy(collision.gameObject);
            }

            //Smaller
            isSmaller = collision.gameObject.name.Contains("Smaller");

            if(isSmaller)
            {
                //transform.localScale = new Vector2(player.localScale.x / (float) 1.3f, player.localScale.y / (float)1.3f);
                StartCoroutine(Smaller_Co());
                Destroy(collision.gameObject);
            }
        }

        //Token
        if(collision.gameObject.CompareTag("Token"))
        {
            Destroy(collision.gameObject);
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
    

    IEnumerator Giant_Co()
    {
        while(transform.localScale.x < maxScale)
        {
            Vector3 newScale = transform.localScale;
            newScale.x += scaleSpeed * Time.deltaTime;
            newScale.y += scaleSpeed * Time.deltaTime;
            transform.localScale = newScale;

            yield return null;
        }
        StartCoroutine(OriginalSize_Co());
    }

    private IEnumerator Smaller_Co()
    {
        while(transform.localScale.x > minScale)
        {
            Debug.Log($"{player.localScale.x} + {player.localScale.y}");
            Vector3 minScale = transform.localScale;
            minScale.x -= scaleSpeed * Time.deltaTime;
            minScale.y -= scaleSpeed * Time.deltaTime;
            transform.localScale = minScale;

            yield return null;
        }
        StartCoroutine(OriginalSize_Co());
    }

    private IEnumerator OriginalSize_Co()
    {
        yield return new WaitForSeconds(sizeDuration);

        while(transform.localScale.x > originalScale)
        {
            Vector3 oriScale = transform.localScale;
            oriScale.x -= scaleSpeed * Time.deltaTime;
            oriScale.y -= scaleSpeed * Time.deltaTime;
            transform.localScale = oriScale;

            yield return null;
        }
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
