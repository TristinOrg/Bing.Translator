using System;
using System.IO;
using System.Net;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Xml;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;

namespace WebTranslator
{
    [DataContract]
    class Response
    {
        [DataMember]
        public int statusCode { get; set; }
        [DataMember]
        public string translationResponse { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string inputStr = string.Empty;
            do
            {
                Console.WriteLine("Type what you want to translate (from English to German)");
                inputStr = Console.ReadLine();
                Console.WriteLine("Translation: "+ RequestUrl(inputStr));
            }
            while(!inputStr.Equals("quit"));
        }

        static string RequestUrl(string sourceTxt)
        {
            string requestUrl = "https://cn.bing.com/ttranslate";
            WebRequest request = WebRequest.Create(requestUrl);

            var postData = string.Format("text={0}&", sourceTxt);
            postData += "from=en&"; //from english
            postData += "to=de";    //to german

            var data = Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(responseString));
            DataContractJsonSerializer dcjs = new DataContractJsonSerializer(typeof(Response));
            Response rs = (Response)dcjs.ReadObject(ms);
            return rs.translationResponse;
        }
    }
}
