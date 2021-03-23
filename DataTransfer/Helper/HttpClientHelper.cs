using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DataTransfer.Helper
{
    public class HttpClientHelper
    {
        private string _credentials;
        private Uri _uri;
        public HttpClientHelper(string credentials, Uri uri)
        {
            _credentials = credentials;
            _uri = uri;

        }

        //public Task<string> GetAsync()
        //{

        //    var client = new HttpClient();
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _credentials);

        //    //connect to the REST endpoint            
        //    HttpResponseMessage response = client.GetAsync(_uri).Result;

        //    //check to see if we have a successful response
        //    if (response.IsSuccessStatusCode)
        //        return response.Content.ReadAsStringAsync();
        //    else
        //    {
        //        //if(response.StatusCode == HttpStatusCode.NotFound)
        //        //    return response.Content.ReadAsStringAsync();
        //        //else
        //        throw new Exception(response.StatusCode.ToString());

        //    }


        //}

        public async Task<Response> GetAsync()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _credentials);

            HttpResponseMessage response = client.GetAsync(_uri).Result;

            //check to see if we have a successful response
            Response r = new Response();

            r.StatusCode = (int)response.StatusCode;
            r.IsSuccessStatusCode = response.IsSuccessStatusCode;

            //get the headers
            IEnumerable<string> values;
            r.ContinuationToken = null;
            if (response.Headers.TryGetValues("x-ms-continuationtoken", out values))
            {
                r.ContinuationToken = values.FirstOrDefault();
            }

            if (response.IsSuccessStatusCode)
                r.Content = await response.Content.ReadAsStringAsync();
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }

            return r;

        }
    }

    public class Response
    {
        public string Content { get; set; }
        public int StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public string ContinuationToken { get; set; }

    }
}
