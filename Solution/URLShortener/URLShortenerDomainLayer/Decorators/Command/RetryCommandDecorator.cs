using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShortenerDomainLayer.Interfaces;

namespace URLShortenerDomainLayer.Decorators.Command
{
    public class RetryCommandDecorator<TParam, TOutput> : ICommand<TParam, TOutput>
    {
        private ICommand<TParam, TOutput> Decoratee { get; set; }
        public RetryCommandDecorator(ICommand<TParam, TOutput> decoratee)
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
