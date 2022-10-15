using ForwardAlgebraic.Effects.Abstractions;
using ForwardAlgebraic.Effects.State.Abstractions;
using LanguageExt;

namespace ForwardAlgebraic.Effects.State.Tests;

public class StateSpec
{
    [Fact]
    public void CaseValueType()
    {
        RWSState<>
        int a = 1;


        static Aff<RT, int> business() =>
            from __ in unitEff
            from _1 in State<RT, int>.Set()
            select State<RT, int>.Get();
    }

    public readonly record struct RT(IEffectState<int> EffectState,
        CancellationTokenSource CancellationTokenSource) :
        HasEffectCancel<RT>,
        HasEffectState<RT, int>
    {
    }
}
