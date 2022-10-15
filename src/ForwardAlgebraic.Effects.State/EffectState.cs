using ForwardAlgebraic.Effects.State.Abstractions;

namespace ForwardAlgebraic.Effects.State;

public readonly record struct EffectState<TState>(TState State) : IEffectState<TState>
{
}

