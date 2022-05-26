using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Net.Https;
using Crestron.SimplSharp.Net.Http;


namespace crestron_lwrf_gen2
{
    public static class HttpRequests
    {
        public static HttpClient Client = new HttpClient();
        public static HttpsClient ClientSecure = new HttpsClient();

        static HttpRequests()
        {
            Client = new HttpClient();
            ClientSecure = new HttpsClient();

            Client.TimeoutEnabled = true;
            Client.Timeout = 5;
            Client.KeepAlive = false;

            ClientSecure.TimeoutEnabled = true;
            ClientSecure.Timeout = 5;
            ClientSecure.KeepAlive = false;
            ClientSecure.PeerVerification = false;
            ClientSecure.HostVerification = false;
        }

        public static void PostAsync(string url, string content, List<HttpsHeader> headers, HTTPSClientResponseCallback response)
        {
            try
            {

                HttpsClientRequest Request;
                Request = new HttpsClientRequest();
                Request.ContentString = content;

                foreach (var header in headers)
                {
                    Request.Header.AddHeader(header);
                }

                Request.RequestType = Crestron.SimplSharp.Net.Https.RequestType.Post;

                Request.Url.Parse(url);
                ClientSecure.DispatchAsync(Request, response);
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in HttpRequests.PostAsync: {0}", e.Message);
            }
        }

        public static string Post(string url, string content, List<HttpsHeader> headers)
        {
            try
            {

                HttpsClientRequest Request;
                Request = new HttpsClientRequest();
                Request.ContentString = content;

                foreach (var header in headers)
                {
                    Request.Header.AddHeader(header);
                }

                Request.RequestType = Crestron.SimplSharp.Net.Https.RequestType.Post;

                Request.Url.Parse(url);
                ClientSecure.Dispatch(Request);

                HttpsClientResponse response = ClientSecure.Dispatch(Request);

                return response.ContentString;
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in HttpRequests.Postc: {0}", e.Message);
                return "";
            }
        }

        public static void GetAsync(string url, List<HttpsHeader> headers, HTTPSClientResponseCallback response)
        {
            try
            {

                HttpsClientRequest Request;
                Request = new HttpsClientRequest();
                Request.ContentString = "";

                foreach (var header in headers)
                {
                    Request.Header.AddHeader(header);
                }

                Request.RequestType = Crestron.SimplSharp.Net.Https.RequestType.Get;

                Request.Url.Parse(url);
                ClientSecure.DispatchAsync(Request, response);
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in HttpRequests.GetAsync: {0}", e.Message);
            }
        }

        public static string Get(string url, List<HttpsHeader> headers)
        {
            try
            {

                HttpsClientRequest Request;
                Request = new HttpsClientRequest();
                Request.ContentString = "";

                foreach (var header in headers)
                {
                    Request.Header.AddHeader(header);
                }

                Request.RequestType = Crestron.SimplSharp.Net.Https.RequestType.Get;

                Request.Url.Parse(url);
                HttpsClientResponse response = ClientSecure.Dispatch(Request);

                return response.ContentString;
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in HttpRequests.Get: {0}", e.Message);
                return "";
            }
        }

        public static void PatchAsync(string url, string content, List<HttpsHeader> headers, HTTPSClientResponseCallback response)
        {
            try
            {

                HttpsClientRequest Request;
                Request = new HttpsClientRequest();
                Request.ContentString = content;

                foreach (var header in headers)
                {
                    Request.Header.AddHeader(header);
                }

                Request.RequestType = Crestron.SimplSharp.Net.Https.RequestType.Patch;

                Request.Url.Parse(url);
                ClientSecure.DispatchAsync(Request, response);
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in HttpRequests.PatchAsync: {0}", e.Message);
            }
        }

        public static string Patch(string url, string content, List<HttpsHeader> headers)
        {
            try
            {

                HttpsClientRequest Request;
                Request = new HttpsClientRequest();
                Request.ContentString = content;

                foreach (var header in headers)
                {
                    Request.Header.AddHeader(header);
                }

                Request.RequestType = Crestron.SimplSharp.Net.Https.RequestType.Patch;

                Request.Url.Parse(url);

                HttpsClientResponse response = ClientSecure.Dispatch(Request);

                return response.ContentString;
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in HttpRequests.Patch: {0}", e.Message);
                return "";
            }
        }

        public static void DeleteAsync(string url, List<HttpsHeader> headers, HTTPSClientResponseCallback response)
        {
            try
            {

                HttpsClientRequest Request;
                Request = new HttpsClientRequest();
                Request.ContentString = "";

                foreach (var header in headers)
                {
                    Request.Header.AddHeader(header);
                }

                Request.RequestType = Crestron.SimplSharp.Net.Https.RequestType.Delete;

                Request.Url.Parse(url);
                ClientSecure.DispatchAsync(Request, response);
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in HttpRequests.DeleteAsync: {0}", e.Message);
            }
        }

        public static string Delete(string url, List<HttpsHeader> headers)
        {
            try
            {

                HttpsClientRequest Request;
                Request = new HttpsClientRequest();
                Request.ContentString = "";

                foreach (var header in headers)
                {
                    Request.Header.AddHeader(header);
                }

                Request.RequestType = Crestron.SimplSharp.Net.Https.RequestType.Delete;

                Request.Url.Parse(url);
                HttpsClientResponse response = ClientSecure.Dispatch(Request);

                return response.ContentString;
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in HttpRequests.Delete: {0}", e.Message);
                return "";
            }
        }
    }
}