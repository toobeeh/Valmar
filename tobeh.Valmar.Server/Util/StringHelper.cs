namespace tobeh.Valmar.Server.Util;

public static class StringHelper
{
    public static string Repeat(this string text, int n)
    {
        var textAsSpan = text.AsSpan();
        var span = new Span<char>(new char[textAsSpan.Length * (int)n]);
        for (var i = 0; i < n; i++)
        {
            textAsSpan.CopyTo(span.Slice((int)i * textAsSpan.Length, textAsSpan.Length));
        }

        return span.ToString();
    }
}