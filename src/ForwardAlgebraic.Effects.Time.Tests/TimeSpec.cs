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

        using var time = new EffectTimeForTest(now);
        using var cts = new CancellationTokenSource();

        var r = q.Run(new(time, cts));

        Assert.Equal(now, r.ThrowIfFail());
    }

    [Theory, InlineAutoData]
    public async Task CaseGenericHostNow(DateTime now, CancellationTokenSource cts)
    {
        var host = Host.CreateDefaultBuilder()
                       .UseEffectTime()
                       .UseEffectTime(() => new EffectTimeForTest(now))
                       .Build();

        await host.StartAsync();

        var q = Time<RT>.NowEff;

        using var time = host.Services.GetEffectFactory<IEffectTime>().Create();

        var r = q.Run(new(time, cts));

        Assert.Equal(now, r.ThrowIfFail());

        await host.StopAsync();
    }


    public readonly record struct RT(in IEffectTime EffectTime,
                                     CancellationTokenSource CancellationTokenSource) :
        HasEffectTime<RT>
    {
        public RT LocalCancel => new(EffectTime, CancellationTokenSource.CreateLinkedTokenSource(CancellationToken));
        public CancellationToken CancellationToken => CancellationTokenSource.Token;
    }
}
