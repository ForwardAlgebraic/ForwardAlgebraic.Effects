using Algebraic.Effect.Abstractions;
using LanguageExt;
using LanguageExt.Effects.Traits;

namespace Algebraic.Effect.Time;


public interface Time<RT> where RT : struct, HasCancel<RT>, Has<RT, DateTime>
{
    public static Eff<RT, DateTime> NowEff => Has<RT, DateTime>.Eff;
}
