using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebStore.Clients.Base
{
    public abstract class BaseClient : IDisposable
    {
        protected readonly string _ServiceAddress;
        protected readonly HttpClient _Client;

        protected BaseClient(IConfiguration Configuration, string ServiceAddress)
        {
            _ServiceAddress = ServiceAddress;
            _Client = new HttpClient
            {
                BaseAddress = new Uri(Configuration["WebApiURL"]),
                DefaultRequestHeaders =
                {
                    Accept =
                    {
                        new MediaTypeWithQualityHeaderValue("application/json")
                    }
                }
            };

        }

        protected T Get<T>(string url) /*where T : new()*/ => GetAsync<T>(url).Result;

        protected async Task<T> GetAsync<T>(string url, CancellationToken Cancel = default)// where T : new()
        {
            var response = await _Client.GetAsync(url, Cancel);
            return await response.EnsureSuccessStatusCode().Content.ReadAsAsync<T>(Cancel);
            //if (response.IsSuccessStatusCode)
            //    return await response.Content.ReadAsAsync<T>(Cancel);
            //return new T();
        }

        protected HttpResponseMessage Post<T>(string url, T item) => PostAsync(url, item).Result;

        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item, CancellationToken Cancel = default)
        {
            var response = await _Client.PostAsJsonAsync(url, item, Cancel);
            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Put<T>(string url, T item) => PutAsync(url, item).Result;

        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item, CancellationToken Cancel = default)
        {
            var response = await _Client.PutAsJsonAsync(url, item, Cancel);
            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;

        protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken Cancel = default)
        {
            return await _Client.DeleteAsync(url, Cancel);
        }

        //~BaseClient() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _Disposed;
        protected virtual void Dispose(bool Disposing)
        {
            if(_Disposed || !Disposing) return;

            _Disposed = true;
            _Client.Dispose();
        }
    }
}
