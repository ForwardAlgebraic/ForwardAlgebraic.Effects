using Proto;

namespace ForwardAlgebraic.Effects.Actor;

internal static class ExtensionsContext
{
    public static Unit SendToUnit(this ISenderContext ctx, PID target, object message)
    {
        ctx.Send(target, message);
        return Unit.Default;
    }

    public static Unit RespondToUnit(this IContext ctx, object msg)
    {
        ctx.Respond(msg);
        return Unit.Default;
    }
    public static Unit SetReceiveTimeoutToUnit(this IContext ctx, TimeSpan duration)
    {
        ctx.SetReceiveTimeout(duration);
        return Unit.Default;
    }

    public static Unit PoisonToUnit(this IContext ctx, PID pid)
    {
        ctx.Poison(pid);
        return Unit.Default;
    }
}
