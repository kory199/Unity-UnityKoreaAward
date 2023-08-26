using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MapMove : MonoBehaviour
{
    Sequence _sequence = null;
    private void Awake()
    {
       
        _sequence = DOTween.Sequence();
        _sequence.Append(gameObject.transform.DOMoveX(Random.Range(1, 5), 3))
            .Append(gameObject.transform.DORotate(Vector3.forward * Random.Range(1, 360), 0.5f))
            .Append(gameObject.transform.DOScale(Random.Range(2, 5.5f), 2))
            .Join(gameObject.transform.DORotate(Vector3.forward * Random.Range(-1, -360), 2))
            .Append(gameObject.transform.DOScale(Random.Range(2, 5.5f), 2))
            .Append(gameObject.transform.DORotate(Vector3.zero, 3))
            .Join(gameObject.transform.DOScale(1, 3))
            .Append(gameObject.transform.DOMove(Vector3.zero, 2));

        _sequence.SetLoops(-1);

            
    }
}
