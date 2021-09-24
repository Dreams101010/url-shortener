using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortenerDomainLayer.Interfaces
{
    /* 
     * Query objects represent non-mutating operations.
     * The result of query operation can be cached.
    */
    public interface IQuery<TParam, TOutput>
    {
        public TOutput Execute(TParam param);
    }
}
