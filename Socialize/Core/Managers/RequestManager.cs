using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;
using UnifyMe.Core.Classes;

namespace UnifyMe.Core.Managers
{
    public static class RequestManager
    {
        public static ResponseWeather GetWheather(double lon, double lat)
        {
            ResponseWeather response = null;
            try
            {
                string data = "{}";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid=40965dbc493164118603044698b4c0b5");
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(data);
                requestWriter.Close();
                string responseString = GetResponse(request);
                if (string.IsNullOrEmpty(responseString))
                    return new ResponseWeather();

                response = JsonConvert.DeserializeObject<ResponseWeather>(responseString);
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
            }
            catch
            {

            }
            return response;
        }

        public static IList<ItemNews> GetNews(string city)
        {
            IList<ItemNews> details = new List<ItemNews>();

            // httpWebRequest with API URL
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create
            ("http://news.google.com/rss/news?q=" + city + "&output=rss&hl=it&gl=IT&ceid=IT:it");
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == "")
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                
                string data = readStream.ReadToEnd();
                DataSet ds = new DataSet();
                StringReader reader = new StringReader(data);
                ds.ReadXml(reader);
                DataTable dtGetNews = new DataTable();

                if (ds.Tables.Count > 0)
                {
                    dtGetNews = ds.Tables["item"];

                    foreach (DataRow dtRow in dtGetNews.Rows)
                    {
                        ItemNews DataObj = new ItemNews();
                        DataObj.Title = dtRow["title"].ToString();
                        DataObj.Link = dtRow["link"].ToString();
                        DataObj.PubDate = dtRow["pubDate"].ToString();
                        DataObj.Description = Regex.Replace(dtRow["description"].ToString(), "<.*?>", String.Empty);
                        details.Add(DataObj);
                    }
                }
            }
            return details.ToList();
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
