using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBall : MonoBehaviour
{
    Player _player = null;
    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }
    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            _player.PlayerHit(20);
        }
    }
}
