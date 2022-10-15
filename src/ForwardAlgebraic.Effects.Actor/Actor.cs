using ForwardAlgebraic.Effects.Actor.Abstractions;
using LanguageExt;
using LanguageExt.Pipes;
using Proto;
using static LanguageExt.Pipes.Proxy;

namespace ForwardAlgebraic.Effects.Actor;

public static class Actor<RT> where RT : struct, HasEffectActor<RT>
{
    public static Eff<RT, Unit> RespondEff(object msg) =>
        from actor in default(RT).Eff
        select actor.Respond(msg);

    public static Aff<RT, Unit> HandlerAff<T>(Func<T, Aff<RT, Unit>> handleAff) =>
        from actor in default(RT).Eff
        let m = actor.ReceiveMessage
        from _ in m switch
        {
            T a => handleAff(a),
            _ => unitEff
        }
        select unit;

    public static Aff<RT, Unit> SetTimeoutAff(TimeSpan timeout) =>
        from actor in default(RT).Eff
        from _1 in HandlerAff<Started>(m =>
            Eff<RT, Unit>(rt => actor.SetTimeout(timeout))
        )
        from _2 in HandlerAff<ReceiveTimeout>(m =>
            Eff<RT, Unit>(rt => actor.PoisonSelf())
        )
        select unit;
}
