using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public enum SFX_Name
{
    Gain,
    Hit
}

public enum BGM_Name
{
    Main,
    Stage
}


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance = null;


    public float bgmValue = 0.6f;
    public float sfxValue = 0.6f;


    [SerializeField] private AudioMixer m_AudioMixer;
    

    [Header("AudioSource Collection")]
    [SerializeField] private AudioSource bgm_AudioSource;
    [SerializeField] private AudioSource sfx_AudioSource;

    [Header("BGM AudioClip")]
    [SerializeField] private AudioClip mainBGM;
    [SerializeField] private AudioClip stageBGM;

    [Header("SFX AudioClip")]
    [SerializeField] private AudioClip gainClip;
    [SerializeField] private AudioClip hitClip;


    private void Awake()
    {
        #region [싱글톤]
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        #endregion
    }

    private void Start()
    {
        //Test
        PlayBGM(BGM_Name.Main);
    }

    #region [Audio Play]

    public void PlaySFX(SFX_Name clip)
    {
        switch (clip)
        {
            case SFX_Name.Gain :
                sfx_AudioSource.PlayOneShot(gainClip);
                break;
            case SFX_Name.Hit:
                sfx_AudioSource.PlayOneShot(hitClip);
                break;
            default:
                Debug.Log("Can`t Find That Clip");
                break;
        }
    }

    public void PlayBGM(BGM_Name music)
    {
        switch (music)
        {
            case BGM_Name.Main:
                bgm_AudioSource.Stop();
                bgm_AudioSource.clip = mainBGM;
                bgm_AudioSource.Play();
                break;
            case BGM_Name.Stage:
                bgm_AudioSource.Stop();
                bgm_AudioSource.clip = stageBGM;
                bgm_AudioSource.Play();
                break;
            default:
                Debug.Log("Can`t Find That Music");
                break;
        }
    }


    #endregion





    public AudioMixer GetAudioMixer()
    {
        return m_AudioMixer;
    }



    #region [SetAudioMixer]

    /// [사용처 및 사용방법]
    ///Use To Volume Slider
    ///ex)  m_MusicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
    public void SetMasterVolume(float volume)
    {
        if (volume <= 0)
        {
            m_AudioMixer.SetFloat("Master", -80f);
            return;
        }
        m_AudioMixer.SetFloat("Master", Mathf.Log10(volume) * 40);
    }

    public void SetBGMVolume(float volume)
    {
        if (volume <= 0)
        {
            m_AudioMixer.SetFloat("BGM", -80f);
            return;
        }
        m_AudioMixer.SetFloat("BGM", Mathf.Log10(volume) * 40);
    }

    public void SetSFXVolume(float volume)
    {
        if (volume <= 0)
        {
            m_AudioMixer.SetFloat("SFX", -80f);
            return;
        }
        m_AudioMixer.SetFloat("SFX", Mathf.Log10(volume) * 40);
    }

    #endregion

}
