using ForwardAlgebraic.Effects.Abstractions;
using ForwardAlgebraic.Effects.Actor.Abstractions;
using LanguageExt;
using Proto;

namespace ForwardAlgebraic.Effects.Actor;

public static class Sender<RT> where RT : struct, HasEffectSender<RT>
{
    public static Eff<RT, Unit> SendEff(PID pid, object msg) =>
        from sender in default(RT).SenderEff
        select sender.SendToUnit(pid, msg);

    public static Aff<RT, T> RequestAff<T>(PID pid, object msg) =>
        from sender in default(RT).SenderEff
        from ct in cancelToken<RT>()
        from _1 in Aff<RT, T>(rt => sender.RequestAsync<T>(pid, msg, ct).ToValue())
        select _1;
}
