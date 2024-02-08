using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrailEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem movementParticle;

    [Range(0, 10)]
    [SerializeField] float effectTime;

    [Range(0,0.2f)]
    [SerializeField] float occurVelocity;

    [SerializeField] Rigidbody2D playerRB;

    float counter;

    private void Update()
    {
        
        EffectProduce();
    }

    private void EffectProduce()
    {
        counter += Time.deltaTime;
        
        if(Mathf.Abs(playerRB.velocity.x)> occurVelocity)
        {
            if(counter > occurVelocity)
            {
                movementParticle.Play();
                counter = 0;
            }
        }
    }


}
