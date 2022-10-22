using ForwardAlgebraic.Effects.Abstractions;
using LanguageExt;
using LanguageExt.Effects.Traits;

namespace ForwardAlgebraic.Effects.Time;

public static class Time<RT> where RT : struct, HasCancel<RT>, Has<DateTime>
{
    public static Eff<RT, DateTime> NowEff =>
        from time in Has<RT, DateTime>.Eff
        select time;
}
