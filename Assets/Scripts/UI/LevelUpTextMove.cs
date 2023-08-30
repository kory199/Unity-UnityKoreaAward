using System.Collections;
using TMPro;
using UnityEngine;

public class LevelUpTextMove : MonoBehaviour
{
    private TextMeshProUGUI levelupText;

    [SerializeField] private float moveDistance = 50f;
    [SerializeField] private float moveDuration = 0.8f;
    [SerializeField] private AnimationCurve moveCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

    private Vector3 initialPosition;

    private void Awake()
    {
        levelupText = GetComponent<TextMeshProUGUI>();
        initialPosition = levelupText.transform.position;
    }

    private void OnEnable()
    {
        StartCoroutine(MoveTextUpwards());
    }

    private IEnumerator MoveTextUpwards()
    {
        float startTime = 0f;

        while (startTime <= moveDuration)
        {
            float yOffset = moveCurve.Evaluate(startTime / moveDuration) * moveDistance;
            levelupText.transform.position = initialPosition + new Vector3(0, yOffset, 0);

            startTime += Time.unscaledDeltaTime;
            yield return null;
        }

        levelupText.transform.position = initialPosition + new Vector3(0, moveDistance, 0);
        this.gameObject.SetActive(false);
    }
}