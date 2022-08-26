using Newtonsoft.Json;
using SQ1Blog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SQ1Blog.Services
{
    public interface IHostedServiceRepository
    {
        Task<ResponseBase<List<ApiArticleResponse>>> ImportPosts();
    }

    public class HostedServiceRepository : IHostedServiceRepository
    {
        public HostedServiceRepository()
        {

        }

        public async Task<ResponseBase<List<ApiArticleResponse>>> ImportPosts()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    ///TODO this should go to the appsettings
                    client.BaseAddress = new Uri("https://sq1-api-test.herokuapp.com");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //GET Method
                    HttpResponseMessage response = await client.GetAsync("/posts");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<ServiceResponse>(responseString);

                        return new ResponseBase<List<ApiArticleResponse>>()
                        {
                            Data = res.Data,
                            StatusCode = response.StatusCode,
                            Message = response.StatusCode.ToString()
                        };
                    }
                    else
                    {
                        Console.WriteLine("Internal server Error");
                        return new ResponseBase<List<ApiArticleResponse>>()
                        {
                            Data = null,
                            StatusCode = response.StatusCode,
                            Message = response.StatusCode.ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseBase<List<ApiArticleResponse>>()
                {
                    Data = null,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Message = ex.Message
                };
            }
        }

    }
}
