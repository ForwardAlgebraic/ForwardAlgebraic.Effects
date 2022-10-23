using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;

namespace Algebraic.Effect.Abstractions;

[Typeclass("*")]
public interface IHasEffectCancel<RT> : HasCancel<RT>
    where RT : struct, IHasEffectCancel<RT>
{
    RT HasCancel<RT>.LocalCancel => default;
    CancellationToken HasCancel<RT>.CancellationToken => CancellationTokenSource.Token;
}
