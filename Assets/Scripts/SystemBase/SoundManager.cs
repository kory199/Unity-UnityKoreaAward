using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singloton
    private static SoundManager _inst;
    public static SoundManager Inst
    {
        get
        {
            if (_inst == null)
            {
                _inst = FindObjectOfType<SoundManager>();
                if (_inst == null)
                {
                    _inst = new GameObject().AddComponent<SoundManager>();
                }
            }
            return _inst;
        }
    }
    #endregion
    public enum EffectSoundType
    {
        None,
        Attack,
        AttackSkill,
        Defence,
        DefenceSkill,
        Hit,
        Die,
        Button,
    }
    public enum StageBGM
    {
        Title,
        Lobby,
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5,
        Stage6,
    }

    [System.Serializable]
    public class EffSoundInfo
    {
        public AudioClip AudioClip;
        public EffectSoundType EffectSoundType;
    }

    [System.Serializable]
    public class BGMSoundInfo
    {
        public AudioClip AudioClip;
        public StageBGM StageBGM;
    }
    Dictionary<EffectSoundType, AudioClip> _effSoundSourec = new Dictionary<EffectSoundType, AudioClip>();
    Dictionary<StageBGM, AudioClip> _bgmSoundSourec = new Dictionary<StageBGM, AudioClip>();
    [SerializeField] private List<EffSoundInfo> _effSoundClips = null;
    [SerializeField] private List<BGMSoundInfo> _bgmSoundClips = null;
    private AudioSource _audioSource = null;
    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
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
    public void TurnOnPlayerOneShot(EffectSoundType type) => _audioSource.PlayOneShot(_effSoundSourec[type]);
    public void TurnOnStageBGM(StageBGM stageBGM)
    {
        _audioSource.Stop();
        _audioSource.clip = _bgmSoundSourec[stageBGM];
        _audioSource.loop = true;
        _audioSource.Play();
    }
}
