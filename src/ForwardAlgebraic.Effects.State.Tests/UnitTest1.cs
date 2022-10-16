using ForwardAlgebraic.Effects.Abstractions;
using ForwardAlgebraic.Effects.State.Abstractions;
using LanguageExt;

namespace ForwardAlgebraic.Effects.State.Tests;

public class StateSpec
{
    [Fact]
    public void CaseValueType()
    {
      
    }

    public readonly record struct RT(IEffectState<int> EffectState,
        CancellationTokenSource CancellationTokenSource) :
        HasEffectCancel<RT>,
        HasEffectState<RT, int>
    {
    }
}
