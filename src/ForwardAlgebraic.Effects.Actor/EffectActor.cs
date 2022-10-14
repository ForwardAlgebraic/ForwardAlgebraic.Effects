using ForwardAlgebraic.Effects.Abstractions;
using Proto;

namespace ForwardAlgebraic.Effects.Actor;

public readonly record struct EffectActor(IContext Context) : IMixinEffectSenderActor
{
    ISenderContext IMixinEffectSenderActor.Context => Context;

    public Unit Respond(object msg)
    {
        Context.Respond(msg);
        return unit;
    }
}

