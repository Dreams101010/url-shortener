using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortenerDomainLayer.Interfaces
{
    /* 
     * Command objects represent mutating operations.
     * Command objects have a return type to return information about operation completion
     * (such as id of inserted entity).
    */
    public interface ICommand<TParam, TOutput> 
    {
        public TOutput Execute(TParam param);
    }
}
