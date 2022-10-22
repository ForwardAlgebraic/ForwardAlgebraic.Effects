using ForwardAlgebraic.Effects.Abstractions;
using LanguageExt.Effects.Traits;
using LanguageExt.Pipes;
using Proto;
using Proto.Cluster;

namespace ForwardAlgebraic.Effects.Actor;

public static class Cluster<RT> where RT : struct, HasCancel<RT>, Has<Cluster>
{
    public static Aff<RT, T> RequestAff<T>(string identity, string kind, object msg) =>
        RequestAff<T>(ClusterIdentity.Create(identity, kind), msg);

    public static Aff<RT, T> RequestAff<T>(ClusterIdentity cid, object msg) =>
        from cluster in Has<RT, Cluster>.Eff
        from ct in cancelToken<RT>()
        from _1 in Aff(async () => await cluster.RequestAsync<T>(cid, msg, ct))
        select _1;
}
