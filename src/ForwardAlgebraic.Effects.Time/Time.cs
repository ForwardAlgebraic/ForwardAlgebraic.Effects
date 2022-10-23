using Algebraic.Effect.Abstractions;
using LanguageExt;
using LanguageExt.Effects.Traits;

namespace Algebraic.Effect.Time;


public interface Time<RT> where RT : struct, HasCancel<RT>, IHas<RT, DateTime>
{
    public static Eff<RT, DateTime> NowEff => IHas<RT, DateTime>.Eff;
}
