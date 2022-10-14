using LanguageExt;

namespace ForwardAlgebraic.Effects.Actor.Abstractions;

public interface IEffectSenderActor : IDisposable
{
    void IDisposable.Dispose() => GC.SuppressFinalize(this);
    Unit Send(string address, string id, object msg);
    ValueTask<T> RequestAsync<T>(string address, string id, object msg, CancellationToken ct);
}
