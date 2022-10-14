using ForwardAlgebraic.Effects;
using ForwardAlgebraic.Effects.Abstractions;
using LanguageExt;
using LanguageExt.UnitsOfMeasure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ForwardAlgebraic.Effects.Time.Tests;

public class TimeSpec
{
    [Fact]
    public void CaseNow()
    {
        var now = DateTime.Now;
        var q = Time<RT>.Now;

        using var time = new EffectTimeForTest(now);
        using var cts = new CancellationTokenSource();

        var r = q.Run(new(time, cts));

        Assert.Equal(now, r.ThrowIfFail());
    }

    [Fact]
    public async Task CaseGenericHostNow()
    {
        var now = DateTime.Now;

        var host = Host.CreateDefaultBuilder()
                       .UseEffectTime()
                       .UseEffectTime(() => new EffectTimeForTest(now))
                       .Build();

        await host.StartAsync();

        var q = Time<RT>.Now;

        using var cts = new CancellationTokenSource();
        using var time = host.Services.GetEffectFactory<IEffectTime>().Create();

        var r = q.Run(new(time, cts));

        Assert.Equal(now, r.ThrowIfFail());

        await host.StopAsync();
    }


    public readonly record struct RT(in IEffectTime EffectTime,
                                     CancellationTokenSource CancellationTokenSource) :
        HasEffectTime<RT>
    {

        public RT LocalCancel => this;
        public CancellationToken CancellationToken => CancellationTokenSource.Token;
    }
}
