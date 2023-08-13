using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundTest_W : MonoSingleton<SoundTest_W>
{
    [Header("[Audio BGM]")]
    [SerializeField] AudioSource BGMAudioSurce = null;
    [SerializeField] public AudioClip titleAudioClip = null;

    [Header("[Audio SFX]")]
    [SerializeField] public AudioSource SFXSource = null;
    [SerializeField] public AudioClip butSourceClip = null;

    [Header("[Audio Volume Control]")]
    [SerializeField] AudioMixer myAudioMixer = null;

    float bgmValue;
    float sfxValue;

    private void Awake()
    {
        BGMAudioSurce.playOnAwake = true;
        BGMAudioSurce.loop = true;

        TitleBGMPlay();
    }

    public void BGMControl(Slider BGMSlider)
    {
        bgmValue = BGMSlider.value;

        if (bgmValue == -40f)
            myAudioMixer.SetFloat("BGM", -80);
        else
            myAudioMixer.SetFloat("BGM", bgmValue);
    }

    public void SFXControl(Slider ButSfxSlider)
    {
        sfxValue = ButSfxSlider.value;

        if (sfxValue == -40f)
            myAudioMixer.SetFloat("SFX", -80);
        else
            myAudioMixer.SetFloat("SFX", sfxValue);
    }

    public void TitleBGMPlay()
    {
        BGMAudioSurce.clip = titleAudioClip;
        BGMAudioSurce.Play();
    }

    public void ButtonSFXPlay()
    {
        SFXSource.clip = butSourceClip;
        SFXSource.Play();
    }

    public void ToggleAduioVolume()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }
}