using Algebraic.Effect.Abstractions;
using LanguageExt.Pipes;
using Proto;
using Proto.Cluster;
using actor = Algebraic.Effect.Actor.IActor<Algebraic.Effect.Actor.Tests.ActorSpec.RT2>;
using sender = Algebraic.Effect.Actor.ISender<Algebraic.Effect.Actor.Tests.ActorSpec.RT1>;


namespace Algebraic.Effect.Actor.Tests;

public class ActorSpec
{
    [Fact]
    public async Task TimeoutEff()
    {
        await using var system = new ActorSystem();
        var ev = new ManualResetEventSlim();

        using var cts = new CancellationTokenSource();

        var pid = system.Root.Spawn(Props.FromFunc(async ctx =>
        {
            var q = actor.SetPoisonSelfEff(3 * sec);
            var r = q.Run(new(ctx, cts));
            _ = r.ThrowIfFail();
            await Task.CompletedTask;
        }));

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
                from _1 in actor.SendEff(PID.FromAddress(ActorSystem.NoHost, "1"), "1")
                from _2 in actor.HandlerAff<string>(static m =>
                    actor.RespondEff(m == "success")
                )
                from _3 in actor.HandlerAff<int>(static m =>
                    actor.RespondEff(m == 1)
                )
                select unit;
        });

        var pid = system.Root.Spawn(props);

        var q =
            from _1 in sender.RequestAff<bool>(pid, "success")
            from _2 in sender.RequestAff<bool>(pid, 1)
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

        var q = sender.SendEff(pid, "success");
        using var cts = new CancellationTokenSource();

        var r = q.Run(new(system.Root, cts));

        _ = r.ThrowIfFail();

        _ = ev.Wait(5000);

        Assert.Equal("success", ret);
    }

    public readonly record struct RT1(in ISenderContext It,
                                      CancellationTokenSource CancellationTokenSource) :
        IHasEffectCancel<RT1>,
        IHas<RT1, ISenderContext>;

    public readonly record struct RT2(in IContext It,
                                      CancellationTokenSource CancellationTokenSource) :
        IHasEffectCancel<RT2>,
        IHas<RT2, IContext>,
        IHas<RT2, ISenderContext>,
        IHas<RT2, Cluster>
    {
        ISenderContext IHas<RT2, ISenderContext>.It => It;

        Cluster IHas<RT2, Cluster>.It => It.Cluster();
    }
}
