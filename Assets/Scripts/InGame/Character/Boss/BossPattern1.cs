using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern1 : MonoBehaviour
{
    [SerializeField] private GameObject _boss;
    [SerializeField] private Vector3 _moveVector;
    [SerializeField] private float _rad;
    // Start is called before the first frame update
    void Start()
    {
        _moveVector = gameObject.transform.position - _boss.transform.position;
        _rad = Vector3.Distance(gameObject.transform.position, _boss.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("pattern1");
            GameObject ball = ObjectPooler.SpawnFromPool("Ball", gameObject.transform.position);
            StartCoroutine(CO_MoveForward(ball));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("pattern2");
            GameObject ball = ObjectPooler.SpawnFromPool("Ball", gameObject.transform.position);
            StartCoroutine(CO_MoveForward(ball));
            StartCoroutine(CO_MoveSpawner());
        }
    }
    IEnumerator CO_MoveForward(GameObject ball)
    {
        int i = 0;
        while (i < 1000)
        {
            Debug.Log(ball.transform.position);

            yield return null;
            ball.transform.Translate(_moveVector.normalized*2);
            i++;
        }
    }
    IEnumerator CO_MoveSpawner()
    {
        int i = 0;
        while (true)
        {
            if (i > 360) i = 0;
            yield return null;
            Vector3 newPos = Vector3.zero;
            newPos.x = _boss.transform.position.x + _rad * Mathf.Cos(i);
            newPos.y = _boss.transform.position.y + _rad * Mathf.Sin(i);
            
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position,newPos,1f);
            i++;
        }
    }
}
