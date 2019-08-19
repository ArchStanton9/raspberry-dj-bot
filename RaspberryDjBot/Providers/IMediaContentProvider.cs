using System;
using System.Threading.Tasks;
using RaspberryDjBot.Common;

namespace RaspberryDjBot.Providers
{
    public interface IMediaContentProvider
    {
        bool TryParseUrl(string text, out Uri url);

        Task<MediaContent> GetMediaContent(Uri url);
    }
}