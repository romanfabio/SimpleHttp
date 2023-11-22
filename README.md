# SimpleHttp

A small package that makes it easy to call external REST APIs by providing a simplified abstraction of HttpRequestMessage and HttpResponseMessage classes.


## Quick start

```cs
// Create a new request using the builder
HttpRequestMessage request = 
    new SimpleHttpRequestBuilder(HttpMethod.Post, "https://example.com")
      .WithBearerToken("abcedef")
      .WithJsonBody(new { Id = 2 })
      .Build();

HttpClient client = new HttpClient();

// Use SimpleHttpResponse to manage the client's response
SimpleHttpResponse response = new(await client.SendAsync(request));

Console.WriteLine(response.GetHeaderValue("Content-Type"));
Console.WriteLine(await response.DeserializeJsonContentAsync<JsonNode>());
```

## Documentation

This package provides two classes: 
- SimpleHttpRequestBuilder
- SimpleHttpResponse

The SimpleHttpRequestBuilder makes it easy to create a new HTTP request using the Builder pattern.

```cs
HttpRequestMessage request = 
    new SimpleHttpRequestBuilder()
      .WithMethod(HttpMethod.Post)
      .WithUri("https://example.com")
      .WithBearerToken("secret-token")
      .WithJsonBody(new { Id = 2 })
      .WithHeader("x-api-key", "value")
      .WithHeaders(new Dictionary<string,string>() { {"header1","value1"}, {"header2","value2"} })
      .Build();
```

The SimpleHttpResponse uses a simplified interface to interact with a HTTP response.

```cs
// HttpResponseMessage raw = response received from a HTTP client

SimpleHttpResponse response = new SimpleHttpResponse(raw);

response.EnsureSuccessStatusCode();

Console.WriteLine(response.StatusCode);
Console.WriteLine(response.IsSuccessStatusCode);
Console.WriteLine(response.ReasonPhrase);

await response.DeserializeJsonContentAsync<JsonNode>();

response.GetHeaderValue("x-api-key");
response.GetHeaderValues("Accept-Encoding");

if(response.TryGetHeaderValues("Accept-Language", out List<string> values) {
  ///
}
```
