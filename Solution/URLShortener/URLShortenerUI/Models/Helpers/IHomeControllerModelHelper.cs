using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortenerDomainLayer.Models;
using UrlShortenerUI.Models.Home;

namespace UrlShortenerUI.Models.Helpers
{
    public interface IHomeControllerModelHelper
    {
        public UrlDomainModel GetUrlDomainModel(UrlRegisterViewModel registerModel);
        public UrlRegistrationSuccessfulViewModel GetUrlRegistrationSuccessfulViewModel(
                    UrlRegisterViewModel model,
                    HttpContext context);
        public UrlRedirectViewModel GetUrlRedirectViewModel(UrlDomainModel domainModel);
    }
}
