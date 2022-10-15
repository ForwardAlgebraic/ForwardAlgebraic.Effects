using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;

namespace ForwardAlgebraic.Effects.Actor.Abstractions;

[Typeclass("*")]
public interface HasEffectSender<RT> : HasCancel<RT>
    where RT : struct, HasEffectSender<RT>
{
    protected IEffectSender EffectSender { get; }

    Eff<RT, IEffectSender> Eff => Eff<RT, IEffectSender>(static rt => rt.EffectSender);
}
