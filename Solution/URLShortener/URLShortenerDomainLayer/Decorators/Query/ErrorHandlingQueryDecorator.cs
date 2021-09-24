using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortenerDomainLayer.Interfaces;
using Microsoft.Extensions.Logging;

namespace UrlShortenerDomainLayer.Decorators.Query
{
    public class ErrorHandlingQueryDecorator<TParam, TOutput> : IQuery<TParam, TOutput>
    {
        private IQuery<TParam, TOutput> Decoratee { get; set; }
        private ILogger<ErrorHandlingQueryDecorator<TParam, TOutput>> Logger { get; set; }
        public ErrorHandlingQueryDecorator(IQuery<TParam, TOutput> decoratee,
            ILogger<ErrorHandlingQueryDecorator<TParam, TOutput>> logger)
        {
            this.Decoratee = decoratee;
            this.Logger = logger;
        }
        public TOutput Execute(TParam param)
        {
            TOutput result;
            try
            {
                result = Decoratee.Execute(param);
            }
            catch (Exception ex)
            {
                Logger.LogError("Exception has occured: {ex}", ex);
                throw;
            }
            return result;
        }
    }
}
