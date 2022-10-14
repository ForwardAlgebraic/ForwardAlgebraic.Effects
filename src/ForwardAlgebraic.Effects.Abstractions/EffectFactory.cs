using Microsoft.Extensions.DependencyInjection;

namespace ForwardAlgebraic.Effects.Abstractions;

public class EffectFactory<TA> : IEffectFactory<TA>
{
    public EffectFactory(Func<TA> factory)
    {
        Factory = factory;
    }

    public Func<TA> Factory { get; }

    public TA Create(params object[] args) => Factory.Invoke();
}

public class EffectFactory<TA, TC> : IEffectFactory<TA>
    where TC : TA
{
    public EffectFactory(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public IServiceProvider ServiceProvider { get; }

    public TA Create(params object[] args) =>
        ActivatorUtilities.CreateInstance<TC>(ServiceProvider, args);

}
