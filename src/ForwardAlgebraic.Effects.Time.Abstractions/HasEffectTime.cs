global using LanguageExt;
global using static LanguageExt.Prelude;
using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;

namespace ForwardAlgebraic.Effects.Abstractions;

[Typeclass("*")]
public interface HasEffectTime<RT> 
    where RT : struct, HasEffectTime<RT>
{
    protected DateTime Now => DateTime.Now;

    Eff<RT, DateTime> TimeEff => Eff<RT, DateTime>(static rt => rt.Now);
}
