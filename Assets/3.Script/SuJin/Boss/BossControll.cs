using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossControll : MonoBehaviour
{
    public EventHandler onBossHPSlide;
    public EventHandler onBossDamage;

    HorizontalPlayer horizontalPlayer;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Vector2 screenBound;

    PlayerProperty playerProperty;

    [Header("Face")]
    [SerializeField] SpriteRenderer bossbase;
    [SerializeField] Sprite bossRed;

    [Header("Particle")]
    [SerializeField] private ParticleSystem bossHitAction_1;
    [SerializeField] private ParticleSystem bossHitAction_2;
    [SerializeField] private ParticleSystem bossHitAction_3;
    [SerializeField] private ParticleSystem bossdDieAction;

    [Header("HP")]
    public float maxHealth;
    public float currentHealth;
    public float damage;

    public  Animator animator;
    [SerializeField] GameObject bossStageClearPopup;

    GameObject endWhite;
    Image endImage;

    private bool isDie =false;
   

    private void Awake()
    {
       // animator = GetComponent<Animator>();
        playerProperty = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerProperty>();
        horizontalPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<HorizontalPlayer>();

        endWhite = GameObject.FindGameObjectWithTag("EditorOnly");  // 태그 바꾸기 
        endImage = endWhite.GetComponent<Image>();
        endWhite.SetActive(false);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    private void LateUpdate()
    {
        ClampScreen();
    }

    #region [Player Follow]
    public void ClampScreen()
    {
        screenBound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        Vector3 objectPos = transform.position;
        objectPos.y = Mathf.Clamp(objectPos.y, screenBound.y * -1, screenBound.y - 0.65f);
        transform.position = objectPos;
    }

    private void FollowPlayer()
    {
        rb.velocity = horizontalPlayer.GetRigidbody2D().velocity;
        transform.position = new Vector2(horizontalPlayer.gameObject.transform.position.x, transform.position.y);
    }
    #endregion


   /* private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Star"))
        {
            animator.SetTrigger("BossRage");

            BossDamage();

            onBossHPSlide?.Invoke(this, EventArgs.Empty);   //Damage
        }
    }*/
    public void BossDamage()
    {
        currentHealth -= damage;
        onBossDamage?.Invoke(this, EventArgs.Empty);

         if (currentHealth <= 0 && !isDie)
        {
            currentHealth = 0;
            BossDie();
            return;
        }
        else if(currentHealth <= 30)
        {
            bossbase.sprite = bossRed;
            gameObject.transform.GetChild(0).transform.localScale = new Vector3(2.09f, 2.09f, 2.09f);
            bossHitAction_3.Play();
        }
        else if(currentHealth <= 50)
        {
            animator.SetTrigger("BossRage");
            bossHitAction_2.Play();
        }
         else if(currentHealth <= 100)
        {
            bossHitAction_1.Play();
        }
    }

    private void BossDie()
    {
        isDie = true;
        horizontalPlayer.isGameOver = true;
        bossdDieAction.Play();
        animator.SetTrigger("BossDie");
        StartCoroutine(FadeIn());
        StartCoroutine(DiePopUp_Co());
    }

    
    public void BossStageClear()
    {
        bossStageClearPopup.GetComponent<StageClear>().ShowClearUI();

        bossStageClearPopup.SetActive(true);
    }


    IEnumerator DiePopUp_Co()
    {
        yield return new WaitForSeconds(4f);
        BossStageClear();
        playerProperty.StopPlayer();
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(2f);
        endWhite.SetActive(true);
        for (int i = 0; i<10; i++)
        {
            float fade = i / 10f;
            Color color = endImage.color;
            color.a = fade;
            endImage.color = color;

            yield return new WaitForSeconds(0.1f);
        }
    }
}

