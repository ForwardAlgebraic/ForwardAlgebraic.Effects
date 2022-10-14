global using LanguageExt;
global using static LanguageExt.Prelude;
using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;

namespace ForwardAlgebraic.Effects.Abstractions;

[Typeclass("*")]
public interface HasEffectTime<RT> : HasCancel<RT>
    where RT : struct, HasEffectTime<RT>
{
    IEffectTime EffectTime { get; }

    Eff<RT, IEffectTime> Eff => Eff<RT, IEffectTime>(static rt => rt.EffectTime);
}
