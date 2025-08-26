using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ERP.Web.Helpers
{
    public class ServicioProxyExterno
    {

        private HttpClient client = new HttpClient();

        public ServicioProxyExterno()
        {

            client.BaseAddress = new Uri("http://servicios.codetus.com/api/");

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public HttpResponseMessage Call<T>(string url, T objectRequest, string metodo = "POST")
        {
            HttpResponseMessage objRpt = new HttpResponseMessage();

            if (metodo == "GET")
            {
                var getTask = client.GetAsync(url + "/" + objectRequest);

                var _result = getTask.Result;

                if (_result.IsSuccessStatusCode)
                {
                    objRpt = _result;
                }

                return objRpt;
            }
            else
            {

                var postTask = client.PostAsJsonAsync<T>(url, objectRequest);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    objRpt = result;
                }

                return objRpt;
            }
        }
    }
}