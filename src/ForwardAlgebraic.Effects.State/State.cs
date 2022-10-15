using ForwardAlgebraic.Effects.State.Abstractions;

namespace ForwardAlgebraic.Effects.State;

public static class State<RT, TState>
    where RT : struct, HasEffectState<RT, TState>
{

}
