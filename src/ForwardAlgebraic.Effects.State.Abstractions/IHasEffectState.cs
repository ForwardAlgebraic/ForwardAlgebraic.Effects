using ForwardAlgebraic.Effects.Abstractions;
using LanguageExt.Attributes;

namespace ForwardAlgebraic.Effects.State.Abstractions;

[Typeclass("*")]
public interface HasEffectState<RT, TState> : HasEffectCancel<RT>
    where RT : struct, HasEffectState<RT, TState>
{
    protected IEffectState<TState> EffectState { get; }

    Eff<RT, IEffectState<TState>> Eff =>
        Eff<RT, IEffectState<TState>>(static rt => rt.EffectState);
}
