using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace CIS174Final.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
     
            context.Result = new RedirectToActionResult("Error", "Home", new { message = context.Exception.Message });
            context.ExceptionHandled = true;
        }
    }
}