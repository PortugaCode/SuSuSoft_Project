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

    //참조
    public SkillActive skillActive;
    private HorizontalPlayer horizontalPlayer;
    public Magnetic magnetic;

    public int level;

    [Header("Particle")]
    [SerializeField] private ParticleSystem hitAction;
    [SerializeField] private ParticleSystem dieAction;


    [Header("Player")]
    [SerializeField] private Transform player;
    public string PlayerColor;
    //public int activeSkill;
    //public int passiveSkill;

    [Header("HP")]
    public int currentHealth;
    public int maxHealth;
    public int damage;
    //private int HealthIncreaseRate;   //+

    //private bool isCanShield;
    public bool isCanSkill;

    //Attack nullified 공격 무효화
    public float ignoreAttack = 0.1f;

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
    [SerializeField] GameObject MagneticOn;
    [SerializeField] GameObject RecoveryOn;
    [SerializeField] GameObject SpeedUpOn;
    [SerializeField] float skillDuration;
    //private bool coolGiant;                             // 자이언트 쿨타임
    

    [Header("Animator")]
    [SerializeField] private Animator animator;

    private bool isCanHit = true;
    private float hitTimer = 1f;


    //Active Skills
    public enum PlayerActiveSkill { Shield, Magnetic, Recovery, SpeedUp }
    public PlayerActiveSkill playerActiveSkill;



    private void Start()
    {
        level = 1;
        currentHealth = maxHealth;
        horizontalPlayer = GetComponent<HorizontalPlayer>();
    }

    #region [OnTrigger]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //장애물
        if (collision.gameObject.CompareTag("Obstacles") && isCanHit)
        {
            //Audio
            AudioManager.Instance.PlaySFX(SFX_Name.Crash1);

            StartCoroutine(HitDelay_Co());

            PassiveAttackNull();
            onHPSlider?.Invoke(this, EventArgs.Empty);
            onStarBar?.Invoke(this, EventArgs.Empty);
            onStarShape?.Invoke(this, EventArgs.Empty);

            //Player Damage
            if (!skillActive.isItemOn)
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

            //Audio
            AudioManager.Instance.PlaySFX(SFX_Name.Crash1);
        }

        //HP
        else if (collision.gameObject.CompareTag("HPItem"))
        {
            //Audio
            AudioManager.Instance.PlaySFX(SFX_Name.GetHeart);

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
            //Audio
            AudioManager.Instance.PlaySFX(SFX_Name.GetHeart);

            currentHealth += 8;
            if (currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
            }
            onHPSlider?.Invoke(this, EventArgs.Empty);
            Destroy(collision.gameObject);
        }

        //별
        else if (collision.gameObject.CompareTag("Star"))
        {
            //Audio
            AudioManager.Instance.PlaySFX(SFX_Name.GetStar);

            Instantiate(starPrefebs, transform.position, Quaternion.identity);
            onStarBar?.Invoke(this, EventArgs.Empty);
            onStarShape?.Invoke(this, EventArgs.Empty);
            Destroy(collision.gameObject);

        }
        else if (collision.gameObject.CompareTag("BigStar"))
        {
            //Audio
            AudioManager.Instance.PlaySFX(SFX_Name.GetStar);

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
                AudioManager.Instance.PlaySFX(SFX_Name.BigBuff);

                StartCoroutine(Giant_Co());
                Destroy(collision.gameObject);
            }

            //Smaller
            isSmaller = collision.gameObject.name.Contains("Smaller");

            if (isSmaller)
            {
                AudioManager.Instance.PlaySFX(SFX_Name.SmallBuff);
                StartCoroutine(Smaller_Co());
                Destroy(collision.gameObject);
            }
        }

        //Token
        else if (collision.gameObject.CompareTag("Token"))
        {
           //Audio
           AudioManager.Instance.PlaySFX(SFX_Name.GetToken);
           
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && isCanHit)
        {
                //Audio
                AudioManager.Instance.PlaySFX(SFX_Name.Crash1);
                
                StartCoroutine(HitDelay_Co());
                PassiveAttackNull();
                onHPSlider?.Invoke(this, EventArgs.Empty);
                onStarBar?.Invoke(this, EventArgs.Empty);
                onStarShape?.Invoke(this, EventArgs.Empty);
        }
    }

    #endregion
    

    public void SkillActive()
    {
        switch((int)playerActiveSkill)
        {
            case 0 :    //Shield
                {
                    skillDuration = 5f;
                    ShieldMode();
                    ShieldOn.SetActive(true);
                    break;
                }
            case 1:     //Magnetic
                {
                    //Audio
                    AudioManager.Instance.PlaySFX(SFX_Name.SkillSound);

                    skillDuration = 5f;
                    magnetic.ActiveMagnet();
                    MagneticOn.SetActive(true);
                    break;
                }
            case 2:     //Recovery
                {
                    skillDuration = 5f;
                    RecoveryMode();
                    RecoveryOn.SetActive(true);
                    break;
                }
            case 3:     //SpeedUp
                {
                    horizontalPlayer.ActiveSpeedUp();
                    SpeedUpOn.SetActive(true);
                    break;
                }

            default:
                {
                    //모든 조건에 부합하지 않을 시
                    break;
                }
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
        if(!isCanSkill)
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
        isCanSkill = true;

        AudioManager.Instance.PlaySFX(SFX_Name.ShieldBuff);

        if(isCanSkill)
        {
            StartCoroutine(shieldSkillDuration_Co());
        }
    }
    private void RecoveryMode()
    {
        skillActive.isItemOn = false;
        isCanSkill = true;



        if (isCanSkill)
        {
            currentHealth += 10;
            Debug.Log($"HP: {currentHealth}");
            if (currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
            }
            onHPSlider?.Invoke(this, EventArgs.Empty);

            StartCoroutine(RecoverySkillDuration_Co());
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
        isCanHit = false;
        yield return new WaitForSeconds(hitTimer);
        isCanHit = true;
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
    
    
    
    // Skill 끝남
    IEnumerator shieldSkillDuration_Co()      
    {
        yield return new WaitForSeconds(skillDuration);

        isCanSkill = false;
        ShieldOn.SetActive(false);
        //skillActive.shieldFillImage.fillAmount = 1.0f;
        this.horizontalPlayer.coroutine = StartCoroutine(skillActive.CoolTime_Co());
    }

     public IEnumerator magneticSkillDuration_Co()
     {
        yield return new WaitForSeconds(skillDuration);



        isCanSkill = false;
        MagneticOn.SetActive(false);
        magnetic.Radius = magnetic.Radius / 2;
        this.horizontalPlayer.coroutine = StartCoroutine(skillActive.CoolTime_Co());
     }

    public IEnumerator RecoverySkillDuration_Co()
    {
        yield return new WaitForSeconds(skillDuration);

        isCanSkill = false;
        RecoveryOn.SetActive(false);
        this.horizontalPlayer.coroutine = StartCoroutine(skillActive.CoolTime_Co());
    }

    public IEnumerator SpeedUpSkillDuration_Co()
    {
        yield return new WaitForSeconds(skillDuration);

        isCanSkill = false;
        SpeedUpOn.SetActive(false);

        horizontalPlayer.maxSpeed = horizontalPlayer.maxSpeed / 2f;
        horizontalPlayer.currentAcceleration = horizontalPlayer.baseAcceleration;

        this.horizontalPlayer.coroutine = StartCoroutine(skillActive.CoolTime_Co());
    }



    #endregion
}
