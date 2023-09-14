using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMgr : MonoSingleton<SoundMgr>
{
    [SerializeField] SoundSO soundSO = null;
    [SerializeField] AudioMixer audioMixer = null;
    [SerializeField] AudioSource bgmSource = null;
    [SerializeField] AudioSource sfxSource = null;

    private Dictionary<EnumTypes.StageBGMType, AudioClip> bgmDic = new Dictionary<EnumTypes.StageBGMType, AudioClip>();
    private Dictionary<EnumTypes.BossBGMType, AudioClip> bossBgmDic = new Dictionary<EnumTypes.BossBGMType, AudioClip>();
    private Dictionary<EnumTypes.SFXType, AudioClip> sfxDic = new Dictionary<EnumTypes.SFXType, AudioClip>();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if (soundSO == null || audioMixer == null)
        {
            soundSO = Resources.Load<SoundSO>("SoundSO");
            audioMixer = Resources.Load<AudioMixer>("GameSound");
        }

        SetSoundSourceToDic();
        BGMPlay(EnumTypes.StageBGMType.Title);
    }

    private void SetSoundSourceToDic()
    {
        foreach (BGMSound sound in soundSO.BGMSoundClips)
        {
            bgmDic[sound.StageBGM] = sound.AudioClip;
        }

        foreach (BossBGMSound sound in soundSO.BossBGMClips)
        {
            bossBgmDic[sound.BossBGMType] = sound.AudioClip;
        }

        foreach (SFXSound sound in soundSO.SFXSoundClips)
        {
            sfxDic[sound.SfxSound] = sound.AudioClip;
        }
    }

    public void MasterControl(Slider masterSlider)
    {
        float masterValue = masterSlider.value;

        if(masterValue == -40)
        {
            audioMixer.SetFloat("BGMSoundValue", -80);
            audioMixer.SetFloat("SFXSoundValue", -80);
        }
        else
        {
            audioMixer.SetFloat("BGMSoundValue", masterValue);
            audioMixer.SetFloat("SFXSoundValue", masterValue);
        }
    }

    public void BGMControl(Slider BGMSlider)
    {
        float bgmValue = BGMSlider.value;

        if (bgmValue == -40f)
            audioMixer.SetFloat("BGMSoundValue", -80);
        else
            audioMixer.SetFloat("BGMSoundValue", bgmValue);
    }

    public void SFXControl(Slider ButSfxSlider)
    {
        float sfxValue = ButSfxSlider.value;

        if (sfxValue == -40f)
            audioMixer.SetFloat("SFXSoundValue", -80);
        else
        {
            audioMixer.SetFloat("SFXSoundValue", sfxValue);
        }

        SFXPlay(EnumTypes.SFXType.Button);
    }

    public void BGMPlay(EnumTypes.StageBGMType stageBGM)
    {
        if (bgmDic.TryGetValue(stageBGM, out AudioClip clipToPlay))
        {
            bgmSource.clip = clipToPlay;
            bgmSource.Play();
        }
    }

    public void BossBgmPlay(EnumTypes.BossBGMType bossBGM)
    {
        if (bossBgmDic.TryGetValue(bossBGM, out AudioClip clipToPlay))
        {
            bgmSource.clip = clipToPlay;
            bgmSource.Play();
        }
    }

    public void SFXPlay(EnumTypes.SFXType sfxType)
    {
        if (sfxDic.TryGetValue(sfxType, out AudioClip clipToPlay))
        {
            sfxSource.clip = clipToPlay;
            sfxSource.Play();
        }
    }

    public float GetSFXLength(EnumTypes.SFXType sfxType)
    {
        if (sfxDic.TryGetValue(sfxType, out AudioClip clip))
        {
            return clip.length;
        }
        return 0f;
    }

    public void ToggleAduioVolume()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }
}