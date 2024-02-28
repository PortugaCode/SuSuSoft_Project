using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SkillActive : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float followSpeed;

    [SerializeField] Vector3 offset;
    private float distanse;

    //Cool Time
    private float coolTime; // 남은 CoolTime
    [SerializeField] private float coolTimeMax; //CoolTime
    [SerializeField] private Image skillBG;
    [SerializeField] private Image itmeImage;
    


    HorizontalPlayer horizontalPlayer;

    private void FixedUpdate()
    {
        ChasePlayer();
    }

    private void ChasePlayer()
    {
        if (player != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(player.transform.position + offset);
        }
    }

    IEnumerator coolTime_Co()
    {
        coolTime -= Time.deltaTime;



        yield return new WaitForFixedUpdate();
    }
}
