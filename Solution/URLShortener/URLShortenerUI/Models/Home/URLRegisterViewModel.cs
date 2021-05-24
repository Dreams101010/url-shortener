using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace URLShortenerUI.Models.Home
{
    public enum ExpireIn
    {
        [Display(Name = "Three days")]
        ThreeDays,
        [Display(Name = "One week")]
        OneWeek,
        [Display(Name = "Two weeks")]
        TwoWeeks,
    }
    public class URLRegisterViewModel
    {
        [Display(Name = "Short URL")]
        [Required(ErrorMessage = "Short URL is required")]
        [RegularExpression("^[a-zA-Z0-9_-]*$", ErrorMessage = "Short URL contains invalid characters")]
        [MaxLength(15, ErrorMessage = "Short URL is too long. (Max length is 15 characters)")]
        public string ShortUrl { get; set; }

        [Url]
        [Display(Name = "URL")]
        [Required(ErrorMessage = "Long URL is required")]
        [MaxLength(1000, ErrorMessage = "Long URL is too long. (Max length is 1000 characters)")]
        public string LongUrl { get; set; }

        [Required]
        [Display(Name = "Expire in")]
        public ExpireIn ExpireIn { get; set; }
    }
}
