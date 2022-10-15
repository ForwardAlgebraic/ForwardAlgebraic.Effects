using ForwardAlgebraic.Effects.Abstractions;
using LanguageExt;

namespace ForwardAlgebraic.Effects.Actor.Abstractions;

public interface IEffectSender : IMixinDisposable
{
    Unit Send(string address, string id, object msg);
    ValueTask<T> RequestAsync<T>(string address, string id, object msg, CancellationToken ct);
}
