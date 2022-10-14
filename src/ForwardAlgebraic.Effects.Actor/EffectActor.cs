using ForwardAlgebraic.Effects.Abstractions;
using Proto;

namespace ForwardAlgebraic.Effects.Actor;

public interface IEffectSenderActorImpl: IEffectSenderActor
{
    ISenderContext Context { get; }

    Unit IEffectSenderActor.Send(string address, string id, object msg)
    {
        Context.Send(PID.FromAddress(address, id), msg);
        return unit;
    }

    async ValueTask<T> IEffectSenderActor.RequestAsync<T>(string address, string id, object msg, CancellationToken ct) =>
        await Context.RequestAsync<T>(PID.FromAddress(address, id), msg, ct);
}

public readonly record struct EffectSenderActor(ISenderContext Context) : IEffectSenderActorImpl
{
}

public readonly record struct EffectActor(IContext Context) : IEffectSenderActorImpl
{
    ISenderContext IEffectSenderActorImpl.Context => Context;

    public Unit Respond(object msg)
    {
        Context.Respond(msg);
        return unit;
    }
}

