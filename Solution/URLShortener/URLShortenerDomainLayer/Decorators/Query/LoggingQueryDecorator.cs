using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortenerDomainLayer.Interfaces;
using Microsoft.Extensions.Logging;

namespace UrlShortenerDomainLayer.Decorators.Query
{
    // The purpose of this decorator is to log the query execution.
    public class LoggingQueryDecorator<TParam, TOutput> : IQuery<TParam, TOutput>
    {
        private IQuery<TParam, TOutput> Decoratee { get; set; }
        private ILogger<LoggingQueryDecorator<TParam, TOutput>> Logger { get; set; }
        public LoggingQueryDecorator(IQuery<TParam, TOutput> decoratee,
            ILogger<LoggingQueryDecorator<TParam, TOutput>> logger)
        {
            this.Decoratee = decoratee;
            this.Logger = logger;
        }

        public TOutput Execute(TParam param)
        {
            Logger.LogInformation($"Executing query with parameter of type {param.GetType()}");
            var result = Decoratee.Execute(param);
            return result;
        }
    }
}
