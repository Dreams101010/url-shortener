using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortenerDomainLayer.Interfaces;

namespace UrlShortenerDomainLayer.Decorators.Query
{
    public class CachingQueryDecorator<TParam, TOutput> : IQuery<TParam, TOutput>
    {
        private IQuery<TParam, TOutput> Decoratee { get; set; }
        private ICache CacheService { get; set; }

        public CachingQueryDecorator(IQuery<TParam, TOutput> decoratee,
            ICache cacheService)
        {
            this.Decoratee = decoratee;
            this.CacheService = cacheService;
        }

        public TOutput Execute(TParam param)
        {
            var key = CacheService.GetKey(param);
            TOutput result;
            if (CacheService.Has(key))
            {
                result = CacheService.Get<TOutput>(key);
            }
            else
            {
                result = Decoratee.Execute(param);
                if (result is not null)
                {
                    CacheService.Set(key, result);
                }
            }
            return result;
        }
    }
}
