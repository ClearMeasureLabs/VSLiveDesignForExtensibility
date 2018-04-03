using System;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Services;
using System.Web;
using System.Web.Mvc;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.Core.Services;
using UI.Models;

namespace ClearMeasure.Bootcamp.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly Bus _bus;
        private readonly IUserSession _session;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AccountController(Bus bus, IUserSession session)
        {
            _bus = bus;
            _session = session;
        }

        public ActionResult Login()
        {
            Log.Info("Visitor viewed login page");
            _session.LogOut();
            return View();
        }

        [HttpPost]
        public RedirectResult Login(string returnUrl)
        {
            var client = new AuthenticationApiClient(
                new Uri(string.Format("https://{0}", ConfigurationManager.AppSettings["auth0:Domain"])));


            var request = this.Request;
            var redirectUri = new UriBuilder(request.Url.Scheme, request.Url.Host, this.Request.Url.IsDefaultPort ? -1 : request.Url.Port, "LoginCallback.ashx");

            var authorizeUrlBuilder = client.BuildAuthorizationUrl()
                .WithClient(ConfigurationManager.AppSettings["auth0:ClientId"])
                .WithRedirectUrl(redirectUri.ToString())
                .WithResponseType(AuthorizationResponseType.Code)
                .WithScope("openid profile")
                // adding this audience will cause Auth0 to use the OIDC-Conformant pipeline
                // you don't need it if your client is flagged as OIDC-Conformant (Advance Settings | OAuth)
                .WithAudience("https://" + @ConfigurationManager.AppSettings["auth0:Domain"] + "/userinfo");

            if (!string.IsNullOrEmpty(returnUrl))
            {
                var state = "ru=" + HttpUtility.UrlEncode(returnUrl);
                authorizeUrlBuilder.WithState(state);
            }

            return new RedirectResult(authorizeUrlBuilder.Build().ToString());
//            Employee employee = _bus.Send(new EmployeeByUserNameQuery(model.UserName)).Result;
//            _session.LogIn(employee);
        }

        public ActionResult Logout()
        {
            _session.LogOut();

            FederatedAuthentication.SessionAuthenticationModule.SignOut();

            // Redirect to Auth0's logout endpoint
            var returnTo = Url.Action("Login", "Account", null, protocol: Request.Url.Scheme);
            return this.Redirect(
                string.Format(CultureInfo.InvariantCulture,
                    "https://{0}/v2/logout?returnTo={1}&client_id={2}",
                    ConfigurationManager.AppSettings["auth0:Domain"],
                    this.Server.UrlEncode(returnTo),
                    ConfigurationManager.AppSettings["auth0:ClientId"]));
        }
    }
}
