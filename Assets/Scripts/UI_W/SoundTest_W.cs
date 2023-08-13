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

    private void Awake()
    {
        BGMAudioSurce.playOnAwake = true;
        BGMAudioSurce.loop = true;

        TitleBGMPlay();
    }

    public void MasterControl(Slider masterSlider)
    {
        float masterValue = masterSlider.value;

        if (masterValue == -40f)
            myAudioMixer.SetFloat("MasterSoundValue", -80);
        else
            myAudioMixer.SetFloat("MasterSoundValue", masterValue);
    }

    public void BGMControl(Slider BGMSlider)
    {
        float bgmValue = BGMSlider.value;

        if (bgmValue == -40f)
            myAudioMixer.SetFloat("BGMSoundValue", -80);
        else
            myAudioMixer.SetFloat("BGMSoundValue", bgmValue);
    }

    public void SFXControl(Slider ButSfxSlider)
    {
        ButtonSFXPlay();

        float sfxValue = ButSfxSlider.value;
        float test = sfxValue == -40f ? -80 : sfxValue;
        Debug.Log($"SFX Slider Value: {sfxValue}, Mixer Value: {test}");

        if (sfxValue == -40f)
            myAudioMixer.SetFloat("SFXSoundValue", -80);
        else
            myAudioMixer.SetFloat("SFXSoundValue", sfxValue);
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