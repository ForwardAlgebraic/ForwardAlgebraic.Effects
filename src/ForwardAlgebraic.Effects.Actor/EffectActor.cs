using ForwardAlgebraic.Effects.Abstractions;
using Proto;

namespace ForwardAlgebraic.Effects.Actor;

public readonly record struct EffectActor(ISenderContext Context) : IEffectActor
{
    public Unit Send(string address, string id, object msg)
    {
        Context.Send(PID.FromAddress(address, id), msg);
        return unit;
    }

    public async ValueTask<T> RequestAsync<T>(string address, string id, object msg, CancellationToken ct) =>
        await Context.RequestAsync<T>(PID.FromAddress(address, id), msg, ct);
}
