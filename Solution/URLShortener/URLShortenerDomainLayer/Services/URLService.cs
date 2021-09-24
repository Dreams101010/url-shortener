using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortenerDomainLayer.Models;
using UrlShortenerDomainLayer.Interfaces;

namespace UrlShortenerDomainLayer.Services
{
    public class UrlService
    {
        private ICommand<AddUrlCommandParam, bool> AddCommand { get; }
        private IQuery<GetUrlByShortUrlQueryParam, UrlDomainModel> GetUrlByShortUrlQuery { get; }
        private ICommand<RemoveUrlByShortUrlCommandParam, bool> RemoveCommand { get; }

        public UrlService(
            ICommand<AddUrlCommandParam, bool> addCommand,
            IQuery<GetUrlByShortUrlQueryParam, UrlDomainModel> getUrlByShortUrlQuery,
            ICommand<RemoveUrlByShortUrlCommandParam, bool> removeCommand)
        {
            AddCommand = 
                addCommand ?? throw new ArgumentNullException(nameof(addCommand));
            GetUrlByShortUrlQuery = 
                getUrlByShortUrlQuery ?? throw new ArgumentNullException(nameof(getUrlByShortUrlQuery));
            RemoveCommand = removeCommand ?? throw new ArgumentNullException(nameof(removeCommand));
        }

        public void AddUrl(UrlDomainModel url)
        {
            if (url is null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            AddUrlCommandParam param = new()
            {
                UrlToAdd = url,
            };
            AddCommand.Execute(param);
        }

        public UrlDomainModel GetUrlByShortUrl(string shortUrl)
        {
            if (shortUrl is null)
            {
                throw new ArgumentNullException(nameof(shortUrl));
            }

            GetUrlByShortUrlQueryParam param = new()
            {
                ShortUrl = shortUrl,
            };
            return GetUrlByShortUrlQuery.Execute(param);
        }

        public bool RemoveUrlByShortUrl(string shortUrl)
        {
            if (shortUrl is null)
            {
                throw new ArgumentNullException(nameof(shortUrl));
            }

            RemoveUrlByShortUrlCommandParam param = new()
            {
                ShortUrl = shortUrl,
            };
            return RemoveCommand.Execute(param);
        }
    }
}
