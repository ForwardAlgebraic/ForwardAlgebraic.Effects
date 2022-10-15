using ForwardAlgebraic.Effects.Abstractions;
using LanguageExt;
using Proto;

namespace ForwardAlgebraic.Effects.Actor.Abstractions;

public interface IEffectSender : IMixinDisposable
{
    protected ISenderContext Context { get; }

    Unit Send(string address, string id, object msg)
    {
        Context.Send(PID.FromAddress(address, id), msg);
        return unit;
    }

    async ValueTask<T> RequestAsync<T>(string address, string id, object msg, CancellationToken ct) =>
        await Context.RequestAsync<T>(PID.FromAddress(address, id), msg, ct);
}
