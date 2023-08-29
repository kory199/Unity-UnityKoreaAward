using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MapFadeOut : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer = null;
    Sequence _sequence = null;
    void Start()
    {
        _sequence = DOTween.Sequence();
        _sequence.Append(gameObject.transform.DOScale(new Vector3(gameObject.transform.localScale.x * 20, gameObject.transform.localScale.y * 20, 0), 2))
            .Join(gameObject.transform.DOMove(new Vector3(40f, -25f, 0), 2))
            .Join(_spriteRenderer.DOColor(new Color(255, 255, 255, 0), 3))
            .AppendCallback(SetActiveFalse);
       
    }
    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
