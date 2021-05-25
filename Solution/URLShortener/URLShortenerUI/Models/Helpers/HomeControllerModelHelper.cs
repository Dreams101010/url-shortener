using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URLShortenerDomainLayer.Models;
using URLShortenerUI.Models.Home;

namespace URLShortenerUI.Models.Helpers
{
    public class HomeControllerModelHelper : IHomeControllerModelHelper
    {
        public URLDomainModel GetURLDomainModel(URLRegisterViewModel registerModel)
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

        public URLRegistrationSuccessfulViewModel GetURLRegistrationSuccessfulViewModel(
            URLRegisterViewModel model,
            HttpContext context)
        {
            var uriBuilder = new UriBuilder();
            uriBuilder.Scheme = context.Request.Scheme;
            uriBuilder.Host = context.Request.Host.Host;
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
