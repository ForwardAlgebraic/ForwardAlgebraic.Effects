using ForwardAlgebraic.Effects.Actor.Abstractions;
using LanguageExt.Pipes;
using Proto;
using Proto.Cluster;

namespace ForwardAlgebraic.Effects.Actor;

public static class Cluster<RT> where RT : struct, HasEffectCluster<RT>
{
    public static Aff<RT, T> RequestAff<T>(string identity, string kind, object msg) =>
        RequestAff<T>(ClusterIdentity.Create(identity, kind), msg);


    public static Aff<RT, T> RequestAff<T>(ClusterIdentity cid, object msg) =>
        from cluster in default(RT).Eff
        from ct in cancelToken<RT>()
        from _1 in Aff<RT, T>(rt => cluster.RequestAsync<T>(cid, msg, ct))
        select _1;
}
