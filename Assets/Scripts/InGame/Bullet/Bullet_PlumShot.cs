using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_PlumShot : MonoBehaviour
{
    Action callback;
    GameObject _player;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnEnable()
    {
        if (_player != null)
            StartCoroutine(Co_BulletMove(_player));
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void OnDisable()
    {
        callback();
    }
    public void InsertPool(Action action, GameObject spanwer)
    {
        callback = action;
        _player = spanwer;
    }
    [SerializeField] bool _isWall = false;

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
    IEnumerator Co_BulletMove(GameObject player)
    {
        Vector3 dir = (gameObject.transform.position - player.transform.position).normalized;
        while (_isWall == false)
        {
            Debug.Log(dir);
            gameObject.transform.Translate(dir * Time.deltaTime * 4);
            yield return null;
        }
        _isWall = false;
        gameObject.SetActive(false);
    }
}
