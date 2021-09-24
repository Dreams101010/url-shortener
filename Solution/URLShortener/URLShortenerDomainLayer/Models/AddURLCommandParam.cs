using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortenerDomainLayer.Models
{
    public class AddUrlCommandParam
    {
        public UrlDomainModel UrlToAdd { get; set; }
    }
}
