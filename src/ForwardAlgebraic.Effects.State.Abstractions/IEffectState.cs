using ForwardAlgebraic.Effects.Abstractions;
using Proto;

namespace ForwardAlgebraic.Effects.State.Abstractions;

public interface IEffectState<TState> : IMixinDisposable
{
    protected TState State { get; }
}
