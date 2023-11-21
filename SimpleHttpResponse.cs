using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Void.SimpleHttp
{
    public class SimpleHttpResponse
    {
        private readonly HttpResponseMessage _response;

        public SimpleHttpResponse(HttpResponseMessage response)
        {
            _response = response;
        }
        
        public HttpStatusCode StatusCode => _response.StatusCode;

        public bool IsSuccessStatusCode => _response.IsSuccessStatusCode;

        public string ReasonPhrase => _response.ReasonPhrase;

        public HttpResponseMessage GetRawResponse()
        {
            return _response;
        }

        public SimpleHttpResponse EnsureSuccessStatusCode()
        {
            _response.EnsureSuccessStatusCode();
            return this;
        }

        public async Task<TValue> DeserializeJsonContentAsync<TValue>(JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        {
            return await JsonSerializer.DeserializeAsync<TValue>(
                await _response.Content.ReadAsStreamAsync(),
                options,
                cancellationToken
            );
        }

        public string GetHeaderValue(string name)
        {
            return GetHeaderValues(name).FirstOrDefault();
        }

        public IEnumerable<string> GetHeaderValues(string name)
        {
            return _response.Headers.GetValues(name);
        }

        public bool TryGetHeaderValues(string name, out IEnumerable<string> values)
        {
            return _response.Headers.TryGetValues(name, out values);
        }
    }
}