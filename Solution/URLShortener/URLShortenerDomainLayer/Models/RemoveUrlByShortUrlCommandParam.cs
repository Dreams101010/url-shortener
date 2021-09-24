using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortenerDomainLayer.Models
{
    public class RemoveUrlByShortUrlCommandParam
    {
        public string ShortUrl { get; set; }
    }
}
