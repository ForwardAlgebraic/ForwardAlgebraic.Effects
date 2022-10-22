using LanguageExt;
using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;
using static LanguageExt.Prelude;

namespace ForwardAlgebraic.Effects.Abstractions;

[Typeclass("*")]
public interface Has<T> 
{
    T It { get; }
}
public static class Has<RT, T> where RT : struct, Has<T>
{
    public static Eff<RT, T> Eff => EffMaybe<RT, T>(static rt => rt.It);
}
