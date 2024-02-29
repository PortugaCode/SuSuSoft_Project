using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_touchFX;




    public ParticleSystem GetTouchFX()
    {
        return m_touchFX;
    }

    public void PlayTouchFX(Vector3 touchPos)
    {
        m_touchFX.gameObject.transform.position = touchPos;
        m_touchFX.Play();
    }
}
