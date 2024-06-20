namespace tobeh.Valmar.Server.Domain.Classes;

public class KeyValueStore<TKey, TValue>(TKey key, Func<TKey, Task<TValue>> retrieval)
{
    private TValue? _data;
    private bool _dirty = true;
    private readonly SemaphoreSlim _fetchLock = new(1, 1);

    public async Task<TValue> Retrieve()
    {
        if (_dirty || _data is null)
        {
            await _fetchLock.WaitAsync();
            if (_dirty || _data is null)
            {
                try
                {
                    _data = await retrieval.Invoke(key);
                }
                catch (Exception e)
                {
                    _fetchLock.Release();
                    throw;
                }
            }

            _fetchLock.Release();
        }

        _dirty = false;
        return _data;
    }

    public void Dirty() => _dirty = true;
    public void Clean(TValue data) => _data = data;
}