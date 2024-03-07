using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public enum SFX_Name
{
    //상호작용, 애니메이션
    Wield,
    Trophy01,
    FlyAway,
    BoingJump,
    ThrowSlideUp,
    ThrowSlideDown,
    Attack,
    Trophy02,
    Touch01,
    Touch02,
    BallBounce,
    Wood01,
    Wood02,
    WoodTwins,
    WoodFruit,

    //하우징 배치
    HousingError,
    HousingSuccess,
    HousingSound01,
    HousingSound02,

    //상점 구매 
    CashBuySound01,
    CashBuySound02,

    //공방 건물 사운드
    WorkshopSound01,
    WorkshopSound02,
    WorkshopSound03,
    WorkshopSound04,

    //캐릭터 성장 사운드
    CharacterGrowth,

    //퀘스트 보상 획득
    uiQuestReward,
    uiQuest,

    //UI 버튼 선택
    uiButtonClik,
    uiPopUpButtonClik,
    uiButtonConfirm,
    uiButtonStageHousing,
    uiError,

    //스테이지 클리어
    StageMatchBright01,
    StageMatchBright02,
    StageMatchBright03,

    //스테이지 오브젝트
    GetStar,
    GetHeart,
    GetToken,

    // 버프
    MagnetBuff,
    ShieldBuff,
    BigBuff,
    SmallBuff,

    //디버프
    InversionDebuff01,
    InversionDebuff02,
    SpeedDownDebuff,
    SpeedUpDebuff,
    DarkDebuff,

    //스테이지
    ChapterStage,
    StageSuccess,
    StageFail,

    //별자리 완성
    Star01,
    Star02,

    //스킬 사용 사운드
    Magnet,
    SkillLight,       //보호막, 지속 회복 같은 스킬 유지 사운드
    SkillSound,      //스킬 발동 및 유지 사운드

    //피격,타격음
    Crash1,
    Crash2,
    Crash3
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

    //상호작용, 애니메이션
    [SerializeField] private AudioClip Wield_Clip;
    [SerializeField] private AudioClip Trophy01_Clip;
    [SerializeField] private AudioClip FlyAway_Clip;
    [SerializeField] private AudioClip BoingJump_Clip;
    [SerializeField] private AudioClip ThrowSlideUp_Clip;
    [SerializeField] private AudioClip ThrowSlideDown_Clip;
    [SerializeField] private AudioClip Attack_Clip;
    [SerializeField] private AudioClip Trophy02_Clip;

    [SerializeField] private AudioClip Touch01_Clip;
    [SerializeField] private AudioClip Touch02_Clip;
    [SerializeField] private AudioClip BallBounce_Clip;
    [SerializeField] private AudioClip Wood01_Clip;
    [SerializeField] private AudioClip Wood02_Clip;
    [SerializeField] private AudioClip WoodTwins_Clip;
    [SerializeField] private AudioClip WoodFruit_Clip;

    //하우징 배치
    [SerializeField] private AudioClip HousingError_Clip;
    [SerializeField] private AudioClip HousingSuccess_Clip;
    [SerializeField] private AudioClip HousingSound01_Clip;
    [SerializeField] private AudioClip HousingSound02_Clip;

    //상점 구매
    [SerializeField] private AudioClip CashBuySound01_Clip;
    [SerializeField] private AudioClip CashBuySound02_Clip;

    //공방 건물 사운드
    //여기 부터 0306


    [SerializeField] private AudioClip GetStar_Clip;
    [SerializeField] private AudioClip GetHeart_Clip;
    [SerializeField] private AudioClip GetToken_Clip;


    [SerializeField] private AudioClip SkillSound_Clip;
    [SerializeField] private AudioClip Magnet_Clip;
    [SerializeField] private AudioClip SkillLight_Clip;

    [SerializeField] private AudioClip MagnetBuff_Clip;
    [SerializeField] private AudioClip ShieldBuff_Clip;
    [SerializeField] private AudioClip BigBuff_Clip;
    [SerializeField] private AudioClip SmallBuff_Clip;

    [SerializeField] private AudioClip Crash1_Clip;
    [SerializeField] private AudioClip Crash2_Clip;
    [SerializeField] private AudioClip Crash3_Clip;

   //[SerializeField] private AudioClip SkillLight_Clip;


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

            case SFX_Name.SkillSound:
                sfx_AudioSource.PlayOneShot(SkillSound_Clip);
                break;
            case SFX_Name.Magnet:
                sfx_AudioSource.PlayOneShot(Magnet_Clip);
                break;
            case SFX_Name.SkillLight:
                sfx_AudioSource.PlayOneShot(SkillLight_Clip);
                break;

            case SFX_Name.GetStar:
                sfx_AudioSource.PlayOneShot(GetStar_Clip);
                break;
            case SFX_Name.GetHeart:
                sfx_AudioSource.PlayOneShot(GetHeart_Clip);
                break;
            case SFX_Name.GetToken:
                sfx_AudioSource.PlayOneShot(GetToken_Clip);
                break;

            case SFX_Name.MagnetBuff:
                sfx_AudioSource.PlayOneShot(MagnetBuff_Clip);
                break;
            case SFX_Name.ShieldBuff:
                sfx_AudioSource.PlayOneShot(ShieldBuff_Clip);
                break;
            case SFX_Name.BigBuff:
                sfx_AudioSource.PlayOneShot(BigBuff_Clip);
                break;
            case SFX_Name.SmallBuff:
                sfx_AudioSource.PlayOneShot(SmallBuff_Clip);
                break;

            case SFX_Name.Crash1:
                sfx_AudioSource.PlayOneShot(Crash1_Clip);
                break;
            case SFX_Name.Crash2:
                sfx_AudioSource.PlayOneShot(Crash2_Clip);
                break;
            case SFX_Name.Crash3:
                sfx_AudioSource.PlayOneShot(Crash3_Clip);
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
