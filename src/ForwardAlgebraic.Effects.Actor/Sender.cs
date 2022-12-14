using Algebraic.Effect.Abstractions;
using LanguageExt.Effects.Traits;
using Proto;

namespace Algebraic.Effect.Actor;

public interface ISender<RT> : IHas<RT, ISenderContext>
    where RT : struct, HasCancel<RT>, IHas<RT, ISenderContext>
{
    public static Eff<RT, Unit> SendEff(PID pid, object msg) =>
        from sender in IHas<RT, ISenderContext>.Eff
        from x in Eff(fun(() => sender.Send(pid, msg)))
        select unit;

    public static Aff<RT, T> RequestAff<T>(PID pid, object msg) =>
        from sender in IHas<RT, ISenderContext>.Eff
        from ct in cancelToken<RT>()
        from _1 in Aff(() => sender.RequestAsync<T>(pid, msg, ct).ToValue())
        select _1;
}
