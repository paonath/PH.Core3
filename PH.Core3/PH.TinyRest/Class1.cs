using System;
using System.Net.Http;
using System.Threading.Tasks;
using PH.Core3.Common.Result;

namespace PH.TinyRest
{
    public interface IGet
    {
        Task<T> GetAsync<T>(string url);
    }

    public interface ICoreGet
    {
        Task<IResult<T>> GetResultAsync<T>(string url);
    }

    public class Client
    {
        private HttpClient _httpClient;

        public Client()
        {
            _httpClient = new HttpClient();
        }

        public void P()
        {
            _httpClient.
        }
    }
}
