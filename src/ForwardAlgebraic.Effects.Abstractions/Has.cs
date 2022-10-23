using LanguageExt;
using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;
using static LanguageExt.Prelude;

namespace Algebraic.Effect.Abstractions;

[Typeclass("*")]
public interface Has<RT, T> where RT : struct, Has<RT, T>
{
    T It { get; }

    static Eff<RT, T> Eff => EffMaybe<RT, T>(static rt => rt.It);
}
