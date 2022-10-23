using Algebraic.Effect.Abstractions;
using LanguageExt.Effects.Traits;
using LanguageExt.Pipes;
using Proto;
using Proto.Cluster;

namespace Algebraic.Effect.Actor;

public interface ICluster<RT> : Has<RT, Cluster>
    where RT : struct, HasCancel<RT>, Has<RT, Cluster>
{
    public static Aff<RT, T> RequestAff<T>(string identity, string kind, object msg) =>
        RequestAff<T>(ClusterIdentity.Create(identity, kind), msg);

    public static Aff<RT, T> RequestAff<T>(ClusterIdentity cid, object msg) =>
        from cluster in Has<RT, Cluster>.Eff
        from ct in cancelToken<RT>()
        from _1 in Aff(() => cluster.RequestAsync<T>(cid, msg, ct).ToValue())
        select _1;
}
