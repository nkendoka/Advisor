// MRUCache.cs
using System;
using System.Collections.Generic;

public class MRUCache<TKey, TValue>
{
    private readonly int _capacity;
    private readonly Dictionary<TKey, TValue> _cache;
    private readonly LinkedList<TKey> _accessOrder;

    public MRUCache(int capacity = 5)
    {
        _capacity = capacity;
        _cache = new Dictionary<TKey, TValue>();
        _accessOrder = new LinkedList<TKey>();
    }

    public TValue Get(TKey key)
    {
        if (_cache.ContainsKey(key))
        {
            _accessOrder.Remove(key);
            _accessOrder.AddFirst(key);
            return _cache[key];
        }
        else
        {
            throw new KeyNotFoundException("Key not found in cache.");
        }
    }

    public void Put(TKey key, TValue value)
    {
        if (_cache.ContainsKey(key))
        {
            _accessOrder.Remove(key); 
        }
        else if (_cache.Count >= _capacity)
        {
            TKey lastKey = _accessOrder.Last.Value;
            _cache.Remove(lastKey);
            _accessOrder.RemoveLast();
        }

        _cache[key] = value;
        _accessOrder.AddFirst(key);
    }

    public void Delete(TKey key)
    {
        if (_cache.ContainsKey(key))
        {
            _cache.Remove(key);
            _accessOrder.Remove(key);
        }
    }
}
