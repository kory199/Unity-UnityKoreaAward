using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LogoMove : MonoBehaviour
{
    [SerializeField] GameObject accountUI = null;
    private RawImage _logo;

    private float moveDistance = 50f;
    private float moveDuration = 1.5f;

    private void Awake()
    {
        _logo = GetComponent<RawImage>();
        accountUI.SetActive(false);
        StartCoroutine(Move());
    }

    public IEnumerator Move()
    {
        float elapsedTime = 0;
        Vector3 startingPosition = _logo.rectTransform.position;

        Vector3 targetPositionDown = startingPosition - new Vector3(0, moveDistance, 0);

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            _logo.rectTransform.position = Vector3.Lerp(startingPosition, targetPositionDown, elapsedTime / moveDuration);
            yield return null;
        }

        _logo.rectTransform.position = targetPositionDown;

        yield return new WaitForSeconds(0.2f);

        elapsedTime = 0;
        Vector3 targetPositionUp = startingPosition + new Vector3(0, moveDistance, 0);

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            _logo.rectTransform.position = Vector3.Lerp(targetPositionDown, targetPositionUp, elapsedTime / moveDuration);
            yield return null;
        }

        _logo.rectTransform.position = targetPositionUp;

        accountUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}