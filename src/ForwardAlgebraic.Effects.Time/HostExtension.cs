using ForwardAlgebraic.Effects.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ForwardAlgebraic.Effects.Time;

public static class HostExtension
{
    public static IHostBuilder UseEffectTime(this IHostBuilder host) =>
        host.UseEffect<IEffectTime, EffectTime>();

    public static IHostBuilder UseEffectTime(this IHostBuilder host, Func<IEffectTime> factory) =>
        host.UseEffect(factory);
}
