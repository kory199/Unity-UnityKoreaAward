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
                Bullet_PlumShot bullet = _bullets.Dequeue();
                bullet.InsertPool(() => _bullets.Enqueue(bullet));

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
    private Queue<Bullet_PlumShot> _bullets = new Queue<Bullet_PlumShot>();
    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < 24; i++)
        {
            GameObject spawner = new GameObject();
            spawner.transform.position = new Vector3
            (gameObject.transform.position.x + Mathf.Cos(15 * (i + 1)), gameObject.transform.position.y + Mathf.Cos(15 * (i + 1)), 0);

            _spawners.Add(spawner);
            if ((i % 6) != 0)
                spawner.SetActive(false);

            Bullet_PlumShot bullet = Resources.Load<Bullet_PlumShot>("Bullet/PlumBullet");
            _bullets.Enqueue(bullet);
            Debug.Log(_bullets.Count);
        }
    }
    protected override void Update()
    {
        base.Update();
    }
}
