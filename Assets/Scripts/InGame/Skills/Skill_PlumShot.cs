using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_PlumShot : SkillBase
{
    private float _damage = 10;
    public override void SkillCoolTime()
    {
        _coolTime = 12f;
    }

    public override void SkillLevelUp()
    {
        OpenSpawners();
        _skillLevel++;
        _damage += 5;
    }

    public override void SkillShot()
    {
        //24방향 공격 
        for (int i = 0; i < 24; i++)
        {
            if (_spawners[i].activeSelf == true)
            {
                Debug.Log("매화샷 " + i + " pos  : " + _spawners[i].transform.position + " que : " + _bullets.Count);

                GameObject bullet = _bullets.Dequeue();
                bullet.SetActive(true);
                bullet.transform.position = _spawners[i].transform.position;
            }
        }
    }
    private void OpenSpawners()
    {
        _spawners[0 + _skillLevel].SetActive(true);
        _spawners[6 + _skillLevel].SetActive(true);
        _spawners[12 + _skillLevel].SetActive(true);
        _spawners[18 + _skillLevel].SetActive(true);
    }

    private List<GameObject> _spawners = new List<GameObject>();
    private Queue<GameObject> _bullets = new Queue<GameObject>();
    protected override void Start()
    {
        base.Start();
        GameObject bulletFolder = new GameObject("BulletPlums");
        for (int i = 0; i < 24; i++)
        {
            GameObject spawner = new GameObject("Spawner");
            spawner.transform.SetParent(gameObject.transform);
            spawner.transform.position = new Vector3
            (gameObject.transform.localPosition.x + Mathf.Cos(15 * (i + 1)), gameObject.transform.localPosition.y + Mathf.Sin(15 * (i + 1)), 0);
            _spawners.Add(spawner);

            Bullet_PlumShot bullet = Resources.Load<Bullet_PlumShot>("Bullet/Bullet_Skill_PlumShot");
            GameObject plum = Instantiate(bullet, bulletFolder.transform).gameObject;

            plum.GetComponent<Bullet_PlumShot>().InsertPool(() => _bullets.Enqueue(plum), _spawners[i], gameObject);
            plum.SetActive(false);

            if ((i % 6) != 0)
                spawner.SetActive(false);
        }
    }
    protected override void Update()
    {
        base.Update();
    }
}
