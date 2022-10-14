using ForwardAlgebraic.Effects.Abstractions;

namespace ForwardAlgebraic.Effects.Time;

public readonly record struct EffectTimeForTest(DateTime FakeNow) : IEffectTime
{
    public DateTime Now => FakeNow;
    public DateTime UtcNow => FakeNow.ToUniversalTime();
}
