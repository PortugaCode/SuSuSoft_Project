using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetic : MonoBehaviour
{
    public LayerMask MagneticLayers; // 자기장을 적용할 레이어
    public Vector3 Position; // 자기장 중심 위치
    public float Radius; // 자기장 반경
    public float Force; // 자기력의 세기

    public int magneticItemDuration = 2;

    Rigidbody2D rb;
    PlayerProperty playerProperty;

    private bool isMagnetItem = false;
    public bool isMagnetActive = false;
    private bool isMagnetPassive = false;

    private void Start()
    {
        isMagnetPassive = true;
        playerProperty = GetComponent<PlayerProperty>();
    }

    private void Update()
    {
        if (isMagnetPassive)
        {
            PassiveMagnet();
        }
        else if (isMagnetItem)
        {
            ItemMagnet();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Magnet"))
        {
            isMagnetItem = true;

            //Audio
            AudioManager.Instance.PlaySFX(SFX_Name.MagnetBuff);

            //쿨타임이 다 돌면 원래 상태로 돌리기
            StartCoroutine(Magneticduration());
            Destroy(collision.gameObject);
        }
    }
    public void PassiveMagnet()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, Radius, MagneticLayers);

        foreach (Collider2D collider in colliders)
        {
            rb = collider.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                continue;
            }
            Vector3 direction = (transform.position - collider.transform.position).normalized;

            if(collider.CompareTag("BigStar") || collider.CompareTag("BigHp"))
            {
                // 자기력을 적용
                rb.AddForceAtPosition(direction, transform.position, ForceMode2D.Impulse);
                rb.AddTorque(360, ForceMode2D.Force);
                return;
            }

            // 자기력을 적용
            rb.AddForceAtPosition(direction, transform.position, ForceMode2D.Impulse);
            //rb.velocity = direction * Force * Time.deltaTime;
            rb.AddTorque(360, ForceMode2D.Force);
        }
    }

    public void ItemMagnet()
    {
        // 자기장 영역 안에 있는 collider 찾기
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, Radius * 3, MagneticLayers);

        foreach (Collider2D collider in colliders)
        {
            rb = collider.GetComponent<Rigidbody2D>();

            if (rb == null)
            {
                continue;
            }

            Vector3 direction = (transform.position - collider.transform.position).normalized;
            rb.velocity = direction * (Force * 10f) * Time.deltaTime;
            Debug.Log($"{Radius} ,{Force}");
        }
    }

    public void ActiveMagnet()
    {
        playerProperty.skillActive.isItemOn = false;
        playerProperty.isCanSkill = true;
        isMagnetActive = true;

        if (playerProperty.isCanSkill && isMagnetActive)
        {
            Radius = Radius * 2;

            //Audio
            AudioManager.Instance.PlaySFX(SFX_Name.Magnet) ;

            StartCoroutine(playerProperty.magneticSkillDuration_Co());
            isMagnetPassive = true;
        }
    }



    IEnumerator Magneticduration()
    {
        yield return new WaitForSeconds(magneticItemDuration);
        isMagnetItem = false;
        isMagnetPassive = true;
    }

    void OnDrawGizmosSelected()
    {
        // 자기장 영역을 시각적으로 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
