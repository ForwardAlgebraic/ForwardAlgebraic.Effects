namespace ForwardAlgebraic.Effects.Abstractions;

public interface IEffectTime : IMixinDisposable
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}
