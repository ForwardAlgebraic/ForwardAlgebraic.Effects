using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;

namespace ForwardAlgebraic.Effects.Abstractions;

[Typeclass("*")]
public interface HasEffectActor<RT> : HasCancel<RT>
    where RT : struct, HasEffectActor<RT>
{
    IEffectActor EffectActor { get; }

    Eff<RT, IEffectActor> Eff => Eff<RT, IEffectActor>(static rt => rt.EffectActor);
}
