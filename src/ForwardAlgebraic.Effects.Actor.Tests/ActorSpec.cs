using ForwardAlgebraic.Effects.Abstractions;
using ForwardAlgebraic.Effects.Actor.Abstractions;
using Proto;

namespace ForwardAlgebraic.Effects.Actor.Tests;

public class ActorSpec
{
    [Fact]
    public async Task TimeoutEff()
    {
        var system = new ActorSystem();
        var ev = new ManualResetEventSlim();

        var props = Props.FromFunc(async ctx =>
        {
            using var cts = new CancellationTokenSource();
            using var actor = new EffectActor(ctx);

            var r = await Business().Run(new(actor, actor, cts));
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

        await system.DisposeAsync();
    }

    [Fact]
    public async Task RequestAff()
    {
        var system = new ActorSystem();

        var props = Props.FromFunc(async ctx =>
        {
            using var cts = new CancellationTokenSource();
            using var actor = new EffectActor(ctx);

            var r = await Business().Run(new(actor, actor, cts));
            _ = r.ThrowIfFail();

            static Aff<RT2, Unit> Business() =>
                from __ in unitAff
                from _1 in Sender<RT2>.SendEff(ActorSystem.NoHost, "1", "1")
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
            from _1 in Sender<RT1>.RequestAff<bool>(pid.Address, pid.Id, "success")
            from _2 in Sender<RT1>.RequestAff<bool>(pid.Address, pid.Id, 1)
            select _1 && _2;

        using var cts = new CancellationTokenSource();
        using var actor = new EffectSender(system.Root);

        var r = await q.Run(new(actor, cts));

        Assert.True(r.ThrowIfFail());

        await system.DisposeAsync();
    }

    [Fact]
    public async Task SendAff()
    {
        var ev = new ManualResetEventSlim();
        var system = new ActorSystem();
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

        var q = Sender<RT1>.SendEff(pid.Address, pid.Id, "success");
        using var cts = new CancellationTokenSource();
        using var sender = new EffectSender(system.Root);

        var r = q.Run(new(sender, cts));

        _ = r.ThrowIfFail();

        _ = ev.Wait(5000);

        Assert.Equal("success", ret);

        await system.DisposeAsync();
    }

    public readonly record struct RT1(in IEffectSender EffectSender,
                                      CancellationTokenSource CancellationTokenSource)
        : HasEffectCancel<RT1>, HasEffectSender<RT1>;

    public readonly record struct RT2(in IEffectActor EffectActor,
                                      in IEffectSender EffectSender,
                                      CancellationTokenSource CancellationTokenSource)
        : HasEffectCancel<RT2>, HasEffectActor<RT2>, HasEffectSender<RT2>;
}
