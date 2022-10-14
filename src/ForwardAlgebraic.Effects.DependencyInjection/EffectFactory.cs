using ForwardAlgebraic.Effects.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ForwardAlgebraic.Effects.DependencyInjection;

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
