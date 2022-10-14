using ForwardAlgebraic.Effects.Abstractions;
using Proto;

namespace ForwardAlgebraic.Effects.Actor.Tests;

public class ActorSpec
{
    [Fact]
    public async Task RequestAff()
    {
        var system = new ActorSystem();

        var props = Props.FromFunc(ctx => ctx.Message switch
        {
            string m => Task.Run(() =>
            {
                ctx.Respond("success" == m);
            }),
            _ => Task.CompletedTask
        });

        var pid = system.Root.Spawn(props);

        var q = Actor<RT>.RequestAff<bool>(pid.Address, pid.Id, "success");
        using var cts = new CancellationTokenSource();
        using var actor = new EffectActor(system.Root);

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

        var q = Actor<RT>.SendEff(pid.Address, pid.Id, "success");
        using var cts = new CancellationTokenSource();
        using var actor = new EffectActor(system.Root);

        var r = q.Run(new(actor, cts));

        r.ThrowIfFail();

        ev.Wait(5000);

        Assert.Equal("success", ret);

        await system.DisposeAsync();
    }

    public readonly record struct RT(in IEffectActor EffectActor,
                                     CancellationTokenSource CancellationTokenSource) :
       HasEffectActor<RT>
    {
        public RT LocalCancel => new(EffectActor, CancellationTokenSource.CreateLinkedTokenSource(CancellationToken));
        public CancellationToken CancellationToken => CancellationTokenSource.Token;
    }
}
