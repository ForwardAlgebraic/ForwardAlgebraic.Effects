using Algebraic.Effect.Abstractions;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Hosting;

namespace Algebraic.Effect.Time.Tests;

public class TimeSpec
{
    [Theory, InlineAutoData]
    public void CaseNow(DateTime now, CancellationTokenSource cts)
    {
        var q = Time<RT>.NowEff;

        var r = q.Run(new(now, cts));

        Assert.Equal(now, r.ThrowIfFail());
    }

    [Theory, InlineAutoData]
    public async Task CaseGenericHostNow(DateTime now, CancellationTokenSource cts)
    {
        var host = Host.CreateDefaultBuilder()
                       .Build();

        await host.StartAsync();

        var q = Time<RT>.NowEff;

        var r = q.Run(new(now, cts));

        Assert.Equal(now, r.ThrowIfFail());

        await host.StopAsync();

    }


    public readonly record struct RT(DateTime It, CancellationTokenSource CancellationTokenSource) :
        HasEffectCancel<RT>,
        Has<RT, DateTime>
    {
    }
}
