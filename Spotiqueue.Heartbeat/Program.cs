using NLog;
using Spotiqueue.Shared.Services;
using System;

namespace Spotiqueue.Heartbeat
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                var authorisationService = new AuthorisationService();

                authorisationService.Authorise(null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to run heartbeat - " + ex.StackTrace);
            }
        }
    }
}
