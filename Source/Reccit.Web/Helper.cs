using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.IO;

namespace Reccit.Web
{
    public class Helper
    {
        public string GetData(string url, Type.RequestType type )
        {
            var response = string.Empty;
            HttpWebResponse webResponse = null;
            Stream responseStream = null;
            StreamReader reader = null;
            try
            {

                HttpWebRequest request = CreateWebRequest(url, type);

                webResponse = (HttpWebResponse)request.GetResponse();
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    string message = String.Format("POST failed. Received HTTP {0}", webResponse.StatusCode);
                    //throw new ApplicationException(message);
                    
                }

                // grab the response
                responseStream = webResponse.GetResponseStream();
                reader = new StreamReader(responseStream);
                response = reader.ReadToEnd();


                return response;
            }
            catch { }
            finally
            {
                if (webResponse != null)
                {
                    webResponse.Close();
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                    responseStream.Dispose();
                }
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            return response;
        }

     private static HttpWebRequest CreateWebRequest(string endPoint, Type.RequestType type)
     {
            var request = (HttpWebRequest)WebRequest.Create(endPoint);
            request.Method = type.ToString();         
            request.ContentLength = 0;
            request.ContentType = "application/json; charset=utf-8";
            return request;
        }
    }

}
