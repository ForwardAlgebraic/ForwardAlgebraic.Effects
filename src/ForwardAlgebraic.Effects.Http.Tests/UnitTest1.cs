using System.Net.Http.Json;
using System.Text.Json;
using ForwardAlgebraic.Effects.Abstractions;
using LanguageExt;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Hosting;

namespace ForwardAlgebraic.Effects.Http.Tests;



public class UnitTest1
{
    record TestResponse(JsonElement Json);
    [Fact]
    public async Task Test1()
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://postman-echo.com/")
        };

        var q = from _1 in Http<RT>.PostAff<TestResponse>("post", new
                {
                    Hello = "World"
                })
                select _1;

        using var cts = new CancellationTokenSource();
        var r = await q.Run(new RT(httpClient, cts));

        var x = r.ThrowIfFail();
    }

    public readonly record struct RT(HttpClient It, CancellationTokenSource CancellationTokenSource) :
        HasEffectCancel<RT>,
        Has<HttpClient>
    {
    }
}
