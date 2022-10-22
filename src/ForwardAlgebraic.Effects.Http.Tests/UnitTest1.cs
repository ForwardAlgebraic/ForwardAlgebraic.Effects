using System.Net.Http.Json;
using System.Text.Json;
using ForwardAlgebraic.Effects.Abstractions;
using LanguageExt;
using LanguageExt.Pipes;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Hosting;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ForwardAlgebraic.Effects.Http.Tests;



public class UnitTest1
{
    record TestResponse(string Hello);

    [Fact]
    public async Task Test1()
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://postman-echo.com/")
        };

        var q = Http<RT>.PostAff<dynamic>("post", new
        {
            Hello = "World"
        });

        using var cts = new CancellationTokenSource();
        var r = await q.Run(new RT(httpClient, cts));

        var x = r.ThrowIfFail();
    }

    [Fact]
    public async Task Test2()
    {
        using var server = WireMockServer.Start();

        server.Given(Request.Create().WithPath("/post").UsingPost())
              .RespondWith(Response.Create()
                                   .WithStatusCode(200)
                                   .WithHeader("Content-Type", "text/plain")
                                   .WithBodyAsJson(new
                                   {
                                       Hello = "World"
                                   }));

        
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(server.Url)
        };

        var q = Http<RT>.PostAff<TestResponse>("post", new
        {
            Hello = "World"
        });

        using var cts = new CancellationTokenSource();
        var r = await q.Run(new RT(httpClient, cts));

        Assert.True(r.ThrowIfFail() is { Hello: "World" }) ;
    }

    public readonly record struct RT(HttpClient It, CancellationTokenSource CancellationTokenSource) :
        HasEffectCancel<RT>,
        Has<HttpClient>
    {
    }
}
