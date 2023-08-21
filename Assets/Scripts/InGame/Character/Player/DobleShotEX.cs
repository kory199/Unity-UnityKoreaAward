using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
interface ISkillBase
{
    void SkillLevelUp();
    void SkillShot();
    KeyCode Shotkey { get;set; }
}
public class DobleShotEX : MonoBehaviour, ISkillBase
{
    GameObject obj;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<Player>();
        GameObject spawner = new GameObject();
        spawner.transform.position = gameObject.transform.position;
        obj = Instantiate(spawner, gameObject.transform);
        //obj.damage = player.attackpower*2;
    }
    KeyCode _shotKey;


    public void GetKeyCode(KeyCode keyCode)
    {
        _shotKey = keyCode;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_shotKey))
        {
            if (obj.activeSelf == false)
            {
                obj.SetActive(true);
                obj.transform.DOMoveX(0, 1);
            }
        }
    }

    public void SkillLevelUp()
    {
        throw new System.NotImplementedException();
    }

    public void SkillShot()
    {
        throw new System.NotImplementedException();
    }
    public KeyCode Shotkey { get => _shotKey; set { _shotKey = value; } }

}
