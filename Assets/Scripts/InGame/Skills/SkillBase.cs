using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
interface ISkillBase
{
    void SkillLevelUp();
    void SkillShot();
}
public abstract class SkillBase : MonoBehaviour, ISkillBase
{
    [SerializeField] protected Player _player;
    protected int _skillLevel = 1;
    protected float _coolTime = 10f;
    protected bool _isCool = false;
    protected KeyCode _shotKey;
    [SerializeField] protected UI_SceneGame _uI_SceneGame;
    public UI_SceneGame UI_SceneGame { get { return _uI_SceneGame; } set { _uI_SceneGame = value; } }
    public KeyCode ShotKey { get { return _shotKey; } set { _shotKey = value; } }

    #region Unity LifeCycle
    protected virtual void Start()
    {
        _player = gameObject.GetComponent<Player>();
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if (Input.GetKeyDown(_shotKey))
        {
            if (_isCool == false)
            {
                _isCool = true;

                SkillShot();

                _uI_SceneGame.SetCoolTime(_shotKey, _coolTime, () => _isCool = false);
            }
        }
    }

    #endregion
    /// <summary>
    /// _skillLevel++ 써주시고 스킬 레벨업시 사용될 기능 자유 구현
    /// </summary>
    public abstract void SkillLevelUp();
    /// <summary>
    /// 스킬 로직 자유롭게 작성해주세요
    /// </summary>
    public abstract void SkillShot();
    /// <summary>
    /// 스킬 쿨타임 _coolTime 여기에 float형의 시간 할당 해주세요 
    /// </summary>
    public abstract void SkillCoolTime();
}