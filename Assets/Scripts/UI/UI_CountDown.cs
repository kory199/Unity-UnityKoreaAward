using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class UI_CountDown : UIBase
{
    [SerializeField] Sprite[] countNums = null;
    [SerializeField] Image countDownImg = null;
    [SerializeField] AnimationCurve PosXCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 7f), new Keyframe(0.9f, -1.5f) });
    [SerializeField] AnimationCurve ScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0.1f), new Keyframe(0.9f, 1f) });

    Vector3 curPos;
    Vector3 curScale;
    RectTransform rectTransformCount;

    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;
    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }

    protected override void Awake()
    {
        rectTransformCount = countDownImg.GetComponent<RectTransform>();
        StartCoroutine(RunCountdown());
    }

    private void OnEnable()
    {
        Time.timeScale = 1;
        StartCoroutine(RunCountdown());

    }

    IEnumerator RunCountdown()
    {
        for (int i = 3; i > 0; i--)
        {
            Show(i);
            yield return new WaitForSeconds(1f);
        }

        OnHide();
    }

    public void Show(int number)
    {
        rectTransformCount.anchoredPosition = Vector2.zero;

        countDownImg.sprite = countNums[number - 1];
        StartCoroutine(nameof(ShowCountDown));
    }

    private IEnumerator ShowCountDown()
    {
        float startTime = 0;
        while (ScaleCurve.keys[ScaleCurve.keys.Length - 1].time >= startTime)
        {
            curPos.x = PosXCurve.Evaluate(startTime);
            rectTransformCount.anchoredPosition = curPos;

            curScale = Vector3.one * ScaleCurve.Evaluate(startTime);
            rectTransformCount.localScale = curScale * 5;

            startTime += Time.deltaTime;

            yield return null;
        }
    }
}