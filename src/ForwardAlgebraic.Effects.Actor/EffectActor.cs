using ForwardAlgebraic.Effects.Abstractions;
using ForwardAlgebraic.Effects.Actor.Abstractions;
using Proto;

namespace ForwardAlgebraic.Effects.Actor;

public readonly record struct EffectActor(IContext Context) : IEffectActor, IEffectSender 
{
    ISenderContext IEffectSender .Context => Context;
}

