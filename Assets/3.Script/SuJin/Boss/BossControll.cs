using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossControll : MonoBehaviour
{
    public EventHandler onBossHPSlide;
    public EventHandler onBossDamage;

    [SerializeField] GameObject fourStageBoss;
    [SerializeField] GameObject bossStageBoss;

    [SerializeField] HorizontalPlayer horizontalPlayer;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Vector2 screenBound;

    [SerializeField] PlayerProperty playerProperty;

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
        Debug.Log("EndWhite false");
        playerProperty = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerProperty>();
        horizontalPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<HorizontalPlayer>();

        endWhite = GameObject.FindObjectOfType<LevelManager>().EndWhite;
        endImage = endWhite.GetComponent<Image>();
        endWhite.SetActive(false);


        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

        if (Utils.Instance.isFourStage)
        {
            Debug.Log("4스테이지 보스 온");
            fourStageBoss.SetActive(true);
            bossStageBoss.SetActive(false);
        }
        else if (Utils.Instance.isBossStage)
        {
            Debug.Log("5스테이지 보스 온");
            fourStageBoss.SetActive(false);
            bossStageBoss.SetActive(true);
        }
        else
        {
            fourStageBoss.SetActive(false);
            bossStageBoss.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if(Utils.Instance.currentLevel == Level.Level_5)
        {
            FollowPlayer();
        }
        else if(Utils.Instance.currentLevel == Level.Level_4)
        {
            FollowPlayer_4();
        }

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

        if(Utils.Instance.isBossStage)
        {
            objectPos.y = Mathf.Clamp(objectPos.y, screenBound.y * -1, screenBound.y - 0.65f);
            transform.position = objectPos;
        }
/*        else if (Utils.Instance.isFourStage)
        {
            objectPos.y = Mathf.Clamp(objectPos.y, screenBound.y * -50 + 0.25f, screenBound.y);
            transform.position = objectPos;
            rb.velocity = Vector2.zero;
        }*/
    }

    private void FollowPlayer_4()
    {
        if (horizontalPlayer != null)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(horizontalPlayer.transform.position.x, horizontalPlayer.transform.position.y - 4.2f), 15f * Time.deltaTime);
        }
    }

    private void FollowPlayer()
    {
        if(horizontalPlayer != null)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(horizontalPlayer.transform.position.x, horizontalPlayer.transform.position.y + 5f), 15f * Time.deltaTime);
            /*            rb.velocity = horizontalPlayer.GetRigidbody2D().velocity;
                        transform.position = new Vector2(horizontalPlayer.gameObject.transform.position.x, transform.position.y);*/
        }
    }
    #endregion

    public void BossDamage()
    {
        currentHealth -= damage;
        onBossDamage?.Invoke(this, EventArgs.Empty);
        Debug.Log($"{currentHealth}");

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
            AudioManager.Instance.PlaySFX(SFX_Name.Crash3);
        }
        else if(currentHealth <= 50)
        {
            animator.SetTrigger("BossRage");
            bossHitAction_2.Play();
            AudioManager.Instance.PlaySFX(SFX_Name.Crash2);
        }
         else if(currentHealth <= 100)
        {
            bossHitAction_1.Play();
            AudioManager.Instance.PlaySFX(SFX_Name.Crash1);
        }
    }
    public void OtherDamage()
    {
        currentHealth -= damage/2;
        onBossDamage?.Invoke(this, EventArgs.Empty);
        Debug.Log($"{currentHealth}");

        if (currentHealth <= 0 && !isDie)
        {
            currentHealth = 0;
            BossDie();
            return;
        }
        else if (currentHealth <= 30)
        {
            bossbase.sprite = bossRed;
            gameObject.transform.GetChild(0).transform.localScale = new Vector3(2.09f, 2.09f, 2.09f);
            bossHitAction_3.Play();
        }
        else if (currentHealth <= 50)
        {
            animator.SetTrigger("BossRage");
            bossHitAction_2.Play();
        }
        else if (currentHealth <= 100)
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
        AudioManager.Instance.PlaySFX(SFX_Name.StageSuccess);
        AudioManager.Instance.BGM_AudioSource.Stop();
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

