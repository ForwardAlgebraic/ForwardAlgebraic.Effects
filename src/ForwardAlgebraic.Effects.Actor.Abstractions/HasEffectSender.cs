using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;
using Proto;

namespace ForwardAlgebraic.Effects.Actor.Abstractions;

[Typeclass("*")]
public interface HasEffectSender<RT> : HasCancel<RT>
    where RT : struct, HasEffectSender<RT>
{
    protected ISenderContext SenderContext { get; }

    Eff<RT, ISenderContext> SenderEff => Eff<RT, ISenderContext>(static rt => rt.SenderContext);
}
