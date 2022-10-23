using LanguageExt.Attributes;
using LanguageExt.Effects.Traits;

namespace Algebraic.Effect.Abstractions;

[Typeclass("*")]
public interface HasEffectCancel<RT> : HasCancel<RT>
    where RT : struct, HasEffectCancel<RT>
{
    RT HasCancel<RT>.LocalCancel => default;
    CancellationToken HasCancel<RT>.CancellationToken => CancellationTokenSource.Token;
}
