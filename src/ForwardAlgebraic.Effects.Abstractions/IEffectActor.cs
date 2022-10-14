namespace ForwardAlgebraic.Effects.Abstractions;

public interface IEffectActor : IDisposable
{
    void IDisposable.Dispose() => GC.SuppressFinalize(this);
    Unit Send(string address, string id, object msg);
    ValueTask<T> RequestAsync<T>(string address, string id, object msg, CancellationToken ct);
}
