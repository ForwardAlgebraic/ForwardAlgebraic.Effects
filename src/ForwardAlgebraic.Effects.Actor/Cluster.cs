using Algebraic.Effect.Abstractions;
using LanguageExt.Effects.Traits;
using LanguageExt.Pipes;
using Proto;
using Proto.Cluster;

namespace Algebraic.Effect.Actor;

public interface ICluster<RT> : IHas<RT, Cluster>
    where RT : struct, HasCancel<RT>, IHas<RT, Cluster>
{
    public static Aff<RT, T> RequestAff<T>(string identity, string kind, object msg) =>
        RequestAff<T>(ClusterIdentity.Create(identity, kind), msg);

    public static Aff<RT, T> RequestAff<T>(ClusterIdentity cid, object msg) =>
        from cluster in IHas<RT, Cluster>.Eff
        from ct in cancelToken<RT>()
        from _1 in Aff(() => cluster.RequestAsync<T>(cid, msg, ct).ToValue())
        select _1;
}
