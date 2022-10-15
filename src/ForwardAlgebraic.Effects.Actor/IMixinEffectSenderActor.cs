using ForwardAlgebraic.Effects.Actor.Abstractions;
using Proto;

namespace ForwardAlgebraic.Effects.Actor;

public interface IMixinEffectSender: IEffectSender
{
    ISenderContext Context { get; }

    Unit IEffectSender.Send(string address, string id, object msg)
    {
        Context.Send(PID.FromAddress(address, id), msg);
        return unit;
    }

    async ValueTask<T> IEffectSender.RequestAsync<T>(string address, string id, object msg, CancellationToken ct) =>
        await Context.RequestAsync<T>(PID.FromAddress(address, id), msg, ct);
}

