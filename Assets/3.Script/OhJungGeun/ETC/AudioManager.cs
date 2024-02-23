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
        if (SceneManager.GetActiveScene().name == "CharacterTest")
        {
            PlayBGM(BGM_Name.Stage);
        }
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




    #region [SetAudioMixer]

    /// [사용처 및 사용방법]
    ///Use To Volume Slider
    ///ex)  m_MusicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
    public void SetMasterVolume(float volume)
    {


        m_AudioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetBGMVolume(float volume)
    {
        m_AudioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        m_AudioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    #endregion

}
