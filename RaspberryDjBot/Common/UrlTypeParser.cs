using System;

namespace RaspberryDjBot.Common
{
    public static class UrlTypeParser
    {
        public static UrlType TryParser(string text, out Uri uri)
        {
            uri = null;
            if (text.StartsWith("https://www.youtube.com"))
            {
                if (Uri.TryCreate(text, UriKind.Absolute, out uri))
                    return UrlType.Youtube;
            }

            return UrlType.None;
        }
    }
}
