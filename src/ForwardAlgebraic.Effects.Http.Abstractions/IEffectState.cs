using ForwardAlgebraic.Effects.Abstractions;

namespace ForwardAlgebraic.Effects.Http.Abstractions;

public readonly record struct EffectHttp(HttpClient HttpClient) : IMixinDisposable
{

}
