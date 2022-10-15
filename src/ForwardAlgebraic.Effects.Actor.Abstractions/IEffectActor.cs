using ForwardAlgebraic.Effects.Abstractions;
using Proto;

namespace ForwardAlgebraic.Effects.Actor.Abstractions;

public interface IEffectActor : IMixinDisposable
{
    IContext Context { get; }

    Unit Respond(object msg)
    {
        Context.Respond(msg);
        return unit;
    }

    object? ReceiveMessage => Context.Message;

    Unit SetTimeout(TimeSpan timeout)
    {
        Context.SetReceiveTimeout(timeout);
        return unit;
    }

    Unit PoisonSelf()
    {
        Context.Poison(Context.Self);
        return unit;
    }
}
