﻿#region License, Terms and Author(s)
//
// ELMAH.Mvc3
// Copyright (c) 2011 Atif Aziz, James Driscoll. All rights reserved.
//
//  Author(s):
//
//      Atif Aziz, http://www.raboof.com
//                 http://stackoverflow.com/questions/766610/how-to-get-elmah-to-work-with-asp-net-mvc-handleerror-attribute
//      James Driscoll
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

namespace Elmah
{
    #region Imports
    using System;
    using System.Web;
    using System.Web.Mvc;
    #endregion

    public class HandleErrorAttribute : System.Web.Mvc.HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);

            if (!context.ExceptionHandled) // if unhandled, will be logged anyhow
                return;

            var e = context.Exception;
            var httpContext = GetCurrentHttpContext(context.HttpContext);
            if (httpContext != null && 
                (RaiseErrorSignal(e, httpContext) // prefer signaling, if possible
                 || IsFiltered(e, httpContext))) // filtered?
                return;

            LogException(e, httpContext);
        }

        private static bool RaiseErrorSignal(Exception e, HttpContext context)
        {
            var signal = ErrorSignal.FromContext(context);
            if (signal == null)
                return false;
            signal.Raise(e, context);
            return true;
        }

        private static ErrorFilterConfiguration _config;
        private static bool IsFiltered(Exception e, HttpContext context)
        {
            if (_config == null)
                _config = context.GetSection("elmah/errorFilter") as ErrorFilterConfiguration ??
                          new ErrorFilterConfiguration();

            var testContext = new ErrorFilterModule.AssertionHelperContext(e, context);
            return _config.Assertion.Test(testContext);
        }

        private static void LogException(Exception e, HttpContext context)
        {
            ErrorLog.GetDefault(context).Log(new Error(e, context));
        }

        private static HttpContext GetCurrentHttpContext(HttpContextBase contextBase)
        {
            // http://stackoverflow.com/questions/1992141/how-do-i-get-an-httpcontext-object-from-httpcontextbase-in-asp-net-mvc-1/4567707#4567707
            var application = (HttpApplication)contextBase.GetService(typeof(HttpApplication));
            return application.Context;
        }
    }
}