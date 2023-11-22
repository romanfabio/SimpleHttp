using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Void.SimpleHttp
{
    /// <summary>Represents a simplified and thin wrapper around <see cref="HttpResponseMessage"/> instances.</summary>
    public class SimpleHttpResponse
    {
        private readonly HttpResponseMessage _response;

        /// <summary>Initializes a new instance of the <see cref="SimpleHttpResponse"/> class.</summary>
        /// <param name="response">The <see cref="HttpResponseMessage"/> instance to manage.</param>
        public SimpleHttpResponse(HttpResponseMessage response)
        {
            _response = response;
        }
        
        /// <summary>Gets the status code of the HTTP response.</summary>
        public HttpStatusCode StatusCode => _response.StatusCode;

        /// <summary>Gets a value that indicates if the HTTP response was successful.</summary>
        public bool IsSuccessStatusCode => _response.IsSuccessStatusCode;

        /// <summary>Gets the reason phrase which typically is sent by servers together with the status code.</summary>
        public string ReasonPhrase => _response.ReasonPhrase;

        /// <summary>Returns the underlying <see cref="HttpResponseMessage"/> instance.</summary>
        public HttpResponseMessage GetRawResponse()
        {
            return _response;
        }

        /// <summary>Throws an exception if the <see cref="SimpleHttpResponse.IsSuccessStatusCode" /> property for the HTTP response is <see langword="false" />.</summary>
        /// <exception cref="T:System.Net.Http.HttpRequestException">The HTTP response is unsuccessful.</exception>
        public SimpleHttpResponse EnsureSuccessStatusCode()
        {
            _response.EnsureSuccessStatusCode();
            return this;
        }

        /// <summary>Reads the HTTP JSON content and return the deserialized object.</summary>
        /// <param name="options">Options to control the deserialization behavior.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <typeparam name="TValue">The type to deserialize the JSON content into.</typeparam>
        /// <returns>A TValue representation of the JSON content.</returns>
        public async Task<TValue> DeserializeJsonContentAsync<TValue>(JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        {
            return await JsonSerializer.DeserializeAsync<TValue>(
                await _response.Content.ReadAsStreamAsync(),
                options,
                cancellationToken
            );
        }

        /// <summary>Returns the first header value for a specified header stored in the <see cref="T:System.Net.Http.Headers.HttpHeaders" /> collection.</summary>
        /// <param name="name">The specified header to return value for.</param>
        /// <exception cref="T:System.InvalidOperationException">The header cannot be found.</exception>
        /// <returns>The first header value or <see langword="null"/> if the header has no value</returns>
        public string GetHeaderValue(string name)
        {
            return GetHeaderValues(name).FirstOrDefault();
        }

        /// <summary>Returns all header values for a specified header stored in the <see cref="T:System.Net.Http.Headers.HttpHeaders" /> collection.</summary>
        /// <param name="name">The specified header to return values for.</param>
        /// <exception cref="T:System.InvalidOperationException">The header cannot be found.</exception>
        /// <returns>An array of header strings.</returns>
        public IEnumerable<string> GetHeaderValues(string name)
        {
            return _response.Headers.GetValues(name);
        }

        /// <summary>Return if a specified header and specified values are stored in the <see cref="T:System.Net.Http.Headers.HttpHeaders" /> collection.</summary>
        /// <param name="name">The specified header.</param>
        /// <param name="values">The specified header values.</param>
        /// <returns>
        /// <see langword="true" /> if the specified header <paramref name="name" /> and <see langword="values" /> are stored in the collection; otherwise <see langword="false" />.</returns>
        public bool TryGetHeaderValues(string name, out IEnumerable<string> values)
        {
            return _response.Headers.TryGetValues(name, out values);
        }
    }
}