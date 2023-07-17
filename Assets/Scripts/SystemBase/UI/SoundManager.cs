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
    [SerializeField] private AudioSource _audioSource = null;
    [Header("Sound Bar")]
    [SerializeField] private Slider _bgmSoundSlider = null;
    [SerializeField] private Slider _bgmSoundSlider2 = null;
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer _bgmMixer = null;
    private void Awake()
    {
        SetSoundSourceToDic();
        _bgmSoundSlider.onValueChanged.AddListener(BgmSoundControl);
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
    public void TurnOnPlayerOneShot(EnumTypes.EffectSoundType type) => _audioSource.PlayOneShot(_effSoundSourec[type]);
    public void TurnOnStageBGM(EnumTypes.StageBGM stageBGM)
    {
        _audioSource.Stop();
        _audioSource.clip = _bgmSoundSourec[stageBGM];
        _audioSource.loop = true;
        _audioSource.Play();
    }
    public void BgmSoundControlMixer()
    {
        float sound = _bgmSoundSlider2.value;

        if (sound == -40f) _bgmMixer.SetFloat("BGMSoundValue", -80f);
        else _bgmMixer.SetFloat("BGMSoundValue", sound);
    }
    private void Update()
    {
        BgmSoundControlMixer();
    }
    public void BgmSoundControl(float value)
    {
        _audioSource.volume = value;
    }
    public bool BGMPlayeState() => _audioSource.isPlaying;
}
