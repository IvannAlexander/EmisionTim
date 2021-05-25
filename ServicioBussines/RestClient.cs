using log4net;
using System;
using System.Collections.Generic;
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
                var res = wc.UploadString(url, "POST", content);
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
    }
}
