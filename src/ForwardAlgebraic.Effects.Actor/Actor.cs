using Algebraic.Effect.Abstractions;
using LanguageExt.Effects.Traits;
using LanguageExt.Pipes;
using Proto;

namespace Algebraic.Effect.Actor;

public interface Actor<RT> : Sender<RT>, Has<RT, IContext>
    where RT : struct, HasCancel<RT>, Has<RT, IContext>, Has<RT, ISenderContext>
{
    public static Eff<RT, Unit> RespondEff(object msg) =>
        from actor in Has<RT, IContext>.Eff
        from _ in Eff(fun(() => actor.Respond(msg)))
        select unit;

    public static Aff<RT, Unit> HandlerAff<T>(Func<T, Aff<RT, Unit>> handleAff) =>
        from actor in Has<RT, IContext>.Eff
        let m = actor.Message
        from _ in m switch
        {
            T a => handleAff(a),
            _ => unitEff
        }
        select unit;

    public static Aff<RT, Unit> SetTimeoutAff(TimeSpan timeout) =>
        from actor in Has<RT, IContext>.Eff
        from _1 in HandlerAff<Started>(m =>
            Eff(fun(() => actor.SetReceiveTimeout(timeout)))
        )
        from _2 in HandlerAff<ReceiveTimeout>(m =>
            Eff(fun(() => actor.Poison(actor.Self)))
        )
        select unit;
}
