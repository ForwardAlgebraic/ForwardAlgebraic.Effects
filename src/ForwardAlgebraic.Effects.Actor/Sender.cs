using ForwardAlgebraic.Effects.Abstractions;
using ForwardAlgebraic.Effects.Actor.Abstractions;
using LanguageExt;

namespace ForwardAlgebraic.Effects.Actor;

public static class Sender<RT> where RT : struct, HasEffectSender<RT>
{
    public static Eff<RT, Unit> SendEff(string address, string id, object msg) =>
        from sender in default(RT).Eff
        select sender.Send(address, id, msg);

    public static Aff<RT, T> RequestAff<T>(string address, string id, object msg) =>
        from sender in default(RT).Eff
        from ct in cancelToken<RT>()
        from _1 in Aff<RT, T>(rt => sender.RequestAsync<T>(address, id, msg, ct))
        select _1;
}
