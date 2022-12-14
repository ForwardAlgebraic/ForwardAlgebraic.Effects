using ForwardAlgebraic.Effects.Abstractions;
using LanguageExt;

namespace ForwardAlgebraic.Effects;

public static class Time<RT>
    where RT : struct, HasEffectTime<RT>
{
    public static Eff<RT, DateTime> NowEff =>
        from time in default(RT).Eff
        select time.Now;

}
