using Newtonsoft.Json;
using NLog;
using Spotiqueue.Shared;
using Spotiqueue.UI.Models;
using System;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace Spotiqueue.UI.Controllers
{
    public class HomeController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index(bool? success)
        {
            var defaultModel = new QueueModel { SearchArtists = true, Result = success };
            return View(defaultModel);
        }

        [HttpPost]
        public ActionResult Queue(QueueModel model)
        {
            try
            {
                var apiUrl = Settings.SpotiqueueApiUrl;
                var username = Settings.SpotiqueueUserName;
                var playlistId = Settings.SpotiqueuePlaylistId;

                var searchModel = model.ToSearchModel(username, playlistId);

                var request = (HttpWebRequest)WebRequest.Create(apiUrl);

                request.Method = "POST";
                request.ContentType = "application/json";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    var json = JsonConvert.SerializeObject(searchModel);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    model.Result = true;
                    return RedirectToAction("Index", new { success = true });
                    return View("Index", model);
                }
                else
                {
                    model.Result = false;
                    logger.Error(string.Format("Search request failed: {0} - {1}", response.StatusCode, response.StatusDescription));
                    return View("Index", model);
                }
            }
            catch (Exception ex)
            {
                model.Result = false;
                logger.Error(ex, "Search request failed.");
                return View("Index", model);
            }
        }
    }
}