using ForwardAlgebraic.Effects.Abstractions;

namespace ForwardAlgebraic.Effects.Time;

public readonly struct EffectTime : IEffectTime
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}
