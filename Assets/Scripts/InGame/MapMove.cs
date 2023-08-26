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
        _sequence.
            Append(gameObject.transform.DORotate(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), Random.Range(0.2f, 5f)))
            .Append(gameObject.transform.DOScale(Random.Range(21, 70f), Random.Range(0.2f, 5f)))
            .Append(gameObject.transform.DORotate(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), Random.Range(0.2f, 5f)))
            .Join(gameObject.transform.DOScale(Random.Range(21, 70f), Random.Range(0.2f, 5f)));

        _sequence.SetLoops(-1);

            
    }
}
