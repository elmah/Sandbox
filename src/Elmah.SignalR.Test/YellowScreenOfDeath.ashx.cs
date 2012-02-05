﻿using System.Web;

namespace Elmah.SignalR.Test
{
    /// <summary>
    /// Summary description for YellowScreenOfDeath
    /// </summary>
    public class YellowScreenOfDeath : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var errorId = context.Request.QueryString["id"] ?? string.Empty;

            if (errorId.Length == 0)
                return;

            var response = context.Response;

            var error = ErrorsStore.Store.GetError(errorId);
            if (error == null)
            {
                // TODO: Send error response entity
                response.Status = "404 Not Found";
                return;
            }

            if (error.webHostHtmlMessage.Length == 0)
                return;

            response.Write(error.webHostHtmlMessage);
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}