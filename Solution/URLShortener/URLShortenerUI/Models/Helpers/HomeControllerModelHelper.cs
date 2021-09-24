using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortenerDomainLayer.Models;
using UrlShortenerUI.Models.Home;

namespace UrlShortenerUI.Models.Helpers
{
    public class HomeControllerModelHelper : IHomeControllerModelHelper
    {
        public UrlDomainModel GetUrlDomainModel(UrlRegisterViewModel registerModel)
        {
            TimeSpan expireSpan = registerModel.ExpireIn switch
            {
                ExpireIn.ThreeDays => TimeSpan.FromDays(3),
                ExpireIn.OneWeek => TimeSpan.FromDays(7),
                ExpireIn.TwoWeeks => TimeSpan.FromDays(14),
                _ => throw new ArgumentException("Invalid enum state")
            };
            DateTime expireDate = DateTime.Now.Add(expireSpan);
            return new()
            {
                ShortUrl = registerModel.ShortUrl,
                LongUrl = registerModel.LongUrl,
                ExpireDateTime = expireDate,
            };
        }

        public UrlRedirectViewModel GetUrlRedirectViewModel(UrlDomainModel domainModel)
        {
            return new()
            {
                Url = domainModel.LongUrl
            };
        }

        public UrlRegistrationSuccessfulViewModel GetUrlRegistrationSuccessfulViewModel(
            UrlRegisterViewModel model,
            HttpContext context)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = context.Request.Scheme,
                Host = context.Request.Host.Host
            };
            uriBuilder.Port = context.Request.Host.Port ?? uriBuilder.Port;
            uriBuilder.Path = model.ShortUrl;
            var url = uriBuilder.Uri.ToString();
            return new()
            {
                Url = url,
            };
        }
    }
}
