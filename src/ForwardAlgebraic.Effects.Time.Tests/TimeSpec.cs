using AutoFixture.Xunit2;
using ForwardAlgebraic.Effects.Abstractions;
using LanguageExt;
using LanguageExt.UnitsOfMeasure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ForwardAlgebraic.Effects.Time.Tests;

public class TimeSpec
{
    [Theory, InlineAutoData]
    public void CaseNow(DateTime now)
    {
        var q = Time<RT>.NowEff;

        using var cts = new CancellationTokenSource();

        var r = q.Run(new(now));

        Assert.Equal(now, r.ThrowIfFail());
    }

    [Theory, InlineAutoData]
    public async Task CaseGenericHostNow(DateTime now)
    {
        var host = Host.CreateDefaultBuilder()
                       .Build();

        await host.StartAsync();

        var q = Time<RT>.NowEff;

        var r = q.Run(new(now));

        Assert.Equal(now, r.ThrowIfFail());

        await host.StopAsync();
    }


    public readonly record struct RT(DateTime Now) :
        HasEffectTime<RT>
    {
    }
}
