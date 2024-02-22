using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetic : MonoBehaviour
{
    public LayerMask MagneticLayers; // 자기장을 적용할 레이어
    public Vector3 Position; // 자기장 중심 위치
    public float Radius; // 자기장 반경
    public float Force; // 자기력의 세기

    public int duration = 2;
    //public  float coolTime = 2f;

    Rigidbody2D rb;

    private bool isMagnetItem = false;
    private bool isMagnetPassive = false;

    private void Start()
    {
        isMagnetPassive = true;
    }

    private void Update()
    {
        if(isMagnetPassive)
        {
            PassiveMagnet();
        }
        else if (isMagnetItem)
        {
            ActiveMagnet();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Magnet"))
        {
            isMagnetItem = true;

            //쿨타임이 다 돌면 원래 상태로 돌리기
            StartCoroutine(Itemduration());
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

    private void ActiveMagnet()
    {
        // 자기장 영역 안에 있는 collider 찾기
        // 박수진 Physics 쓰고 안된다고 함 => 2D 프로젝트인데...
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, Radius * 3, MagneticLayers);

        foreach (Collider2D collider in colliders)
        {
            rb = collider.GetComponent<Rigidbody2D>();
            //rigidbody = (Rigidbody2D)collider.gameObject.GetComponent(typeof(Rigidbody2D));
            if (rb == null)
            {
                continue;
            }
            Vector3 direction = (transform.position - collider.transform.position).normalized;

            rb.velocity = direction * (Force * 10f) * Time.deltaTime;
            // 자기력을 적용
            //rigidbody.AddForceAtPosition(Force, transform.position);
        }
    }

    IEnumerator Itemduration()
    {
        yield return new WaitForSeconds(duration);
        isMagnetItem = false;
        isMagnetPassive = true;
    }

    /*IEnumerator coolTime_co()   //쿨타임
    {
        while(coolTime > 0.0f)
        {
            coolTime -= Time.deltaTime;
            Debug.Log(coolTime);

           yield return new WaitForFixedUpdate();
        }
    }*/
    void OnDrawGizmosSelected()
    {
        // 자기장 영역을 시각적으로 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
