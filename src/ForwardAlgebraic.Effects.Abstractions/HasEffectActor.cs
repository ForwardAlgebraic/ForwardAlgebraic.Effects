using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;

namespace ForwardAlgebraic.Effects.Abstractions;

[Typeclass("*")]
public interface HasEffectActor<RT> : HasCancel<RT>
    where RT : struct, HasEffectActor<RT>
{
    IEffectSenderActor EffectActor { get; }

    Eff<RT, IEffectSenderActor> Eff => Eff<RT, IEffectSenderActor>(static rt => rt.EffectActor);
}
