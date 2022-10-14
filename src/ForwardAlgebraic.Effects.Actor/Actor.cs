using ForwardAlgebraic.Effects.Abstractions;
using ForwardAlgebraic.Effects.Actor.Abstractions;
using LanguageExt;

namespace ForwardAlgebraic.Effects.Actor;

public static class Actor<RT> where RT : struct, HasEffectActor<RT>
{
    public static Eff<RT, Unit> SendEff(string address, string id, object msg) =>
        from actor in default(RT).Eff
        select actor.Send(address, id, msg);

    public static Aff<RT, T> RequestAff<T>(string address, string id, object msg) =>
        from actor in default(RT).Eff
        from ct in cancelToken<RT>()
        from _1 in Aff<RT, T>(rt => actor.RequestAsync<T>(address, id, msg, ct))
        select _1;

}
