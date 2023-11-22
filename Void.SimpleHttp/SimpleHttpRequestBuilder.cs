using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Void.SimpleHttp
{
    /// <summary>Represents a simple builder to facilitate the creation of <see cref="HttpRequestMessage"/> instances.</summary>
    public class SimpleHttpRequestBuilder
    {
        private readonly HttpRequestMessage _request;

        /// <summary>Initializes a new instance of the <see cref="SimpleHttpRequestBuilder"/> class.</summary>
        public SimpleHttpRequestBuilder()
        {
            _request = new HttpRequestMessage();
        }

        /// <summary>Initializes a new instance of the <see cref="SimpleHttpRequestBuilder" /> class with an HTTP method and a request <see cref="T:System.Uri" />.</summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="requestUri">A string that represents the request  <see cref="T:System.Uri" />.</param>
        public SimpleHttpRequestBuilder(HttpMethod method, string requestUri)
        {
            _request = new HttpRequestMessage(method, requestUri);
        }
        
        /// <summary>Initializes a new instance of the <see cref="SimpleHttpRequestBuilder"/> class with an HTTP method and a request <see cref="T:System.Uri" />.</summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="requestUri">The <see cref="T:System.Uri" /> to request.</param>
        public SimpleHttpRequestBuilder(HttpMethod method, Uri requestUri)
        {
            _request = new HttpRequestMessage(method, requestUri);
        }
        
        /// <summary>Sets the HTTP method used by the HTTP request message.</summary>
        /// <param name="method">The HTTP method.</param>
        public SimpleHttpRequestBuilder WithMethod(HttpMethod method)
        {
            _request.Method = method;
            return this;
        }

        /// <summary>Sets the request <see cref="T:System.Uri"/> used by the HTTP request message.</summary>
        /// <param name="requestUri">The <see cref="T:System.Uri"/> to request.</param>
        public SimpleHttpRequestBuilder WithUri(Uri requestUri)
        {
            _request.RequestUri = requestUri;
            return this;
        }

        /// <summary>Sets the request <see cref="T:System.Uri"/> used by the HTTP request message.</summary>
        /// <param name="requestUri">A string that represents the request <see cref="T:System.Uri"/>.</param>
        public SimpleHttpRequestBuilder WithUri(string requestUri)
        {
            _request.RequestUri = new Uri(requestUri);
            return this;
        }

        /// <summary>Sets the Bearer token used by the HTTP request message.</summary>
        /// <param name="token">The token.</param>
        /// <param name="prefix">The prefix used for the token. The default is "Bearer".</param>
        public SimpleHttpRequestBuilder WithBearerToken(string token, string prefix = "Bearer")
        {
            _request.Headers.Authorization = new AuthenticationHeaderValue(prefix, token);
            return this;
        }

        /// <summary>
        /// Serialize the value and uses it as JSON body for the HTTP request message.
        /// The body is configured with UTF-8 encoding and "application/json" media type.
        /// </summary>
        /// <param name="value">The value to be serialized.</param>
        /// <param name="options">Options to control the serialization behavior.</param>
        /// <typeparam name="T">The type of the value to serialize.</typeparam>
        public SimpleHttpRequestBuilder WithJsonBody<T>(T value, JsonSerializerOptions? options = null)
        {
            _request.Content = new StringContent(JsonSerializer.Serialize(value, options), Encoding.UTF8, "application/json");
            return this;
        }

        /// <summary>Adds a single-value header to the HTTP request message.</summary>
        /// <param name="name">The header to add to the HTTP request message.</param>
        /// <param name="value">The content of the header.</param>
        public SimpleHttpRequestBuilder WithHeader(string name, string value)
        {
            _request.Headers.Add(name, value);
            return this;
        }

        /// <summary>Adds a multi-value header to the HTTP request message.</summary>
        /// <param name="name">The header to add to the HTTP request message.</param>
        /// <param name="values">The values of the header.</param>
        public SimpleHttpRequestBuilder WithHeader(string name, IEnumerable<string> values)
        {
            _request.Headers.Add(name, values);
            return this;
        }

        /// <summary>Adds multiple single-value headers to the HTTP request message.</summary>
        /// <param name="headers">The headers to add to the HTTP request message.</param>
        public SimpleHttpRequestBuilder WithHeaders(IDictionary<string, string> headers)
        {
            foreach (var (name, value) in headers)
            {
                _request.Headers.Add(name, value);
            }

            return this;
        }

        /// <summary>Adds multiple multi-value headers to the HTTP request message.</summary>
        /// <param name="headers">The headers to add to the HTTP request message.</param>
        public SimpleHttpRequestBuilder WithHeaders(IDictionary<string, IEnumerable<string>> headers)
        {
            foreach (var (name, value) in headers)
            {
                _request.Headers.Add(name, value);
            }

            return this;
        }

        /// <summary>Returns the built instance of <see cref="HttpRequestMessage"/>.</summary>
        /// <returns>The built instance of <see cref="HttpRequestMessage"/>.</returns>
        public HttpRequestMessage Build()
        {
            return _request;
        }
        
    }
}