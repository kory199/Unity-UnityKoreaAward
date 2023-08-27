using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_PlumShot : MonoBehaviour
{
    Action callback;
    GameObject _player;
    GameObject _spawner;
   
    private void OnEnable()
    {
        if (_player != null)
            StartCoroutine(Co_BulletMove(_player,_spawner));
    }
    private void OnDisable()
    {
        callback();
    }
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
                gameObject.SetActive(false);
            }
            else if (collision.name == "BossOne")
            {
                collision.GetComponent<BossOne>().BossHit(2);
                gameObject.SetActive(false);
            }
        }
    }
    public void InsertPool(Action action, GameObject spanwer, GameObject player)
    {
        callback = action;
        _spawner = spanwer;
        _player = player;
    }
    [SerializeField] bool _isWall = false;

  
    IEnumerator Co_BulletMove(GameObject player,GameObject spawner)
    {
        Vector3 dir = (player.transform.position - spawner.transform.position).normalized;
        while (_isWall == false)
        {
            gameObject.transform.Translate(dir * Time.deltaTime * 4);
            yield return null;
        }
        _isWall = false;
        gameObject.SetActive(false);
    }
}
