using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShortenerDomainLayer.Interfaces;

namespace URLShortenerDomainLayer.Decorators.Query
{
    // The purpose of this decorator is to retry the query for a few times in case of error
    public class RetryQueryDecorator<TParam, TOutput> : IQuery<TParam, TOutput>
    {
        private IQuery<TParam, TOutput> Decoratee { get; set; }
        public RetryQueryDecorator(IQuery<TParam, TOutput> decoratee)
        {
            this.Decoratee = decoratee;
        }

        public TOutput Execute(TParam param)
        {
            int retryCounter = 0;
            TOutput result;
            while (true)
            {
                try
                {
                    result = Decoratee.Execute(param);
                    return result;
                }
                catch
                {
                    if (retryCounter >= 4)
                    {
                        throw;
                    }
                    else
                    {
                        retryCounter++;
                    }
                }
            }
        }
    }
}
