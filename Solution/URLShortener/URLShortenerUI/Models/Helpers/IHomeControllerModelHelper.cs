using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URLShortenerDomainLayer.Models;
using URLShortenerUI.Models.Home;

namespace URLShortenerUI.Models.Helpers
{
    public interface IHomeControllerModelHelper
    {
        public URLDomainModel GetURLDomainModel(URLRegisterViewModel registerModel);
        public URLRegistrationSuccessfulViewModel GetURLRegistrationSuccessfulViewModel(
                    URLRegisterViewModel model,
                    HttpContext context);
        public URLRedirectViewModel GetURLRedirectViewModel(URLDomainModel domainModel);
    }
}
