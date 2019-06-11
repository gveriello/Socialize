using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using UnifyMe.Core.Classes;

namespace UnifyMe.Core.Managers
{
    public static class RequestManager
    {
        public static ResponseWeather GetWheatherAsync(double lon, double lat)
        {
            string data = "{}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid=40965dbc493164118603044698b4c0b5");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            requestWriter.Write(data);
            requestWriter.Close();
            ResponseWeather response = JsonConvert.DeserializeObject<ResponseWeather>(GetResponse(request));
            if (response != null)
            {
                WebClient client = new WebClient();
                string icon = response?.weather.Select(i => i.icon).First();
                Stream stream = client.OpenRead(new Uri($"http://openweathermap.org/img/w/{icon}.png"));
                response.logo = CreateImageFromStream(stream);
                stream.Flush();
                stream.Close();
                client.Dispose();
            }
            return response;
        }

        private static BitmapImage CreateImageFromStream(Stream stream)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = stream;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            try
            {
                bitmap.Freeze();
            }
            catch { }
            return bitmap;
        }

        private static string GetResponse(HttpWebRequest request)
        {
            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                responseReader.Close();
                return response;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
