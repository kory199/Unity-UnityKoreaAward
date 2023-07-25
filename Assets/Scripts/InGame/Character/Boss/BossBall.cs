using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBall : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(Co_ReturnToPool());
    }
    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }
    IEnumerator Co_ReturnToPool()
    {
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);
    }
}
