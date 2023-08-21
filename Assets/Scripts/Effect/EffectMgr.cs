using System.Collections.Generic;
using UnityEngine;

public class EffectMgr : MonoSingleton<EffectMgr>
{
    private EffectSO effectSO = null;
    private List<GameObject> effectObjects = new List<GameObject>();
    private int _cloneCount = 10;

    private void Awake()
    {
        if (effectSO == null)
        {
            effectSO = Resources.Load<EffectSO>("EffectSO");
        }

        PreloadEffects();
    }

    private void PreloadEffects()
    {
        foreach (var effect in effectSO.EffectList)
        {
            for (int i = 0; i < _cloneCount; i++)
            {
                GameObject instance = Instantiate(effect.sfxObj);
                instance.SetActive(false);
                effectObjects.Add(instance);
            }
        }
    }

    public GameObject CreateEffect(EnumTypes.EffectType type, Vector3 position, Transform parent = null)
    {
        GameObject effectPrefab = effectSO.EffectList.Find(e => e.effectType == type).sfxObj;

        if (effectPrefab)
        {
            GameObject existingEffect = null;

            if (parent != null)
            {
                foreach (Transform child in parent)
                {
                    if (child.name == effectPrefab.name + "(Clone)") 
                    {
                        existingEffect = child.gameObject;
                        break;
                    }
                }
            }

            if (existingEffect)
            {
                return existingEffect;
            }

            GameObject pooledEffect = GetEffectFromPool(effectPrefab);

            if (existingEffect == null)
            {
                existingEffect = Instantiate(effectPrefab);
                effectObjects.Add(existingEffect);
            }

            existingEffect.transform.SetParent(parent);
            existingEffect.transform.position = position;
            existingEffect.SetActive(true);

            return existingEffect;
        }

        return null;
    }

    private GameObject GetEffectFromPool(GameObject effectPrefab)
    {
        foreach (var effect in effectObjects)
        {
            if (!effect.activeInHierarchy && effect.name == effectPrefab.name)
            {
                return effect;
            }
        }
        return null;
    }

    public void DeactivateEffect(GameObject effectInstance)
    {
        effectInstance.SetActive(false);
    }
}