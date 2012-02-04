﻿using System;
using System.Web;

namespace Elmah.SignalR.Test
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            ErrorsStore.Store
                .AddSource(
                    "Wasp is doing ElmahR", 
                    "The fool on the hill")
                .AddSource(
                    "Wasp is doing ElmahR again",
                    "Strawberry fields forever");
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}