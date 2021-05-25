using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

namespace URLShortenerUI.Filters
{
    public class ExceptionRedirectFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var controllerName = context.RouteData.Values["controller"].ToString();
            System.Diagnostics.Debug.WriteLine(controllerName);
            var result = new RedirectToActionResult("Error", "Home", null);
            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}