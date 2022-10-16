using ForwardAlgebraic.Effects.Actor.Abstractions;
using LanguageExt;
using LanguageExt.Pipes;
using Proto;

namespace ForwardAlgebraic.Effects.Actor;

public static class Actor<RT> where RT : struct, HasEffectActor<RT>
{
    public static Eff<RT, Unit> RespondEff(object msg) =>
        from actor in default(RT).ActorEff
        select actor.RespondToUnit(msg);

    public static Aff<RT, Unit> HandlerAff<T>(Func<T, Aff<RT, Unit>> handleAff) =>
        from actor in default(RT).ActorEff
        let m = actor.Message
        from _ in m switch
        {
            T a => handleAff(a),
            _ => unitEff
        }
        select unit;

    public static Aff<RT, Unit> SetTimeoutAff(TimeSpan timeout) =>
        from actor in default(RT).ActorEff
        from _1 in HandlerAff<Started>(m =>
            Eff<RT, Unit>(rt => actor.SetReceiveTimeoutToUnit(timeout))
        )
        from _2 in HandlerAff<ReceiveTimeout>(m =>
            Eff<RT, Unit>(rt => actor.PoisonToUnit(actor.Self))
        )
        select unit;
}
