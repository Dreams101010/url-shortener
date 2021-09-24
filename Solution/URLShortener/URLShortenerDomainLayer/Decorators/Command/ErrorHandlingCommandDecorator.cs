using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortenerDomainLayer.Interfaces;
using Microsoft.Extensions.Logging;

namespace UrlShortenerDomainLayer.Decorators.Command
{
    // The purpose of this decorator is to log the exception and throw it.
    public class ErrorHandlingCommandDecorator<TParam, TOutput> : ICommand<TParam, TOutput>
    {
        private ICommand<TParam, TOutput> Decoratee { get; set; }
        private ILogger<ErrorHandlingCommandDecorator<TParam, TOutput>> Logger { get; set; }
        public ErrorHandlingCommandDecorator(ICommand<TParam, TOutput> decoratee, 
            ILogger<ErrorHandlingCommandDecorator<TParam, TOutput>> logger)
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
