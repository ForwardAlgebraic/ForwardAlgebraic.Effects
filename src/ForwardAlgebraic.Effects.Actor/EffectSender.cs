using ForwardAlgebraic.Effects.Actor.Abstractions;
using Proto;

namespace ForwardAlgebraic.Effects.Actor;

public readonly record struct EffectSender(ISenderContext Context) : IEffectSender 
{
}
