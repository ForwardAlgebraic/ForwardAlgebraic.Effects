using ForwardAlgebraic.Effects.Abstractions;
using Proto;

namespace ForwardAlgebraic.Effects.Actor.Tests;

public class ActorSpec
{
    [Fact]
    public async Task TimeoutEff()
    {
        await using var system = new ActorSystem();
        var ev = new ManualResetEventSlim();

        var props = Props.FromFunc(async ctx =>
        {
            using var cts = new CancellationTokenSource();

            var r = await Business().Run(new(ctx, cts));
            _ = r.ThrowIfFail();

            static Aff<RT2, Unit> Business() =>
                from __ in unitAff
                from _3 in Actor<RT2>.SetTimeoutAff(3 * sec)
                select unit;
        });

        var pid = system.Root.Spawn(props);

        _ = system.Root.Spawn(Props.FromFunc(ctx => ctx.Message switch
        {
            Started => Task.Run(() => ctx.Watch(pid)),
            Terminated => Task.Run(() => ev.Set()),
            _ => Task.CompletedTask
        }));

        Assert.True(ev.Wait(10000));
    }

    [Fact]
    public async Task RequestAff()
    {
        await using var system = new ActorSystem();

        var props = Props.FromFunc(async ctx =>
        {
            using var cts = new CancellationTokenSource();

            var r = await Business().Run(new(ctx, cts));
            _ = r.ThrowIfFail();

            static Aff<RT2, Unit> Business() =>
                from __ in unitAff
                from _1 in Sender<RT2>.SendEff(PID.FromAddress(ActorSystem.NoHost, "1"), "1")
                from _2 in Actor<RT2>.HandlerAff<string>(static m =>
                    Actor<RT2>.RespondEff(m == "success")
                )
                from _3 in Actor<RT2>.HandlerAff<int>(static m =>
                    Actor<RT2>.RespondEff(m == 1)
                )
                select unit;
        });

        var pid = system.Root.Spawn(props);

        var q =
            from _1 in Sender<RT1>.RequestAff<bool>(pid, "success")
            from _2 in Sender<RT1>.RequestAff<bool>(pid, 1)
            select _1 && _2;

        using var cts = new CancellationTokenSource();
        var r = await q.Run(new(system.Root, cts));

        Assert.True(r.ThrowIfFail());
    }

    [Fact]
    public async Task SendAff()
    {
        var ev = new ManualResetEventSlim();
        await using var system = new ActorSystem();
        var ret = "fail";

        var props = Props.FromFunc(ctx => ctx.Message switch
        {
            string m => Task.Run(() =>
            {
                ret = m;
                ev.Set();
            }),
            _ => Task.CompletedTask
        });

        var pid = system.Root.Spawn(props);

        var q = Sender<RT1>.SendEff(pid, "success");
        using var cts = new CancellationTokenSource();

        var r = q.Run(new(system.Root, cts));

        _ = r.ThrowIfFail();

        _ = ev.Wait(5000);

        Assert.Equal("success", ret);
    }

    public readonly record struct RT1(in ISenderContext It,
                                      CancellationTokenSource CancellationTokenSource)
        : HasEffectCancel<RT1>, Has<ISenderContext>;

    public readonly record struct RT2(in IContext It,
                                      CancellationTokenSource CancellationTokenSource)
        : HasEffectCancel<RT2>, Has<IContext>, Has<ISenderContext>
    {
        ISenderContext Has<ISenderContext>.It => It;
    }
}
