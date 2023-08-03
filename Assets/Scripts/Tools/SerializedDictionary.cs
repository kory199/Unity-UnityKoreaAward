using System.Collections;
using System.Collections.Generic;
/// <summary>
/// This Dictionary is Purposed for serialization in Unity's inspector view.
/// It works similarly to a dictionary and can be used the same way.
/// _by OneChance
/// </summary>
/// <typeparam name="Tkeys"></typeparam> 
/// <typeparam name="Tvalues"></typeparam>
[System.Serializable]
public class SerializedDictionary<Tkeys, Tvalues> 
{
    public List<Tkeys> Keys;
    public List<Tvalues> Values;
    private Dictionary<Tkeys, Tvalues> _dictionary;
    public SerializedDictionary()
    {
        Keys = new List<Tkeys>();
        Values = new List<Tvalues>();
        _dictionary = new Dictionary<Tkeys, Tvalues>();
    }
    public void Add(Tkeys key, Tvalues value)
    {
        if (_dictionary.ContainsKey(key))
        {
            return;
        }
        Keys.Add(key);
        Values.Add(value);
        _dictionary.Add(key, value);
    }
    public bool Remove(Tkeys key)
    {
        Tvalues values;
        if (!_dictionary.TryGetValue(key, out values))
        {
            return false;
        }
        _dictionary.Remove(key);
        int idx = Keys.IndexOf(key);
        Keys.Remove(key);
        Values.Remove(values);
        return true;
    }
    public void Clear()
    {
        _dictionary.Clear();
        Keys.Clear();
        Values.Clear();
    }
    public bool TryGetValue(Tkeys key, out Tvalues value)
    {
        return _dictionary.TryGetValue(key, out value);
    }
    public bool ContainKey(Tkeys key)
    {
        return _dictionary.ContainsKey(key);
    }
    public bool ContainsValue(Tvalues value)
    {
        return ContainsValue(value);
    }
    public int Count()
    {
        return _dictionary.Count;
    }
    public IEnumerator GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }
    public IEqualityComparer<Tkeys> Comparer()
    {
        return _dictionary.Comparer;
    }
    public void TryAdd(Tkeys key, Tvalues value)
    {
        bool IsAdded = _dictionary.TryAdd(key, value);
        if (IsAdded == true)
        {
            Keys.Add(key);
            Values.Add(value);
        }
    }
    public Tvalues this[Tkeys key]
    {
        get
        {
            if (_dictionary.ContainsKey(key)) 
            {
                int idx = Keys.IndexOf(key);
                if (idx >= 0) return Values[idx];
                throw new KeyNotFoundException();
            }
            else
                throw new KeyNotFoundException();

        }
        set
        {
            if (_dictionary.ContainsKey(key)) 
            {
                return;
            }
            else
            {
                _dictionary[key] = value;
                Keys.Add(key);
                Values.Add(value);
            }
        }
    }
}