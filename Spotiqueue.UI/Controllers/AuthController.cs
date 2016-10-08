using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using System.Configuration;
using System.Web.Mvc;

namespace Spotiqueue.UI.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public void Index()
        {
            var auth = new AutorizationCodeAuth()
            {
                ClientId = ConfigurationManager.AppSettings["SpotifyClientId"],
                RedirectUri = ConfigurationManager.AppSettings["RedirectUri"],
                Scope = Scope.PlaylistModifyPrivate,
            };

            var verificationCode = Request.QueryString["code"];

            var token = auth.ExchangeAuthCode(verificationCode, ConfigurationManager.AppSettings["SpotifyClientSecret"]);

            var settings = ConfigurationManager.AppSettings["Settings"];

            System.IO.File.WriteAllLines(settings, new string[] { token.AccessToken });
        }
    }
}