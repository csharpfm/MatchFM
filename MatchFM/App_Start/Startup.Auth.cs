using System;
using System.Threading.Tasks;
using MatchFM;
using MatchFM.Models;
using MatchFM.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace MatchFM
{
    /// <summary>
    /// Configuration class for authentication, get access_token from request
    /// </summary>
    /// <seealso cref="Microsoft.Owin.Security.OAuth.OAuthBearerAuthenticationProvider" />
    public class QueryStringOAuthBearerProvider : OAuthBearerAuthenticationProvider
    {
        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            var value = context.Request.Query.Get("access_token");

            if (!string.IsNullOrEmpty(value))
            {
                context.Token = value;
            }

            return Task.FromResult<object>(null);
        }
    }

    /// <summary>
    /// Add authentication provider on startup
    /// </summary>
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext<ApplicationDbContext>(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/oauth/token"),
                Provider = new ApplicationOAuthProvider(),
                AuthorizeEndpointPath = new PathString("/oauth/authorize"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(90),
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthAuthorizationServer(OAuthOptions);

            var OAuthBearerOptions = new OAuthBearerAuthenticationOptions()
            {
                Provider = new QueryStringOAuthBearerProvider(),
                AccessTokenProvider = new AuthenticationTokenProvider()
                {
                    OnCreate = create,
                    OnReceive = receive
                },
            };

            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
        }

        public static Action<AuthenticationTokenCreateContext> create = new Action<AuthenticationTokenCreateContext>(c =>
        {
            c.SetToken(c.SerializeTicket());
        });

        public static Action<AuthenticationTokenReceiveContext> receive = new Action<AuthenticationTokenReceiveContext>(c =>
        {
            c.DeserializeTicket(c.Token);
            c.OwinContext.Environment["Properties"] = c.Ticket.Properties;
        });
    }
}