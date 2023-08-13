using UnityEngine;
using UnityEngine.UI;

public class SoundValueTest_W : MonoBehaviour
{
    [SerializeField] Slider masterSlider = null;
    [SerializeField] Slider bgmSlider = null;
    [SerializeField] Slider sfxSlider = null;

    [SerializeField] Button createBtn = null;
    [SerializeField] Button loginBtn = null;

    void Awake()
    {
        masterSlider.onValueChanged.AddListener(OnChangedMasterControl);
        bgmSlider.onValueChanged.AddListener(OnChangedBGMControl);
        sfxSlider.onValueChanged.AddListener(OnChangedSFXControl);

        createBtn.onClick.AddListener(() => SoundTest_W.Instance.ButtonSFXPlay());
        loginBtn.onClick.AddListener(() => SoundTest_W.Instance.ButtonSFXPlay());
    }

    private void OnChangedMasterControl(float sound)
    {
        SoundTest_W.Instance.MasterControl(masterSlider);
    }

    private void OnChangedBGMControl(float sound)
    {
        SoundTest_W.Instance.BGMControl(bgmSlider);
    }

    private void OnChangedSFXControl(float sound)
    {
        SoundTest_W.Instance.SFXControl(sfxSlider);
    }
}