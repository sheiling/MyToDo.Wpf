using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MyToDo.Shared;
using RestSharp;
using Newtonsoft.Json;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;

namespace MyToDo.Service
{
    public class HttpRestClient
    {
        private readonly string _apiUrl;
        private RestClient _restSharp;

        public HttpRestClient(string apiUrl)
        {
            _apiUrl = apiUrl;
            _restSharp = new RestClient();
        }

        public async Task<ApiResponse> ExecuteAsync(BaseRequest baseRequest)
        {
            var request = new RestRequest(baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);

            if (baseRequest.Parameter != null)
                request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter),
                    ParameterType.RequestBody);

            _restSharp.BaseUrl = new Uri(_apiUrl + baseRequest.Route);
            var response = await _restSharp.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
        }

        public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
        {
            var request = new RestRequest(baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);

            if (baseRequest.Parameter != null)
                request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter),
                    ParameterType.RequestBody);

            _restSharp.BaseUrl = new Uri(_apiUrl + baseRequest.Route);
            var response = await _restSharp.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
        }
    }
}
