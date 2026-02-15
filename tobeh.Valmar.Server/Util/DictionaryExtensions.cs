namespace tobeh.Valmar.Server.Util;

public static class DictionaryExtensions
{
    public static void AddToList<TKey, TValue>(
        this Dictionary<TKey, List<TValue>> dictionary,
        TKey key,
        TValue value)
    {
        if (!dictionary.TryGetValue(key, out var list))
        {
            list = new List<TValue>();
            dictionary[key] = list;
        }

        list.Add(value);
    }
}