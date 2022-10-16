using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;
using Proto.Cluster;

namespace ForwardAlgebraic.Effects.Actor.Abstractions;

[Typeclass("*")]
public interface HasEffectCluster<RT> : HasCancel<RT>
    where RT : struct, HasEffectCluster<RT>
{
    protected Cluster Cluster { get; }

    Eff<RT, Cluster> ClusterEff => Eff<RT, Cluster>(static rt => rt.Cluster);
}
