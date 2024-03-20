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
    Stage,
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

    public AudioSource BGM_AudioSource => bgm_AudioSource;
    public AudioSource SFX_AudioSource => sfx_AudioSource;

    [Header("BGM AudioClip")]
    [SerializeField] private AudioClip mainBGM;
    [SerializeField] private AudioClip stageBGM;

    [Header("SFX AudioClip")]
    [Header("상호작용, 애니메이션")]
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

    [Header("하우징 배치")]
    [SerializeField] private AudioClip HousingError_Clip;
    [SerializeField] private AudioClip HousingSuccess_Clip;
    [SerializeField] private AudioClip HousingSound01_Clip;
    [SerializeField] private AudioClip HousingSound02_Clip;

    [Header("상점 구매")]
    [SerializeField] private AudioClip CashBuySound01_Clip;
    [SerializeField] private AudioClip CashBuySound02_Clip;

    [Header("공방 건물 사운드")]
    [SerializeField] private AudioClip WorkshopSound01_Clip;
    [SerializeField] private AudioClip WorkshopSound02_Clip;
    [SerializeField] private AudioClip WorkshopSound03_Clip;
    [SerializeField] private AudioClip WorkshopSound04_Clip;

    [Header("캐릭터 성장 사운드")]
    [SerializeField] private AudioClip CharacterGrowth_Clip;

    [Header("퀘스트 보상 획득")]
    [SerializeField] private AudioClip uiQuestReward_Clip;
    [SerializeField] private AudioClip uiQuest_Clip;

    [Header("UI버튼 선택")]
    [SerializeField] private AudioClip uiButtonClik_Clip;
    [SerializeField] private AudioClip uiPopUpButtonClik_Clip;
    [SerializeField] private AudioClip uiButtonConfirm_Clip;
    [SerializeField] private AudioClip uiButtonStageHousing_Clip;
    [SerializeField] private AudioClip uiError_Clip;

    [Header("스테이지 클리어")]
    [SerializeField] private AudioClip StageMatchBright01_Clip;
    [SerializeField] private AudioClip StageMatchBright02_Clip;
    [SerializeField] private AudioClip StageMatchBright03_Clip;

    [Header("스테이지 오브젝트")]
    [SerializeField] private AudioClip GetStar_Clip;
    [SerializeField] private AudioClip GetHeart_Clip;
    [SerializeField] private AudioClip GetToken_Clip;

    [Header("버프")]
    [SerializeField] private AudioClip MagnetBuff_Clip;
    [SerializeField] private AudioClip ShieldBuff_Clip;
    [SerializeField] private AudioClip BigBuff_Clip;
    [SerializeField] private AudioClip SmallBuff_Clip;

    [Header("디버프")]
    [SerializeField] private AudioClip InversionDebuff01_Clip;
    [SerializeField] private AudioClip InversionDebuff02_Clip;
    [SerializeField] private AudioClip SpeedDownDebuff_Clip;
    [SerializeField] private AudioClip SpeedUpDebuff_Clip;
    [SerializeField] private AudioClip DarkDebuff_Clip;

    [Header("스테이지")]
    [SerializeField] private AudioClip ChapterStage_Clip;
    [SerializeField] private AudioClip StageSuccess_Clip;
    [SerializeField] private AudioClip StageFail_Clip;

    [Header("별자리 완성")]
    [SerializeField] private AudioClip Star01_Clip;
    [SerializeField] private AudioClip Star02_Clip;

    [Header("스킬 사용 사운드")]
    [SerializeField] private AudioClip Magnet_Clip;
    [SerializeField] private AudioClip SkillLight_Clip;
    [SerializeField] private AudioClip SkillSound_Clip;

    [Header("피격, 타격음")]
    [SerializeField] private AudioClip Crash1_Clip;
    [SerializeField] private AudioClip Crash2_Clip;
    [SerializeField] private AudioClip Crash3_Clip;


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
        SetBGMVolume(0.6f);
        SetSFXVolume(0.6f);
        //Test
        PlayBGM(BGM_Name.Main);
    }

    #region [Audio Play]

    public void PlaySFX(SFX_Name clip)
    {
        switch (clip)
        {
            case SFX_Name.Wield:
                sfx_AudioSource.PlayOneShot(Wield_Clip);
                break;
            case SFX_Name.Trophy01:
                sfx_AudioSource.PlayOneShot(Trophy01_Clip);
                break;
            case SFX_Name.FlyAway:
                sfx_AudioSource.PlayOneShot(FlyAway_Clip);
                break;
            case SFX_Name.BoingJump:
                sfx_AudioSource.PlayOneShot(BoingJump_Clip);
                break;
            case SFX_Name.ThrowSlideUp:
                sfx_AudioSource.PlayOneShot(ThrowSlideUp_Clip);
                break;
            case SFX_Name.ThrowSlideDown:
                sfx_AudioSource.PlayOneShot(ThrowSlideDown_Clip);
                break;
            case SFX_Name.Attack:
                sfx_AudioSource.PlayOneShot(Attack_Clip);
                break;
            case SFX_Name.Trophy02:
                sfx_AudioSource.PlayOneShot(Trophy02_Clip);
                break;
            case SFX_Name.Touch01:
                sfx_AudioSource.PlayOneShot(Touch01_Clip);
                break;
            case SFX_Name.Touch02:
                sfx_AudioSource.PlayOneShot(Touch02_Clip);
                break;
            case SFX_Name.BallBounce:
                sfx_AudioSource.PlayOneShot(BallBounce_Clip);
                break;
            case SFX_Name.Wood01:
                sfx_AudioSource.PlayOneShot(Wood01_Clip);
                break;
            case SFX_Name.Wood02:
                sfx_AudioSource.PlayOneShot(Wood02_Clip);
                break;
            case SFX_Name.WoodTwins:
                sfx_AudioSource.PlayOneShot(WoodTwins_Clip);
                break;
            case SFX_Name.WoodFruit:
                sfx_AudioSource.PlayOneShot(WoodFruit_Clip);
                break;

            //
            case SFX_Name.HousingError:
                sfx_AudioSource.PlayOneShot(HousingError_Clip);
                break;
            case SFX_Name.HousingSuccess:
                sfx_AudioSource.PlayOneShot(HousingSuccess_Clip);
                break;
            case SFX_Name.HousingSound01:
                sfx_AudioSource.PlayOneShot(HousingSound01_Clip);
                break;
            case SFX_Name.HousingSound02:
                sfx_AudioSource.PlayOneShot(HousingSound02_Clip);
                break;

            //
            case SFX_Name.CashBuySound01:
                sfx_AudioSource.PlayOneShot(CashBuySound01_Clip);
                break;
            case SFX_Name.CashBuySound02:
                sfx_AudioSource.PlayOneShot(CashBuySound02_Clip);
                break;

            //
            case SFX_Name.WorkshopSound01:
                sfx_AudioSource.PlayOneShot(WorkshopSound01_Clip);
                break;
            case SFX_Name.WorkshopSound02:
                sfx_AudioSource.PlayOneShot(WorkshopSound02_Clip);
                break;
            case SFX_Name.WorkshopSound03:
                sfx_AudioSource.PlayOneShot(WorkshopSound03_Clip);
                break;
            case SFX_Name.WorkshopSound04:
                sfx_AudioSource.PlayOneShot(WorkshopSound04_Clip);
                break;

            //
            case SFX_Name.CharacterGrowth:
                sfx_AudioSource.PlayOneShot(CharacterGrowth_Clip);
                break;

            //
            case SFX_Name.uiQuestReward:
                sfx_AudioSource.PlayOneShot(uiQuestReward_Clip);
                break;
            case SFX_Name.uiQuest:
                sfx_AudioSource.PlayOneShot(uiQuest_Clip);
                break;

            //
            case SFX_Name.uiButtonClik:
                sfx_AudioSource.PlayOneShot(uiButtonClik_Clip);
                break;
            case SFX_Name.uiPopUpButtonClik:
                sfx_AudioSource.PlayOneShot(uiPopUpButtonClik_Clip);
                break;
            case SFX_Name.uiButtonConfirm:
                sfx_AudioSource.PlayOneShot(uiButtonConfirm_Clip);
                break;
            case SFX_Name.uiButtonStageHousing:
                sfx_AudioSource.PlayOneShot(uiButtonStageHousing_Clip);
                break;
            case SFX_Name.uiError:
                sfx_AudioSource.PlayOneShot(uiError_Clip);
                break;

            //
            case SFX_Name.StageMatchBright01:
                sfx_AudioSource.PlayOneShot(StageMatchBright01_Clip);
                break;
            case SFX_Name.StageMatchBright02:
                sfx_AudioSource.PlayOneShot(StageMatchBright02_Clip);
                break;
            case SFX_Name.StageMatchBright03:
                sfx_AudioSource.PlayOneShot(StageMatchBright03_Clip);
                break;

            //
            case SFX_Name.GetStar:
                sfx_AudioSource.PlayOneShot(GetStar_Clip);
                break;
            case SFX_Name.GetHeart:
                sfx_AudioSource.PlayOneShot(GetHeart_Clip);
                break;
            case SFX_Name.GetToken:
                sfx_AudioSource.PlayOneShot(GetToken_Clip);
                break;

            //
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

            //
            case SFX_Name.InversionDebuff01:
                sfx_AudioSource.PlayOneShot(InversionDebuff01_Clip);
                break;
            case SFX_Name.InversionDebuff02:
                sfx_AudioSource.PlayOneShot(InversionDebuff02_Clip);
                break;
            case SFX_Name.SpeedDownDebuff:
                sfx_AudioSource.PlayOneShot(SpeedDownDebuff_Clip);
                break;
            case SFX_Name.SpeedUpDebuff:
                sfx_AudioSource.PlayOneShot(SpeedUpDebuff_Clip);
                break;
            case SFX_Name.DarkDebuff:
                sfx_AudioSource.PlayOneShot(DarkDebuff_Clip);
                break;

            //
            case SFX_Name.ChapterStage:
                sfx_AudioSource.PlayOneShot(ChapterStage_Clip);
                break;
            case SFX_Name.StageSuccess:
                sfx_AudioSource.PlayOneShot(StageSuccess_Clip);
                break;
            case SFX_Name.StageFail:
                sfx_AudioSource.PlayOneShot(StageFail_Clip);
                break;

            //
            case SFX_Name.Star01:
                sfx_AudioSource.PlayOneShot(Star01_Clip);
                break;
            case SFX_Name.Star02:
                sfx_AudioSource.PlayOneShot(Star02_Clip);
                break;

            //
            case SFX_Name.SkillSound:
                sfx_AudioSource.PlayOneShot(SkillSound_Clip);
                break;
            case SFX_Name.Magnet:
                sfx_AudioSource.PlayOneShot(Magnet_Clip);
                break;
            case SFX_Name.SkillLight:
                sfx_AudioSource.PlayOneShot(SkillLight_Clip);
                break;

            //
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
