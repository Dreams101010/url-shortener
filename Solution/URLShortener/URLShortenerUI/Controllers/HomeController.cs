using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortenerUI.Models.Helpers;
using UrlShortenerUI.Models.Home;
using UrlShortenerDomainLayer.Services;
using AspNetCore.ReCaptcha;

namespace UrlShortenerUI.Controllers
{
    public class HomeController : Controller
    {
        UrlService UrlService { get; set; }
        IHomeControllerModelHelper ModelHelper { get; set; }

        public HomeController(UrlService urlService,
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
        public IActionResult Register(UrlRegisterViewModel urlToRegister)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var exists = UrlService.GetUrlByShortUrl(urlToRegister.ShortUrl) is not null;
            if (exists)
            {
                ModelState.AddModelError("ShortUrl", "Short URL is already taken");
                return View();
            }
            var domainModel = ModelHelper.GetUrlDomainModel(urlToRegister);
            UrlService.AddUrl(domainModel);
            var registerSuccessModel = ModelHelper
                .GetUrlRegistrationSuccessfulViewModel(urlToRegister, HttpContext);
            return View("RegisterSuccess", registerSuccessModel);
        }

        public IActionResult RedirectTo(string shortUrl)
        {
            var urlModel = UrlService.GetUrlByShortUrl(shortUrl);
            if (urlModel is null)
            {
                return View("Error");
            }
            if (!urlModel.HasExpired()) // URL has not expired
            {
                var redirectModel = ModelHelper.GetUrlRedirectViewModel(urlModel);
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
