﻿using Newtonsoft.Json;
using Spotiqueue.UI.Models;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace Spotiqueue.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Queue(QueueModel model)
        {
            var apiUrl = ConfigurationManager.AppSettings["SpotiqueueApiUrl"];
            var username = ConfigurationManager.AppSettings["SpotiqueueUserName"];
            var playlistId = ConfigurationManager.AppSettings["SpotiqueuePlaylistId"];

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

            return View("Index");
        }
    }
}