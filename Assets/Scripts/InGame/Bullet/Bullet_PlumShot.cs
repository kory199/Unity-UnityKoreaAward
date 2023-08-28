using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_PlumShot : MonoBehaviour
{
    Action callback;
    GameObject _spawner;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnEnable()
    {
        StartCoroutine(Co_BulletMove(_spawner));
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void InsertPool(Action action,GameObject spanwer)
    {
        callback = action;
        _spawner = spanwer;
    }
    bool _isWall = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            _isWall = true;
        }
        else if (collision.tag == "Monster")
        {
            if (collision.name == "BasicMeleeMonster")
            {
                collision.GetComponent<MeleeMonster>().Hit();
                callback();
                gameObject.SetActive(false);
            }
            else if (collision.name == "BossOne")
            {
                collision.GetComponent<BossOne>().BossHit(2);
                callback();
                gameObject.SetActive(false);
            }
        }
    }
    IEnumerator Co_BulletMove( GameObject spanwer)
    {
        Vector3 dir = (gameObject.transform.position - spanwer.transform.position).normalized;
        while (_isWall != false)
        {
            gameObject.transform.Translate(dir);
            yield return null;
        }
        callback();
        _isWall = false;
        gameObject.SetActive(false);
    }
}
