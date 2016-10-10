using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using Spotiqueue.Shared;
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
                ClientId = Settings.SpotifyClientId,
                RedirectUri = Settings.RedirectUri,
                Scope = Scope.PlaylistModifyPrivate,
            };

            var verificationCode = Request.QueryString["code"];

            var token = auth.ExchangeAuthCode(verificationCode, Settings.SpotifyClientSecret);

            var tokenFile = Settings.TokenFile;

            System.IO.File.WriteAllLines(tokenFile, new string[] { token.RefreshToken });
        }
    }
}