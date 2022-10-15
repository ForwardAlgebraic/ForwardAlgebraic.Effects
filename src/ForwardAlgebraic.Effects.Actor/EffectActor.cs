using ForwardAlgebraic.Effects.Abstractions;
using Proto;

namespace ForwardAlgebraic.Effects.Actor;

public readonly record struct EffectActor(IContext Context) : IMixinEffectActor, IMixinEffectSender
{
    ISenderContext IMixinEffectSender.Context => Context;
}

