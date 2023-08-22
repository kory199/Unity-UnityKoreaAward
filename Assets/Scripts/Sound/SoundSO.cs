using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BGMSound
{
    public AudioClip AudioClip;
    public EnumTypes.StageBGMType StageBGM;
}

[Serializable]
public class BossBGMSound
{
    public AudioClip AudioClip;
    public EnumTypes.BossBGMType BossBGMType;
}

[Serializable]
public class SFXSound
{
    public AudioClip AudioClip;
    public EnumTypes.SFXType SfxSound;
}

[CreateAssetMenu(fileName = "SoundSO", menuName = "ScriptableObjects/SoundSO", order = int.MaxValue)]
public class SoundSO : ScriptableObject
{
    [SerializeField] public List<BGMSound> BGMSoundClips = null;
    [SerializeField] public List<BossBGMSound> BossBGMClips = null;
    [SerializeField] public List<SFXSound> SFXSoundClips = null;
}