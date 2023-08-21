using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
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
        public EnumTypes.StageBGMType StageBGM;
    }
    Dictionary<EnumTypes.EffectSoundType, AudioClip> _effSoundSourec = new Dictionary<EnumTypes.EffectSoundType, AudioClip>();
    Dictionary<EnumTypes.StageBGMType, AudioClip> _bgmSoundSourec = new Dictionary<EnumTypes.StageBGMType, AudioClip>();
    [SerializeField] private List<EffSoundInfo> _effSoundClips = null;
    [SerializeField] private List<BGMSoundInfo> _bgmSoundClips = null;
    private AudioSource _audioSource = null;
    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        SetSoundSourceToDic();
        DontDestroyOnLoad(this.gameObject);
    }
    /// <summary>
    /// ��� ���� ��ųʸ� �ֱ�
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
    public void TurnOnStageBGM(EnumTypes.StageBGMType stageBGM)
    {
        _audioSource.Stop();
        _audioSource.clip = _bgmSoundSourec[stageBGM];
        _audioSource.loop = true;
        _audioSource.Play();
    }
}
