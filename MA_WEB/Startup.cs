using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http.ExceptionHandling;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Microsoft.Owin.Cors;
using WebGrease;

[assembly: OwinStartup(typeof(MA_WEB.Startup))]

namespace MA_WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", map =>
            {
                // Setup the CORS middleware to run before SignalR.
                // By default this will allow all origins. You can 
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    // You can enable JSONP by uncommenting line below.
                    // JSONP requests are insecure but some older browsers (and some
                    // versions of IE) require JSONP to work cross domain
                    // EnableJSONP = true
                    EnableDetailedErrors = true,
                    EnableJavaScriptProxies = true
                };

                var authorizer = new QueryStringBearerAuthorizeAttribute();
                var module = new AuthorizeModule(authorizer, authorizer);
                GlobalHost.HubPipeline.AddModule(module);


                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch already runs under the "/signalr" path.
                map.RunSignalR(hubConfiguration);

            });
           // app.MapSignalR();
            ConfigureAuth(app);
        }
        public class QueryStringBearerAuthorizeAttribute : AuthorizeAttribute
        {
            public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
            {
                var _Authorization = request.QueryString.Get("Bearer");
                if (!string.IsNullOrEmpty(_Authorization))
                {
                    var ticket = Startup.OAuthOptions.AccessTokenFormat.Unprotect(_Authorization);

                    if (ticket != null && ticket.Identity != null && ticket.Identity.IsAuthenticated)
                    {
                        request.Environment["server.User"] = new ClaimsPrincipal(ticket.Identity);
                        return true;
                    }
                }
                return false;
            }

            public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext,
                bool appliesToMethod)
            {
                var connectionId = hubIncomingInvokerContext.Hub.Context.ConnectionId;
                var request = hubIncomingInvokerContext.Hub.Context.Request;
                var _Authorization = request.QueryString.Get("Bearer");
                if (!string.IsNullOrEmpty(_Authorization))
                {
                    //var token = _Authorization.Replace("Bearer ", "");
                    var ticket = Startup.OAuthOptions.AccessTokenFormat.Unprotect(_Authorization);

                    if (ticket != null && ticket.Identity != null && ticket.Identity.IsAuthenticated)
                    {
                        Dictionary<string, object> _DCI = new Dictionary<string, object>();
                        _DCI.Add("server.User", new ClaimsPrincipal(ticket.Identity));
                        hubIncomingInvokerContext.Hub.Context = new HubCallerContext(new ServerRequest(_DCI), connectionId);
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Глобальный логгер исключений
        /// </summary>
        internal class GlobalExceptionLogger : ExceptionLogger
        {
            public override void Log(ExceptionLoggerContext context)
            {
               /* LogManager.
                LogManager.GetLogger(typeof(WebApiApplication)).ErrorFormat("Unhandled exception thrown in {0} for request {1}: {2}",
                                            context.Request.Method, context.Request.RequestUri, context.Exception);*/
            }
        }
    }
}
