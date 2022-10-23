using Algebraic.Effect.Abstractions;
using LanguageExt.Effects.Traits;
using LanguageExt.Pipes;
using Proto;

namespace Algebraic.Effect.Actor;

public static class ActorExtensions
{
    public static Eff<Unit> EffRespond(this IContext actor, object msg) =>
        Eff(fun(() => actor.Respond(msg)));

    public static Aff<Unit> AffHandler<RT, T>(this IContext actor, Func<T, Aff<RT, Unit>> handleAff) 
        where RT : struct, HasCancel<RT> =>
            from __ in unitEff
            let msg = actor.Message
            from _1 in msg switch
            {
                T a => handleAff(a),
                _ => unitEff
            }
            select unit;

    public static Aff<R> AffHandler<T, R>(this IContext actor, Func<T, Aff<R>> handleAff) =>
        from __ in unitEff
        let msg = actor.Message
        from _1 in msg switch
        {
            T a => handleAff(a),
            _ => Eff(() => default(R)!)
        }
        select _1;

    public static Aff<Unit> AffSetTimeout(this IContext actor, TimeSpan timeout) =>
        from _1 in actor.AffHandler<Started>(m =>
            Eff(fun(() => actor.SetReceiveTimeout(timeout)))
        )
        from _2 in actor.AffHandler<ReceiveTimeout>(m =>
            Eff(fun(() => actor.Poison(actor.Self)))
        )
        select unit;
}
