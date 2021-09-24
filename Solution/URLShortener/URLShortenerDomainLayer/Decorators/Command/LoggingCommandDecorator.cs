using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortenerDomainLayer.Interfaces;
using Microsoft.Extensions.Logging;

namespace UrlShortenerDomainLayer.Decorators.Command
{
    // The purpose of this decorator is to log the command execution.
    public class LoggingCommandDecorator<TParam, TOutput> : ICommand<TParam, TOutput>
    {
        private ICommand<TParam, TOutput> Decoratee { get; set; }
        private ILogger<LoggingCommandDecorator<TParam, TOutput>> Logger { get; set; }
        public LoggingCommandDecorator(ICommand<TParam, TOutput> decoratee,
            ILogger<LoggingCommandDecorator<TParam, TOutput>> logger)
        {
            this.Decoratee = decoratee;
            this.Logger = logger;
        }

        public TOutput Execute(TParam param)
        {
            Logger.LogInformation($"Executing command with parameter of type {param.GetType()}");
            var result = Decoratee.Execute(param);
            return result;
        }
    }
}
