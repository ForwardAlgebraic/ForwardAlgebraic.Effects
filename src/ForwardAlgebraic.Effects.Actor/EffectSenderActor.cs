using Proto;

namespace ForwardAlgebraic.Effects.Actor;

public readonly record struct EffectSenderActor(ISenderContext Context) : IMixinEffectSenderActor
{
}

