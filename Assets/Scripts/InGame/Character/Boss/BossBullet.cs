using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }
}
