 
using System.Text.Json;
using System.Text.Json.Serialization;
using Algebraic.Effect.Abstractions;
using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Algebraic.Effect.Http.Tests;

internal record DataContext(string Hello);

internal record HeadersContext
{
    [JsonExtensionData]
    public Dictionary<string, JsonElement> Extensions { get; init; } = new();
}

internal record PostmanResponse
(
    DataContext Data,
    HeadersContext Headers
);

public class PostSpec
{
    [Fact]
    public async Task Test1()
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://postman-echo.com/")
        };

        var q = from http in Has<RT, HttpClient>.Eff
                from _1 in http.EffAddHeader("a", "b")
                from _2 in http.EffAddHeader("a", "c")
                from _3 in http.AffPost<PostmanResponse>("post", new
                {
                    Hello = "World"
                })
                select _3;

        using var cts = new CancellationTokenSource();
        var r = (await q.Run(new(httpClient, cts))).ThrowIfFail();

        Assert.Equal("World", r.Data.Hello);
        Assert.Equal("b, c", r.Headers.Extensions["a"].ToString());
    }


    private record TestResponse(string Hello);

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


        var host = Host.CreateDefaultBuilder()
                       .ConfigureServices((ctx, services) =>
                            services.AddHttpClient("stub", c => c.BaseAddress = new Uri(server.Url)))
                       .Build();

        await host.StartAsync();

        var q = from http in Has<RT, HttpClient>.Eff
                from _1 in http.EffAddHeader("a", "b")
                from _2 in http.AffPost<TestResponse>("post", new
                {
                    Hello = "World"
                })
                select _2;
        {
            using var cts = new CancellationTokenSource();
            using var http = host.Services.GetRequiredService<IHttpClientFactory>().CreateClient("stub");
            using var flurl = new FlurlClient(http);

            

            var r = await q.Run(new(http, cts));

            Assert.True(r.ThrowIfFail() is { Hello: "World" });
        }

        {
            using var cts = new CancellationTokenSource();
            using var http = host.Services.GetRequiredService<IHttpClientFactory>().CreateClient("stub");
            using var flurl = new FlurlClient(http);
            var r = await q.Run(new(http, cts));

            Assert.True(r.ThrowIfFail() is { Hello: "World" });
        }

    }

    public readonly record struct RT(HttpClient It,
                                     CancellationTokenSource CancellationTokenSource) :
        HasEffectCancel<RT>,
        Has<RT, HttpClient>;
}
