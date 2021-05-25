using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URLShortenerUI.Models.Home;
using Microsoft.AspNetCore.Mvc.Routing;

namespace URLShortenerUI.Models.Helpers
{
    public class URLRegistrationSuccessfulModelHelper
    {
        public static URLRegistrationSuccessfulViewModel GetURLRegistrationSuccessfulViewModel(
            URLRegisterViewModel model)
        {
            return new()
            {
                ShortURL = model.ShortUrl,
            };
        }
    }
}
