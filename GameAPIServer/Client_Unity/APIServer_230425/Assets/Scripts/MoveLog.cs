using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveLog : MonoBehaviour
{
    Image logImg = null;
    [SerializeField] AnimationCurve scaleCurve = null;

    void Awake()
    {
        logImg = this.GetComponent<Image>();
        scaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0.5f), new Keyframe(1f, 1.5f), new Keyframe(1.5f, 1f) });
    }

    IEnumerator Start()
    {
        float time = 0f;

        Vector3 curScale = Vector3.zero;

        while (scaleCurve.keys[scaleCurve.keys.Length - 1].time >= time)
        {

            curScale = Vector3.one * scaleCurve.Evaluate(time);
            transform.localScale = curScale;

            time += Time.deltaTime;
            yield return null;
        }

        
        Loading_UIMgr.Inst.SetTermofUsePanel(true);
    }
}