using System.Text.Json;
using System.Text.Json.Nodes;

namespace Void.SimpleHttp.Test;

public class SimpleHttpRequestBuilderTests
{
    [Fact]
    public void Constructor_ShouldInitializeRequestMethodAndUri()
    {
        var uri = new Uri("https://example.com");
        var builder = new SimpleHttpRequestBuilder(
            HttpMethod.Post,
            uri
        );

        var request = builder.Build();
        
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal(uri, request.RequestUri);
    }

    [Fact]
    public void Constructor_ShouldInitializeRequestMethodAndStringUri()
    {
        const string uri = "https://example.com";
        var builder = new SimpleHttpRequestBuilder(
            HttpMethod.Post,
            uri
        );

        var request = builder.Build();
        
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal(new Uri(uri), request.RequestUri);
    }

    [Fact]
    public void WithMethod_ShouldSetRequestMethod()
    {
        var builder = new SimpleHttpRequestBuilder().WithMethod(HttpMethod.Post);

        var request = builder.Build();
        
        Assert.Equal(HttpMethod.Post, request.Method);
    }
    
    [Fact]
    public void WithUri_ShouldSetRequestUri()
    {
        var uri = new Uri("https://example.com");
        
        var builder = new SimpleHttpRequestBuilder().WithUri(uri);

        var request = builder.Build();
        
        Assert.Equal(uri, request.RequestUri);
    }
    
    [Fact]
    public void WithUri_ShouldSetRequestStringUri()
    {
        const string uri = "https://example.com";
        
        var builder = new SimpleHttpRequestBuilder().WithUri(uri);

        var request = builder.Build();
        
        Assert.Equal(new Uri(uri), request.RequestUri);
    }

    [Fact]
    public void WithBearerToken_ShouldBeInitializedWithDefaultPrefix()
    {
        const string token = "example";
        const string bearerToken = $"Bearer {token}";

        var builder = new SimpleHttpRequestBuilder().WithBearerToken(token);

        var request = builder.Build();
        
        Assert.Equal(bearerToken, request.Headers.Authorization.ToString());
    }

    [Fact]
    public void WithBearerToken_ShouldBeInitializedWithCustomPrefix()
    {
        const string token = "example";
        const string prefix = "Custom";
        const string bearerToken = $"{prefix} {token}";

        var builder = new SimpleHttpRequestBuilder().WithBearerToken(token, prefix);

        var request = builder.Build();
        
        Assert.Equal(bearerToken, request.Headers.Authorization.ToString());
    }

    [Fact]
    public void WithHeader_ShouldSetHeaderWithSingleValue()
    {
        const string header = "x-custom-header";
        const string value = "example";

        var builder = new SimpleHttpRequestBuilder().WithHeader(header, value);

        var request = builder.Build();
        
        Assert.NotEmpty(request.Headers.GetValues(header));
        Assert.Equal(value, request.Headers.GetValues(header).FirstOrDefault());
    }
    
    [Fact]
    public void WithHeader_ShouldSetHeaderWithMultipleValues()
    {
        const string header = "x-custom-header";
        var values = new List<string>() { "value1", "value2", "value3" };

        var builder = new SimpleHttpRequestBuilder().WithHeader(header, values);

        var request = builder.Build();

        foreach (var value in values)
        {
            Assert.Contains(request.Headers.GetValues(header), x => value.Equals(x));
        }
    }

    [Fact]
    public void WithHeaders_ShouldSetHeadersWithSingleValue()
    {
        var headers = new Dictionary<string, string>()
        {
            { "header1", "value1" },
            { "header2", "value2" },
            { "header3", "value3" }
        };

        var builder = new SimpleHttpRequestBuilder().WithHeaders(headers);

        var request = builder.Build();

        foreach (var (header, value) in headers)
        {
            Assert.Equal(value, request.Headers.GetValues(header).FirstOrDefault());
        }
    }
    
    [Fact]
    public void WithHeaders_ShouldSetHeadersWithMultipleValues()
    {
        var headers = new Dictionary<string, IEnumerable<string>>()
        {
            { "header1", new List<string>() {"header1-value1","header1-value2","header1-value3"}},
            { "header2", new List<string>() {"header2-value1","header2-value2","header2-value3"}},
            { "header3", new List<string>() {"header3-value1","header3-value2","header3-value3"} }
        };

        var builder = new SimpleHttpRequestBuilder().WithHeaders(headers);

        var request = builder.Build();

        foreach (var (header, values) in headers)
        {
            foreach (var value in values)
            {
                Assert.Contains(request.Headers.GetValues(header), x => value.Equals(x));
            }
        }
    }

    [Fact]
    public async Task WithJsonBody_ShouldSetContentWithSerializedJsonObject()
    {
        var obj = new { FirstName = "John", LastName = "Smith" };

        var builder = new SimpleHttpRequestBuilder().WithJsonBody(obj, null);

        var request = builder.Build();
        
        Assert.Equal(JsonSerializer.Serialize(obj), await request.Content.ReadAsStringAsync());
    }
    
    [Fact]
    public async Task WithJsonBody_ShouldSetContentWithSerializedJsonNode()
    {
        JsonNode node = new JsonObject()
        {
            { "FirstName", "John" },
            { "LastName", "Smith"}
        };

        var builder = new SimpleHttpRequestBuilder().WithJsonBody(node, null);

        var request = builder.Build();
        
        Assert.Equal(node.ToJsonString(), await request.Content.ReadAsStringAsync());
    }
}