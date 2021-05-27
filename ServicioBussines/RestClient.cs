using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bussines
{
    public class RestClient
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(RestClient));
        public static string SendToService(string url, string stringToSend, Dictionary<string, string> headers, Encoding enc)
        {
            try
            {
                url = url.Replace("[dollar]", "$");
                var content = stringToSend;
                WebClient wc = new WebClient();
                foreach (var key in headers.Keys)
                {
                    wc.Headers.Add(key, headers[key]);
                }
                wc.Encoding = enc;
                string res = string.Empty;
                if (content == string.Empty)
                {
                    res = wc.UploadString(url, string.Empty);
                }
                else
                {
                    res = wc.UploadString(url, "POST", content);
                }
                return res;
            }
            catch (WebException we)
            {
                var response = we.Response as HttpWebResponse;
                if (response != null && response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Logger.Error("Error al autenticar", we);
                }
                else Logger.Error(we);

                return "error";
            }
        }

        public static string GetFromService(string url, Dictionary<string, string> headers, Encoding enc)
        {
            try
            {
                if (headers == null)
                {
                    headers = new Dictionary<string, string>();
                    headers.Add("Accept-Language", "es-ES");
                    //headers.Add("Authorization", "Basic " + Convert.ToBase64String(enc.GetBytes($"{user}:{pass}")));
                }

                var sw = Stopwatch.StartNew();
                url = url.Replace("[dollar]", "$");
                WebClient wc = new WebClient();
                foreach (var key in headers.Keys)
                {
                    wc.Headers.Add(key, headers[key]);
                }
                wc.Encoding = enc;
                string res = wc.DownloadString(url);
                Logger.Debug($"Called: {url} ");
                Logger.Debug($"Lasted: {sw.Elapsed.TotalSeconds} seconds");
                return res;
            }
            catch (WebException we)
            {
                var response = we.Response as HttpWebResponse;
                if (response != null && response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Logger.Error("Error al autenticar", we);
                }
                else
                {
                    Logger.Error(we);
                }
                return "Error";
                throw;
            }

        }


    }
}
