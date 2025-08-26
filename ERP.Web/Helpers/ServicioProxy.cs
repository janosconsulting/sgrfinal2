using ERP.Web.Models;
using Mantenimiento.Datos.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace ERP.Web.Helpers
{
    public class ServicioProxy
    {
      
        private HttpClient client = new HttpClient();

        public ServicioProxy()
        {

           
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["urlServicio"]);
          
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public ServicioProxy(string url)
        {
            client.BaseAddress = new Uri(url);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public void setBaseAdress(string url)
        {
            this.client.BaseAddress = new Uri(url);
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