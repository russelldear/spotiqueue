using System.Diagnostics;
using System.Text;
namespace Spotiqueue.Services
{
    public class AuthorisationModel
    {
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public string State { get; set; }
        public string Scope { get; set; }
        public bool ShowDialog { get; set; }

        public void DoAuth()
        {
            var uri = GetUri();

            var p = new Process
            {
                StartInfo =
                {
                    FileName = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe",
                    UseShellExecute = false,
                    Arguments = uri
                }
            };

            p.Start();
        }

        private string GetUri()
        {
            StringBuilder builder = new StringBuilder("https://accounts.spotify.com/authorize/?");
            builder.Append("client_id=" + ClientId);
            builder.Append("&response_type=code");
            builder.Append("&redirect_uri=" + RedirectUri);
            builder.Append("&state=" + State);
            builder.Append("&scope=" + Scope);
            builder.Append("&show_dialog=" + ShowDialog);
            return builder.ToString();
        }
    }
}