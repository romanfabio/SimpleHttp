using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Void.SimpleHttp.Test;

public class SimpleHttpResponseTests
{
    [Fact]
    public void StatusCode_ShouldBeOK()
    {
        var response = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK
        };

        var simpleResponse = new SimpleHttpResponse(response);
        
        Assert.Equal(HttpStatusCode.OK, simpleResponse.StatusCode);
    }

    [Fact]
    public void ReasonPhrase_ShouldBeOk()
    {
        var response = new HttpResponseMessage()
        {
            ReasonPhrase = "Ok"
        };

        var simpleResponse = new SimpleHttpResponse(response);
        
        Assert.Equal("Ok", simpleResponse.ReasonPhrase);
    }

    [Fact]
    public void IsSuccessStatusCode_ShouldBeTrue()
    {
        var response = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK
        };

        var simpleResponse = new SimpleHttpResponse(response);
        
        Assert.True(simpleResponse.IsSuccessStatusCode);
    }
    
    [Fact]
    public void EnsureSuccessStatusCode_ShouldThrowAnException()
    {
        var response = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.NotFound
        };

        var simpleResponse = new SimpleHttpResponse(response);

        Assert.Throws<HttpRequestException>(() => simpleResponse.EnsureSuccessStatusCode());
    }

    [Fact]
    public void GetHeaderValue_ShouldReturnSingleValue()
    {
        const string header = "x-test";
        const string value = "example";

        var response = new HttpResponseMessage()
        {
            Headers = { { header, value } }
        };

        var simpleResponse = new SimpleHttpResponse(response);
        
        Assert.Equal(value, simpleResponse.GetHeaderValue(header));
    }
    
    [Fact]
    public void GetHeaderValues_ShouldReturnMultipleValues()
    {
        const string header = "x-test";
        IEnumerable<string> values = new List<string>{"value1","value2","value3"};

        var response = new HttpResponseMessage()
        {
            Headers = { { header, values } }
        };

        var simpleResponse = new SimpleHttpResponse(response);

        foreach (var value in values)
        {
            Assert.Contains(simpleResponse.GetHeaderValues(header), x => value.Equals(x));
        }
    }

    [Fact]
    public void TryGetHeaderValues_ShouldReturnFalse()
    {
        var response = new HttpResponseMessage();

        var simpleResponse = new SimpleHttpResponse(response);
        
        Assert.False(simpleResponse.TryGetHeaderValues("notpresent", out _));
    }

    [Fact]
    public async Task DeserializeJsonContentAsync_ShouldReturnCorrectDeserializedObject()
    {
        var obj = new JsonObject()
        {
            { "FirstName", "John" },
            { "LastName", "Smith" }
        };
        
        var response = new HttpResponseMessage()
        {
            Content = new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json")
        };

        var simpleResponse = new SimpleHttpResponse(response);

        var deserializedObject = await simpleResponse.DeserializeJsonContentAsync<JsonObject>();
        
        Assert.Equal(obj["FirstName"].ToString(), deserializedObject?["FirstName"].ToString());
    }
}