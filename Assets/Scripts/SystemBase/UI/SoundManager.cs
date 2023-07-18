using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SoundManager : MonoSingleton<SoundManager>
{
    [System.Serializable]
    public class EffSoundInfo
    {
        public AudioClip AudioClip;
        public EnumTypes.EffectSoundType EffectSoundType;
    }

    [System.Serializable]
    public class BGMSoundInfo
    {
        public AudioClip AudioClip;
        public EnumTypes.StageBGM StageBGM;
    }
    Dictionary<EnumTypes.EffectSoundType, AudioClip> _effSoundSourec = new Dictionary<EnumTypes.EffectSoundType, AudioClip>();
    Dictionary<EnumTypes.StageBGM, AudioClip> _bgmSoundSourec = new Dictionary<EnumTypes.StageBGM, AudioClip>();
    [SerializeField] private List<EffSoundInfo> _effSoundClips = null;
    [SerializeField] private List<BGMSoundInfo> _bgmSoundClips = null;
    [SerializeField] private AudioSource _bgmSource = null;
    [SerializeField] private AudioSource _sfxSource = null;
    [Header("Sound Bar")]
    [SerializeField] private Slider _masterSoundSlider = null;
    [SerializeField] private Slider _bgmSoundSlider = null;
    [SerializeField] private Slider _sfxSoundSlider = null;
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer _soundMixer = null;
    private void Awake()
    {
        SetSoundSourceToDic();
    }
    /// <summary>
    /// 모든 사운드 딕셔너리 넣기
    /// </summary>
    public void SetSoundSourceToDic()
    {
        foreach (var sound in _effSoundClips)
        {
            _effSoundSourec[sound.EffectSoundType] = sound.AudioClip;
        }
        foreach (var sound in _bgmSoundClips)
        {
            _bgmSoundSourec[sound.StageBGM] = sound.AudioClip;
        }
    }
    public void TurnOnPlayerOneShot(EnumTypes.EffectSoundType type)
    {
        _sfxSource.outputAudioMixerGroup = _soundMixer.outputAudioMixerGroup;
        _sfxSource.PlayOneShot(_effSoundSourec[type]);
    }
    public void TurnOnStageBGM(EnumTypes.StageBGM stageBGM)
    {
        _bgmSource.Stop();
        _bgmSource.clip = _bgmSoundSourec[stageBGM];
        _bgmSource.outputAudioMixerGroup = _soundMixer.outputAudioMixerGroup;
        _bgmSource.loop = true;
        _bgmSource.Play();
    }
    /// <summary>
    /// 슬라이더바 볼륨 믹서 세팅
    /// </summary>
    /// <param name="type"></param>
    public void SetSoundMixer(string type)
    {
        float sound = -20f;
        switch (type)
        {
            case "MasterSoundValue":
                sound = _masterSoundSlider.value;
                if (sound == -40f) _soundMixer.SetFloat(type, -80f);
                else _soundMixer.SetFloat(type, sound);
                break;
            case "BGMSoundValue":
                Debug.Log(_bgmSoundSlider.value);
                sound = _bgmSoundSlider.value;
                if (sound == -40f) _soundMixer.SetFloat(type, -80f);
                else _soundMixer.SetFloat(type, sound);
                break;
            case "SFXSoundValue":
                sound = _sfxSoundSlider.value;
                if (sound == -40f) _soundMixer.SetFloat(type, -80f);
                else _soundMixer.SetFloat(type, sound);
                break;
        }
    }
    public bool BGMPlayeState() => _bgmSource.isPlaying;
}
