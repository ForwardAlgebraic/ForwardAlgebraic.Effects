using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ForwardAlgebraic.Effects.Abstractions;

public static class HostExtension
{
    public static IHostBuilder UseEffect<T, TC>(this IHostBuilder host) where TC : T =>
        host.ConfigureServices((ctx, services) => services.AddSingleton<IEffectFactory<T>, EffectFactory<T, TC>>());

    public static IHostBuilder UseEffect<T>(this IHostBuilder host, Func<T> factory) =>
       host.ConfigureServices((ctx, services) =>
       {
           services.AddSingleton<IEffectFactory<T>>(sp => new EffectFactory<T>(factory));
       });

    public static IEffectFactory<T> GetEffectFactory<T>(this IServiceProvider sp) =>
       sp.GetRequiredService<IEffectFactory<T>>();
}
