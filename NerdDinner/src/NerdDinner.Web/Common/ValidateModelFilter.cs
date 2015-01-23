﻿using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json.Linq;

namespace NerdDinner.Web.Common
{
    /// <summary>
    /// Validate Model Filter
    /// </summary>
    public class ValidateModelFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Check for validity of model state
        /// </summary>
        /// <param name="context">action context</param>
        /// <param name="next">the next delegate</param>
        /// <returns></returns>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var errors = new JObject();
                foreach (var key in context.ModelState.Keys)
                {
                    var state = context.ModelState[key];
                    if (state.Errors.Count > 0)
                    {
                        errors[key] = state.Errors[0].ErrorMessage;
                    }
                }

                context.Result = new JsonResult(errors);
            }
            else
            { 
                await next();
            }
        }
    }
}