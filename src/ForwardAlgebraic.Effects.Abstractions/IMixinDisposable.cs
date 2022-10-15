namespace ForwardAlgebraic.Effects.Abstractions;

public interface IMixinDisposable : IDisposable
{
    void IDisposable.Dispose() => GC.SuppressFinalize(this);
}
