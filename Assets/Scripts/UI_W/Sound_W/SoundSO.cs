using UnityEngine;

[CreateAssetMenu(fileName = "SoundSO", menuName = "ScriptableObjects/SoundSO", order = int.MaxValue)]
public class SoundSO : ScriptableObject
{
    public AudioClip[] BGMClips = new AudioClip[2];
    public AudioClip[] InGameBGMClips = new AudioClip[4];
    public AudioClip[] BossBGMClips = new AudioClip[3];

    public AudioClip playerAttackSFX;
    public AudioClip playerDeathSFX;
    public AudioClip playerHitSFX;

    public AudioClip monsterHitSFX;
    public AudioClip monsterDeathSFX;

    public AudioClip skiilsSFX;
    public AudioClip stageChangeSFX;
    public AudioClip buttonSFX;
    public AudioClip escSFX;
    public AudioClip resultWindowSFX;
}