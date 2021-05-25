using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShortenerDomainLayer.Models;
using URLShortenerDomainLayer.Interfaces;

namespace URLShortenerDomainLayer.Services
{
    public class URLService
    {
        private ICommand<AddURLCommandParam, bool> AddCommand { get; }
        private IQuery<GetURLByShortURLQueryParam, URLDomainModel> GetUrlByShortUrlQuery { get; }
        private ICommand<RemoveUrlByShortUrlCommandParam, bool> RemoveCommand { get; }

        public URLService(
            ICommand<AddURLCommandParam, bool> addCommand,
            IQuery<GetURLByShortURLQueryParam, URLDomainModel> getUrlByShortUrlQuery,
            ICommand<RemoveUrlByShortUrlCommandParam, bool> removeCommand)
        {
            AddCommand = 
                addCommand ?? throw new ArgumentNullException(nameof(addCommand));
            GetUrlByShortUrlQuery = 
                getUrlByShortUrlQuery ?? throw new ArgumentNullException(nameof(getUrlByShortUrlQuery));
            RemoveCommand = removeCommand ?? throw new ArgumentNullException(nameof(removeCommand));
        }

        public void AddURL(URLDomainModel url)
        {
            if (url is null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            AddURLCommandParam param = new()
            {
                URLToAdd = url,
            };
            AddCommand.Execute(param);
        }

        public URLDomainModel GetURLByShortURL(string shortUrl)
        {
            if (shortUrl is null)
            {
                throw new ArgumentNullException(nameof(shortUrl));
            }

            GetURLByShortURLQueryParam param = new()
            {
                ShortURL = shortUrl,
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
