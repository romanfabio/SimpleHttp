using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Void.SimpleHttp
{
    public class SimpleHttpRequestBuilder
    {
        private readonly HttpRequestMessage _request;

        public SimpleHttpRequestBuilder()
        {
            _request = new HttpRequestMessage();
        }

        public SimpleHttpRequestBuilder(HttpMethod method, string requestUri)
        {
            _request = new HttpRequestMessage(method, requestUri);
        }
        
        public SimpleHttpRequestBuilder(HttpMethod method, Uri requestUri)
        {
            _request = new HttpRequestMessage(method, requestUri);
        }

        public SimpleHttpRequestBuilder WithBearerToken(string token, string prefix = "Bearer")
        {
            _request.Headers.Authorization = new AuthenticationHeaderValue(prefix, token);
            return this;
        }

        public SimpleHttpRequestBuilder WithJsonBody<T>(T value, JsonSerializerOptions? options = null)
        {
            _request.Content = new StringContent(JsonSerializer.Serialize(value, options), Encoding.UTF8, "application/json");
            return this;
        }

        public SimpleHttpRequestBuilder WithHeader(string name, string value)
        {
            _request.Headers.Add(name, value);
            return this;
        }

        public SimpleHttpRequestBuilder WithHeader(string name, IEnumerable<string> values)
        {
            _request.Headers.Add(name, values);
            return this;
        }

        public SimpleHttpRequestBuilder WithHeaders(IDictionary<string, string> headers)
        {
            foreach (var (name, value) in headers)
            {
                _request.Headers.Add(name, value);
            }

            return this;
        }

        public SimpleHttpRequestBuilder WithHeaders(IDictionary<string, IEnumerable<string>> headers)
        {
            foreach (var (name, value) in headers)
            {
                _request.Headers.Add(name, value);
            }

            return this;
        }

        public HttpRequestMessage Build()
        {
            return _request;
        }

        public static SimpleHttpRequestBuilder CreateRequest()
        {
            return new SimpleHttpRequestBuilder();
        }
    }
}