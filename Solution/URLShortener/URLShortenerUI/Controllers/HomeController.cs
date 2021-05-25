using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URLShortenerUI.Models.Helpers;
using URLShortenerUI.Models.Home;
using URLShortenerDomainLayer.Services;
using AspNetCore.ReCaptcha;

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
        [ValidateReCaptcha]
        public IActionResult Register(URLRegisterViewModel urlToRegister)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var exists = UrlService.GetURLByShortURL(urlToRegister.ShortUrl) is not null;
            if (exists)
            {
                ModelState.AddModelError("ShortUrl", "Short URL is already taken");
                return View();
            }
            var domainModel = ModelHelper.GetURLDomainModel(urlToRegister);
            UrlService.AddURL(domainModel);
            var registerSuccessModel = ModelHelper
                .GetURLRegistrationSuccessfulViewModel(urlToRegister, HttpContext);
            return View("RegisterSuccess", registerSuccessModel);
        }

        public IActionResult RedirectTo(string shortUrl)
        {
            var urlModel = UrlService.GetURLByShortURL(shortUrl);
            if (urlModel is null)
            {
                return View("Error");
            }
            if (!urlModel.HasExpired()) // URL has not expired
            {
                var redirectModel = ModelHelper.GetURLRedirectViewModel(urlModel);
                return View("Redirect", redirectModel);
            }
            else
            {
                UrlService.RemoveUrlByShortUrl(shortUrl);
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
    }
}
