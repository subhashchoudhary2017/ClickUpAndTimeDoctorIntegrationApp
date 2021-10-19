using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClickUpIntegration.Helpers
{
    public static class DataHelper<T> where T : class, new()
    {
        public async static Task<Response<T>> Execute(string baseUrl, string route, OperationType type, object payload = null)
        {
            Response<T> response = new Response<T>();
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new Uri(baseUrl)
                };
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage httpResponse = null;
                if (type == OperationType.GET)
                {
                    httpResponse = await client.GetAsync(route);
                }
                else if (type == OperationType.POST)
                {
                    var data = JsonConvert.SerializeObject(payload);
                    var stringContent = new StringContent(data, Encoding.UTF8, "application/json");
                    httpResponse = await client.PostAsync(route, stringContent);
                }
                else if (type == OperationType.DELETE)
                {
                    httpResponse = await client.DeleteAsync(route);
                }
                var result = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Result = JsonConvert.DeserializeObject<T>(result, new IsoDateTimeConverter());
                    //response.Message = "";
                }
                else
                {
                    response.Error = JsonConvert.DeserializeObject<Result>(result, new IsoDateTimeConverter());
                    response.Success = false;
                    //response.Message = "Something went wrong with api calling.";
                }
                return response;
            }
            catch (Exception ex)
            {
                //response.Message = "Error occured!!";
                response.Success = false;
            }
            return response;
        }

        public async static Task<Response<T>> ExecuteWithToken(string baseUrl, string route, OperationType type, string token, object payload = null)
        {
            Response<T> response = new Response<T>();
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new Uri(baseUrl)
                };
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", token);

                HttpResponseMessage httpResponse = null;
                if (type == OperationType.GET)
                {
                    httpResponse = await client.GetAsync(route);
                }
                else if (type == OperationType.POST)
                {
                    var data = JsonConvert.SerializeObject(payload);
                    var stringContent = new StringContent(data, Encoding.UTF8, "application/json");
                    httpResponse = await client.PostAsync(route, stringContent);
                }
                else if (type == OperationType.PUT)
                {
                    var data = JsonConvert.SerializeObject(payload);
                    var stringContent = new StringContent(data, Encoding.UTF8, "application/json");
                    httpResponse = await client.PutAsync(route, stringContent);
                }
                else if (type == OperationType.DELETE)
                {
                    httpResponse = await client.DeleteAsync(route);
                }
                var result = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Result = JsonConvert.DeserializeObject<T>(result, new IsoDateTimeConverter());
                }
                else
                {
                    response.Error = JsonConvert.DeserializeObject<Result>(result, new IsoDateTimeConverter());
                    response.Success = false;
                }
                return response;
            }
            catch (Exception ex)
            {
                //response.Message = "Error occured!!";
                response.Success = false;
            }
            return response;
        }
    }

    public enum OperationType
    {
        GET,
        POST,
        PUT,
        DELETE
    }
    public class Response<T> where T : class
    {
        public T Result { get; set; }
        public bool Success { get; set; } = true;

        public Result Error { get; set; }

    }

    public class Result
    {
        [JsonProperty("err")]
        public string ErrorMsg { get; set; }
        [JsonProperty("ECODE")]
        public string ErrorCode { get; set; }
        [JsonProperty("error")]
        public string TimeDoctorError { get; set; }
    }
}
