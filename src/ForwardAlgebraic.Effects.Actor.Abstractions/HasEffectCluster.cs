using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;

namespace ForwardAlgebraic.Effects.Actor.Abstractions;

[Typeclass("*")]
public interface HasEffectCluster<RT> : HasCancel<RT>
    where RT : struct, HasEffectCluster<RT>
{
    protected IEffectCluster EffectCluster { get; }

    Eff<RT, IEffectCluster> Eff => Eff<RT, IEffectCluster>(static rt => rt.EffectCluster);
}
