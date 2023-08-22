using System.Collections;
using TMPro;
using UnityEngine;

public class TimerTest_W : MonoBehaviour
{
    TextMeshProUGUI timer;

    private float time = 0f;

    private void Awake()
    {
        timer = this.GetComponent<TextMeshProUGUI>();
        StartCoroutine(TimerCoroutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateSkillUIObj();
        }
    }

    private void updatetimerFormat()
    {
        time += Time.deltaTime;

        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);

        timer.text = $"Time : {minutes.ToString()}:{seconds.ToString("00")}";
    }

    private IEnumerator TimerCoroutine()
    {
        while(true)
        {
            updatetimerFormat();
            yield return null;
        }
    }

    private void CreateSkillUIObj()
    {
        UI_Enhance uI_Enhance = null;

        if(uI_Enhance == null)
        {
            uI_Enhance = UIManager.Instance.CreateObject
                <UI_Enhance>("UI_Enhance", EnumTypes.LayoutType.First);
        }

        uI_Enhance.OnShow();
    }
}