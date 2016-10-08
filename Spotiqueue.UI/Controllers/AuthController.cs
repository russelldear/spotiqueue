using System.Web.Mvc;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System.Configuration;

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
                RedirectUri = "http://localhost/Spotiqueue.UI/Auth",
                Scope = Scope.PlaylistModifyPrivate,
            };

            var verificationCode = Request.QueryString["code"];

            var token = auth.ExchangeAuthCode(verificationCode, ConfigurationManager.AppSettings["SpotifyClientSecret"]);

            var settings = ConfigurationManager.AppSettings["Settings"];

            System.IO.File.WriteAllLines(settings, new string[] { token.AccessToken });
        }
    }
}