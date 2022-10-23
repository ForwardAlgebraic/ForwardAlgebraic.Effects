using LanguageExt;
using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;
using static LanguageExt.Prelude;

namespace Algebraic.Effect.Abstractions;

[Typeclass("*")]
public interface Has<RT, T> : HasCancel<RT> where RT : struct, Has<RT, T>
{
    protected T It { get; }

    static Eff<RT, T> Eff => EffMaybe<RT, T>(static rt => rt.It);
}
