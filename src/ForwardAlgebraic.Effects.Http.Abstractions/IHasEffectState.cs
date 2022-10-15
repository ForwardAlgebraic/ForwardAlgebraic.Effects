using System.Net.Http;
using ForwardAlgebraic.Effects.Abstractions;
using ForwardAlgebraic.Effects.Http.Abstractions;
using LanguageExt.Attributes;

namespace ForwardAlgebraic.Effects.State.Abstractions;

[Typeclass("*")]
public interface HasEffectHttp<RT> : HasEffectCancel<RT>
    where RT : struct, HasEffectHttp<RT>
{

    protected EffectHttp EffectHttp { get; }


    Eff<RT, EffectHttp> Eff =>
        Eff<RT, EffectHttp>(static rt => rt.EffectHttp);
}
