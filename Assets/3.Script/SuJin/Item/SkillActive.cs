using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActive : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float followSpeed;

    private void FixedUpdate()
    {
        ChasePlayer();
    }

    private void ChasePlayer()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.transform.position + Vector3.right * 0.5f;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}
