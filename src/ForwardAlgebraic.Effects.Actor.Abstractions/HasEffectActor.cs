using ForwardAlgebraic.Effects.Abstractions;
using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;

namespace ForwardAlgebraic.Effects.Actor.Abstractions;

[Typeclass("*")]
public interface HasEffectActor<RT> : HasEffectCancel<RT>
    where RT : struct, HasEffectActor<RT>
{
    protected IEffectActor EffectActor { get; }

    Eff<RT, IEffectActor> Eff => Eff<RT, IEffectActor>(static rt => rt.EffectActor);
}
