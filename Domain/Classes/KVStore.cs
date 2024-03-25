namespace Valmar.Domain.Implementation.Drops;

public class KVStore<TKey, TData>(TKey key, Func<TKey, Task<TData>> retrieval)
{
    private TData _data;
    private bool _dirty = true;

    public async Task<TData> Retrieve()
    {
        if (_dirty) _data = await retrieval.Invoke(key);
        _dirty = false;
        return _data;
    }
    
    public void Dirty() => _dirty = true;
    public void Clean(TData data) => _data = data;
}