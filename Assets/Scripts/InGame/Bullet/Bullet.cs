using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject playerAim;
    Rigidbody bulletRb;
    Vector2 dirBullet;

    private void Awake()
    {
        BulletInit();
    }

    void Start()
    {
    }

    void Update()
    {
    }

    private void BulletInit()
    {
        if (gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            bulletRb = rb;
        else
            bulletRb = gameObject.AddComponent<Rigidbody>();

        bulletSpeed = 1f;
        DirBullet();
    }

    private void DirBullet()
    {
        
    }

    private void OnEnable()
    {
        bulletRb.velocity = dirBullet * bulletSpeed;
    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }
}
