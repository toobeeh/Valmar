namespace Valmar.Domain.Classes;

public class KeyValueStore<TKey, TValue>(TKey key, Func<TKey, Task<TValue>> retrieval)
{
    private TValue? _data;
    private bool _dirty = true;

    public async Task<TValue> Retrieve()
    {
        if (_dirty || _data is null) _data = await retrieval.Invoke(key);
        _dirty = false;
        return _data;
    }
    
    public void Dirty() => _dirty = true;
    public void Clean(TValue data) => _data = data;
}