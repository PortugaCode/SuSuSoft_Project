using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerProperty : MonoBehaviour
{
    public EventHandler onHPSlider;
    public EventHandler onGetHealthSlider;
    public EventHandler onStarBar;
    public EventHandler onStarShape;
    public EventHandler onDamage;
    public EventHandler onChangeStar;

    public int level;

    [Header("Particle")]
    [SerializeField] private ParticleSystem hitAction;
    [SerializeField] private ParticleSystem dieAction;


    [Header("Player")]
    [SerializeField] private Transform player;
    public string PlayerColor;
    public int activeSkill;
    public int passiveSkill;

    [Header("HP")]
    public int currentHealth;
    public int maxHealth;
    public int damage;
    //private int HealthIncreaseRate;   //+

    private bool isCanShield;

    //Attack nullified 공격 무효화
    public float ignoreAttack = 0.1f;
    public SkillActive skillActive;
    private HorizontalPlayer horizontalPlayer;

    [Header("Giant")]
    [SerializeField] private GameObject GiantFace;
    [SerializeField] private int sizeDuration = 3;  //크기 유지 시간
    public float maxScale = 0.6f;                   // 최대 크기
    public float minScale = 0.1f;                   // 최소 크기
    private float originalScale = 0.3f;
    private float scaleSpeed = 1f;                  // 초당 크기 증가량

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
    public int maxStar;
    public List<GameObject> stars = new List<GameObject>();
   // Vector3 starsScale;

    public int getStarCount;
    [SerializeField] GameObject starPrefebs;

    [Header("Skill")]
    [SerializeField] GameObject ShieldOn;
    [SerializeField] float skillDuration;
    //private bool coolGiant;                             // 자이언트 쿨타임
    

    [Header("Animator")]
    [SerializeField] private Animator animator;


    private bool canHit = true;
    private float hitTimer = 1f;


    private void Start()
    {
        level = 1;
        currentHealth = maxHealth;
        horizontalPlayer = GetComponent<HorizontalPlayer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //장애물
        if (collision.gameObject.CompareTag("Obstacles") && canHit)
        {
                StartCoroutine(HitDelay_Co());

                //Player Damage
                if(!skillActive.isItemOn)
                {
                    PassiveAttackNull();
                    onHPSlider?.Invoke(this, EventArgs.Empty);
                    onStarBar?.Invoke(this, EventArgs.Empty);
                    onStarShape?.Invoke(this, EventArgs.Empty);
                }
        }

        else if (collision.gameObject.CompareTag("Breaking"))
        {
            animator.SetTrigger("Hit");
        }

        //HP
        else if (collision.gameObject.CompareTag("HPItem"))
        {
            currentHealth += 2;
            if (currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
            }
            onHPSlider?.Invoke(this, EventArgs.Empty);
            Destroy(collision.gameObject);
        }

        else if(collision.gameObject.CompareTag("BigHp"))
        {
            currentHealth += 8;
            if (currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
            }
            onHPSlider?.Invoke(this, EventArgs.Empty);
            Destroy(collision.gameObject);
        }

        //별
        //Pooling 으로 바꾸기
        else if (collision.gameObject.CompareTag("Star"))
        {
            Instantiate(starPrefebs, transform.position, Quaternion.identity);
            onStarBar?.Invoke(this, EventArgs.Empty);
            onStarShape?.Invoke(this, EventArgs.Empty);
            Destroy(collision.gameObject);

        }
        else if (collision.gameObject.CompareTag("BigStar"))
        {
            Instantiate(starPrefebs, transform.position, Quaternion.identity);
            Instantiate(starPrefebs, transform.position, Quaternion.identity);
            Instantiate(starPrefebs, transform.position, Quaternion.identity);
            onStarBar?.Invoke(this, EventArgs.Empty);
            onStarShape?.Invoke(this, EventArgs.Empty);
            Destroy(collision.gameObject);
        }

        //거대화, 최소화
        else if (collision.gameObject.CompareTag("Item"))
        {
            //Giant
            isGiant = collision.gameObject.name.Contains("Giant");

            if (isGiant)
            {
                // transform.localScale = new Vector2(player.localScale.x * 2f, player.localScale.y * 2f);
                // ignoreAttack = 1f;
                StartCoroutine(Giant_Co());
                Destroy(collision.gameObject);
            }

            //Smaller
            isSmaller = collision.gameObject.name.Contains("Smaller");

            if (isSmaller)
            {
                //transform.localScale = new Vector2(player.localScale.x / (float) 1.3f, player.localScale.y / (float)1.3f);
                StartCoroutine(Smaller_Co());
                Destroy(collision.gameObject);
            }
        }

        //Token
        else if (collision.gameObject.CompareTag("Token"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && canHit)
        {
                StartCoroutine(HitDelay_Co());
                PassiveAttackNull();
                onHPSlider?.Invoke(this, EventArgs.Empty);
                onStarBar?.Invoke(this, EventArgs.Empty);
                onStarShape?.Invoke(this, EventArgs.Empty);
        }
    }
   
    #region [Attack nullified 공격 무효화]
    private void PassiveAttackNull()    //10% 확률로 데미지 무효화
    {
        float randomoValue = UnityEngine.Random. value;

        if (randomoValue >= ignoreAttack)
        {
            SetDamage();
        }
    }

    private void SetDamage()     //Damage 입는 메서드
    {
        if(!isCanShield)
        {
            if (stars.Count > 0)
            {
                GameObject a = stars[stars.Count - 1];
                stars.RemoveAt(stars.Count - 1);
                Destroy(a);
            }

            currentHealth -= damage;
            animator.SetTrigger("Hit");
            onDamage?.Invoke(this, EventArgs.Empty);
            onStarBar?.Invoke(this, EventArgs.Empty);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
                return;
            }
        }
        
        hitAction.Play();
    }

    public void ShieldMode()                    // Shield Mode
    {
        skillActive.isItemOn = false;
        isCanShield = true;


        Debug.Log("들어옴");

        if(isCanShield)
        {
            ShieldOn.SetActive(true);
            StartCoroutine(SkillDuration_Co());
        }
    }

    private void Die()
    {
        if(TryGetComponent<HorizontalPlayer>(out HorizontalPlayer horizontalPlayer))
        {
            horizontalPlayer.GameControl.ActiveOnEndUI();
        }
        
        Destroy(gameObject);
        hitAction.gameObject.transform.SetParent(null);
        dieAction.gameObject.transform.SetParent(null);
        hitAction.Play();
        dieAction.Play();
    }

    #endregion //Attack nullified 공격 무효화


    #region  [IEnumerator]


    private IEnumerator HitDelay_Co()
    {
        canHit = false;
        yield return new WaitForSeconds(hitTimer);
        canHit = true;
    }

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

    IEnumerator SkillDuration_Co()      // Skill 끝남
    {
        yield return new WaitForSeconds(skillDuration);
        isCanShield = false;
        ShieldOn.SetActive(false);
        this.horizontalPlayer.coroutine = StartCoroutine(skillActive.CoolTime_Co());
    }


    #endregion
}
