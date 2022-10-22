using AutoFixture.Xunit2;
using ForwardAlgebraic.Effects.Abstractions;
using LanguageExt;
using LanguageExt.UnitsOfMeasure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.CodeCoverage;

namespace ForwardAlgebraic.Effects.Time.Tests;

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
        Has<DateTime>
    {
    }
}
