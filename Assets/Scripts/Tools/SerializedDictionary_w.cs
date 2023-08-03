using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KeyValue<Tkey, Tvalue>
{
    public Tkey Key;
    public Tvalue Value;
}

[Serializable]
public class SerializedDictionary_w<Tkey, Tvalue>
{
    [SerializeField]
    private List<KeyValue<Tkey, Tvalue>> items = new List<KeyValue<Tkey, Tvalue>>();
    private Dictionary<Tkey, Tvalue> dictionary = new Dictionary<Tkey, Tvalue>();

    public void Add(Tkey key, Tvalue value)
    {
        if (!dictionary.ContainsKey(key))
        {
            dictionary.Add(key, value);
            items.Add(new KeyValue<Tkey, Tvalue> { Key = key, Value = value });
        }
    }

    public bool Remove(Tkey key)
    {
        if (dictionary.Remove(key))
        {
            items.RemoveAll(item => EqualityComparer<Tkey>.Default.Equals(item.Key, key));
            return true;
        }
        return false;
    }

    public void Clear()
    {
        items.Clear();
        dictionary.Clear();
    }

    public bool TryGetValue(Tkey key, out Tvalue value)
    {
        return dictionary.TryGetValue(key, out value);
    }

    public bool ContainsKey(Tkey key)
    {
        return dictionary.ContainsKey(key);
    }

    public int Count()
    {
        return dictionary.Count;
    }

    public IEnumerator GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }

    public IEqualityComparer<Tkey> Comparer()
    {
        return EqualityComparer<Tkey>.Default;
    }

    public Tvalue this[Tkey key]
    {
        get
        {
            if (dictionary.TryGetValue(key, out Tvalue value))
            {
                return value;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }
        set
        {
            if (!dictionary.ContainsKey(key))
            {
                Add(key, value);
            }
            else
            {
                dictionary[key] = value;
                items.Find(item => EqualityComparer<Tkey>.Default.Equals(item.Key, key)).Value = value;
            }
        }
    }
}