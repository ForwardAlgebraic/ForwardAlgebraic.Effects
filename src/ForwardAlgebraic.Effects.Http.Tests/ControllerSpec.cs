using System.Text;
using Algebraic.Effect.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Algebraic.Effect.Http.Tests;


public class SutController : ControllerBase
{
    public async Task A()
    {
        await Results.Ok(new
        {
            World = "Hello"
        }).ExecuteAsync(HttpContext);
    }
}

public class ControllerSpec
{
    [Fact]
    public async Task Test1()
    {
        var host = Host.CreateDefaultBuilder().Build();
        await host.StartAsync();

        var json = System.Text.Json.JsonSerializer.Serialize(new
        {
            Hello = "World"
        });
        using var reqStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        using var resStream = new MemoryStream();
        var httpContext = new DefaultHttpContext()
        {
            RequestServices = host.Services,
            Request = {
                Method = "POST",
                Body = reqStream,
                ContentLength = reqStream.Length
            },
            Response =
            {
                Body = resStream
            }
        };

        var sut = new SutController()
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            }
        };

        await sut.A();

        await host.StopAsync();

    }

    public readonly record struct RT(HttpContext It,
                                     CancellationTokenSource CancellationTokenSource) :
        IHasEffectCancel<RT>,
        IHas<RT, HttpContext>;
}
