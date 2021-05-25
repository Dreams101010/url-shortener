using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShortenerDomainLayer.Models
{
    public class URLDomainModel
    {
        [Required(ErrorMessage = "Short URL is required")]
        [MaxLength(15, ErrorMessage = "Short URL is too long. (Max length is 15 characters)")]
        public string ShortUrl { get; set; }

        [Required(ErrorMessage = "Long URL is required")]
        [MaxLength(1000, ErrorMessage = "Long URL is too long. (Max length is 1000 characters)")]
        public string LongUrl { get; set; }

        [Required(ErrorMessage = "Expire Date/Time is required.")]
        public DateTime ExpireDateTime { get; set; }

        public bool HasExpired()
        {
            return ExpireDateTime < DateTime.Now;
        }
    }
}
