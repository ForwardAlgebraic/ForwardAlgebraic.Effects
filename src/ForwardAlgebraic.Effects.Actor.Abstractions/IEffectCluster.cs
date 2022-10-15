using ForwardAlgebraic.Effects.Abstractions;
using Proto;
using Proto.Cluster;

namespace ForwardAlgebraic.Effects.Actor.Abstractions;

public interface IEffectCluster : IMixinDisposable
{
    protected ISenderContext Context { get; }

    protected Cluster Cluster => Context.System.Cluster();

    async ValueTask<T> RequestAsync<T>(ClusterIdentity cid, object message, CancellationToken ct) =>
        await Cluster.RequestAsync<T>(cid, message, ct);


}
