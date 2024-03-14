using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BossControll : MonoBehaviour
{
    public EventHandler onBossHPSlide;
    public EventHandler onBossDamage;

    [SerializeField] HorizontalPlayer horizontalPlayer;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Vector2 screenBound;

    [SerializeField] GameObject bossRed;

    //HP
    public float maxHealth;
    public float currentHealth;
    public float damage;


    [SerializeField] Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        horizontalPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<HorizontalPlayer>();
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Star"))
        {
            animator.SetTrigger("BossHurt");

            BossDamage();
            onBossHPSlide?.Invoke(this, EventArgs.Empty);   //Damage
        }

    }
    public void BossDamage()
    {
        currentHealth -= damage;
        onBossDamage?.Invoke(this, EventArgs.Empty);

         if (currentHealth <= 0)
        {
            currentHealth = 0;
            //BossDie(); // 만들어 주기
            return;
        }
        else if(currentHealth <= 30)
        {
            bossRed.gameObject.SetActive(true);
            //Die 메서드
        }
        else if(currentHealth <= 50)
        {
            animator.SetTrigger("BossRage");
        }
      
    }
}

