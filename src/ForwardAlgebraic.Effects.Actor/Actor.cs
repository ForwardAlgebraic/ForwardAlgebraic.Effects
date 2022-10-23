using Algebraic.Effect.Abstractions;
using LanguageExt.Effects.Traits;
using LanguageExt.Pipes;
using Proto;
using Proto.Cluster;

namespace Algebraic.Effect.Actor;

public interface IActor<RT> : IHas<RT, IContext>, ISender<RT>, ICluster<RT>
    where RT : struct, HasCancel<RT>, IHas<RT, IContext>, IHas<RT, ISenderContext>, IHas<RT, Cluster>
{
    public static Eff<RT, Unit> RespondEff(object msg) =>
        from actor in IHas<RT, IContext>.Eff
        from _ in Eff(fun(() => actor.Respond(msg)))
        select unit;

    public static Aff<RT, Unit> HandlerAff<T>(Func<T, Aff<RT, Unit>> handleAff) =>
        from actor in IHas<RT, IContext>.Eff
        let m = actor.Message
        from _ in m switch
        {
            T a => handleAff(a),
            _ => unitEff
        }
        select unit;

    public static Eff<RT, Unit> HandlerEff<T>(Func<T, Eff<RT, Unit>> handleEff) =>
        from actor in IHas<RT, IContext>.Eff
        let m = actor.Message
        from _ in m switch
        {
            T a => handleEff(a),
            _ => unitEff
        }
        select unit;

    public static Eff<RT, Unit> SetPoisonSelfEff(TimeSpan timeout) =>
        from actor in IHas<RT, IContext>.Eff
        from _1 in HandlerEff<Started>(m =>
            Eff(fun(() => actor.SetReceiveTimeout(timeout)))
        )
        from _2 in HandlerEff<ReceiveTimeout>(m =>
            Eff(fun(() => actor.Poison(actor.Self)))
        )
        select unit;
}
