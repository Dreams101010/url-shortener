using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URLShortenerUI.Models.Helpers;
using URLShortenerUI.Models.Home;
using URLShortenerDomainLayer.Services;

namespace URLShortenerUI.Controllers
{
    public class HomeController : Controller
    {
        URLService UrlService { get; set; }
        IHomeControllerModelHelper ModelHelper { get; set; }

        public HomeController(URLService urlService,
            IHomeControllerModelHelper modelHelper)
        {

            this.UrlService = urlService ?? throw new ArgumentNullException(nameof(urlService));
            ModelHelper = modelHelper ?? throw new ArgumentNullException(nameof(modelHelper));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(URLRegisterViewModel urlToRegister)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                var domainModel = ModelHelper.GetURLDomainModel(urlToRegister);
                UrlService.AddURL(domainModel);
            }
            catch
            {
                return View("Error");
            }
            var registerSuccessModel = ModelHelper
                .GetURLRegistrationSuccessfulViewModel(urlToRegister, HttpContext);
            return View("RegisterSuccess", registerSuccessModel);
        }

        public IActionResult RedirectTo(string shortUrl)
        {
            try
            {
                var urlModel = UrlService.GetURLByShortURL(shortUrl);
                if (urlModel.ExpireDateTime > DateTime.Now) // URL has expired
                {
                    return View(urlModel.LongUrl);
                }
                else
                {
                    return View("Error");
                }
            }
            catch
            {
                return View("Error");
            }
        }
    }
}
