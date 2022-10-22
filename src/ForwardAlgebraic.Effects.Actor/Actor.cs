using ForwardAlgebraic.Effects.Abstractions;
using LanguageExt.Effects.Traits;
using LanguageExt.Pipes;
using Proto;

namespace ForwardAlgebraic.Effects.Actor;

public static class Actor<RT> where RT : struct, HasCancel<RT>, Has<IContext>
{
    public static Eff<RT, Unit> RespondEff(object msg) =>
        from actor in Has<RT, IContext>.Eff
        select fun(() => actor.Respond(msg))();

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
            Eff<RT, Unit>(rt => fun(() => actor.SetReceiveTimeout(timeout))())
        )
        from _2 in HandlerAff<ReceiveTimeout>(m =>
            Eff<RT, Unit>(rt => fun(() => actor.Poison(actor.Self))())
        )
        select unit;
}
