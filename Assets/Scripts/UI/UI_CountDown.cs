using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class UI_CountDown : UIBase
{
    [SerializeField] Sprite[] countNums = null;
    [SerializeField] TextMeshProUGUI countDownImg = null;
    [SerializeField] Image countDownBG = null;
    [SerializeField] AnimationCurve PosXCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 7f), new Keyframe(0.9f, -1.5f) });
    [SerializeField] AnimationCurve ScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0.1f), new Keyframe(0.9f, 1f) });

    Vector3 curPos;
    Vector3 curScale;
    RectTransform rectTransform;
    RectTransform rectTransformBG;

    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;
    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }

    protected override void Awake()
    {
        rectTransform = countDownImg.GetComponent<RectTransform>();
        rectTransformBG = countDownBG.GetComponent<RectTransform>();
        StartCoroutine(RunCountdown());
    }

    private void OnEnable()
    {
        Time.timeScale = 1;
        StartCoroutine(RunCountdown());
        StartCoroutine(Co_BGTurn());
        
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
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransformBG.anchoredPosition = Vector2.zero;
        countDownImg.text = number.ToString();
        StartCoroutine(nameof(ShowCountDown));
    }

    private IEnumerator ShowCountDown()
    {
        float startTime = 0;
        while (ScaleCurve.keys[ScaleCurve.keys.Length - 1].time >= startTime)
        {
            curPos.x = PosXCurve.Evaluate(startTime);
            rectTransform.anchoredPosition = curPos;
            rectTransformBG.anchoredPosition = curPos;

            curScale = Vector3.one * ScaleCurve.Evaluate(startTime);
            rectTransform.localScale = curScale;
            rectTransformBG.localScale = curScale*6;

            rectTransformBG.Rotate(Vector3.forward * 2);


            startTime += Time.deltaTime;

            yield return null;
        }
    }
    private IEnumerator Co_BGTurn()
    {
        while(true)
        {
            rectTransformBG.Rotate(Vector3.forward * 1);
            yield return null;
        }
    }
}