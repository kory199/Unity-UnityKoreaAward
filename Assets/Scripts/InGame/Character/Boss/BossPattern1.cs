using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 보스 옆에 투사체를 발사할 오브젝트 풀러들이 갖고 있어야 하는 보스 패턴 스크립트.
/// </summary>
public class BossPattern1 : MonoBehaviour
{
    [SerializeField] private GameObject _boss;
    [SerializeField] private Vector3 _initMoveVector;
    [SerializeField] private float _rad;
    [SerializeField] private float _initDegree;
    [SerializeField] private float lotationSpeed = 0.01f;
    [SerializeField] private float lotationSpeed2 = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 tempVector = _boss.transform.position - gameObject.transform.position;
        _initDegree = Mathf.Atan2(tempVector.x, tempVector.y);
        _initMoveVector = gameObject.transform.position - _boss.transform.position;
        _rad = Vector3.Distance(gameObject.transform.position, _boss.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //ball은 보스의 투사체 오브젝트 
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("pattern1");
            GameObject ball = ObjectPooler.SpawnFromPool("Ball", gameObject.transform.position);
            StartCoroutine(CO_MoveForward(ball, _initMoveVector));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("pattern2");
            GameObject ball = ObjectPooler.SpawnFromPool("Ball", gameObject.transform.position);
            Vector3 tempDir = gameObject.transform.position - _boss.transform.position;
            StartCoroutine(CO_MoveForward(ball, tempDir));
            StartCoroutine(CO_MoveSpawner());
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("pattern3");
            GameObject ball = ObjectPooler.SpawnFromPool("Ball", gameObject.transform.position);
            Vector3 tempDir = gameObject.transform.position - _boss.transform.position;
            StartCoroutine(CO_MoveForward(ball, tempDir));
            StartCoroutine(CO_halfMoveSpawner(_initDegree));
        }
    }
    /// <summary>
    /// 투사체를 직선으로 날리는 로직 , 보스 1단계 패턴
    /// </summary>
    /// <param name="ball"></param>
    /// <returns></returns>
    IEnumerator CO_MoveForward(GameObject ball, Vector3 dir)
    {
        int i = 0;
        while (i < 1000)
        {
            yield return null;
            ball.transform.Translate(dir.normalized * 2);
            i++;
        }
    }
    /// <summary>
    /// 패턴 2
    /// </summary>
    /// <returns></returns>
    IEnumerator CO_MoveSpawner()
    {
        float i = 0;
        float transedDegree = _initDegree;
        while (true)
        {
            if (i > 360)
            {
                i = 0;
                transedDegree -= 360;
            }
            yield return null;
            Vector3 newPos = Vector3.zero;
            newPos.x = _boss.transform.position.x + _rad * Mathf.Cos(transedDegree);
            newPos.y = _boss.transform.position.y + _rad * Mathf.Sin(transedDegree);

            gameObject.transform.position = newPos;
            transedDegree += lotationSpeed;
            i += lotationSpeed;
        }
    }
    /// <summary>
    /// 패턴3
    /// </summary>
    /// <returns></returns>
    IEnumerator CO_halfMoveSpawner(float nowRad)
    {
        float i = 0;
        float transedDegree = nowRad;
        while (true)
        {
            if (i > 2)
            {
                i = 0;
                transedDegree -= 360;
                break;
            }
            yield return null;
            Vector3 newPos = Vector3.zero;
            newPos.x = _boss.transform.position.x + _rad * Mathf.Cos(transedDegree);
            newPos.y = _boss.transform.position.y + _rad * Mathf.Sin(transedDegree);

            gameObject.transform.position = newPos;
            transedDegree += lotationSpeed;
            i += lotationSpeed;
        }
        Vector2 tempVector = _boss.transform.position - gameObject.transform.position;
        float newRad = Mathf.Atan2(tempVector.x, tempVector.y);

        StartCoroutine(CO_halfMoveSpawnerRevers(newRad));
    }
    IEnumerator CO_halfMoveSpawnerRevers(float nowRad)
    {
        float i = 0;
        float transedDegree = nowRad;
        while (true)
        {
            if (i > 2)
            {
                i = 0;
                transedDegree -= 360;
                yield break;
            }
            yield return null;
            Vector3 newPos = Vector3.zero;
            newPos.x = _boss.transform.position.x - _rad * Mathf.Cos(transedDegree);
            newPos.y = _boss.transform.position.y + _rad * Mathf.Sin(transedDegree);

            gameObject.transform.position = newPos;
            transedDegree += lotationSpeed;
            i += lotationSpeed;
        }
        Vector2 tempVector = _boss.transform.position - gameObject.transform.position;
        float newRad = Mathf.Atan2(tempVector.x, tempVector.y);

        StartCoroutine(CO_halfMoveSpawner(newRad));
    }
}
