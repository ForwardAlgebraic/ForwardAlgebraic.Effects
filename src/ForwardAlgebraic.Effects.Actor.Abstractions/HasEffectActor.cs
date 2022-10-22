using ForwardAlgebraic.Effects.Abstractions;
using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;
using Proto;

namespace ForwardAlgebraic.Effects.Actor.Abstractions;

[Typeclass("*")]
public interface HasEffectActor<RT> : HasEffectCancel<RT>
    where RT : struct, HasEffectActor<RT>
{
    protected IContext Context { get; }

    Eff<RT, IContext> ActorEff => Eff<RT, IContext>(static rt => rt.Context);
}

public static class Has<RT, T> where RT : struct, Has<T>
{
    public static Eff<RT, T> Eff => EffMaybe<RT, T>(static rt => rt.It);
}
