using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EffectInfo
{
    public GameObject sfxObj;
    public EnumTypes.EffectType effectType;
}

[CreateAssetMenu(fileName = "EffectSO", menuName = "ScriptableObjects/EffectSO", order = int.MaxValue)]
public class EffectSO : ScriptableObject
{
    [SerializeField] public List<EffectInfo> EffectList = new List<EffectInfo>();
}