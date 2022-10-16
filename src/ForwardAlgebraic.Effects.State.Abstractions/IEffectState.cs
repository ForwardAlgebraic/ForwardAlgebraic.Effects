using ForwardAlgebraic.Effects.Abstractions;

namespace ForwardAlgebraic.Effects.State.Abstractions;

public interface IEffectState<TState> : IMixinDisposable
{
    protected TState State { get; }
}
