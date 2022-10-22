using Flurl.Http;
using ForwardAlgebraic.Effects.Abstractions;
using LanguageExt.Effects.Traits;

namespace ForwardAlgebraic.Effects.Http;

public static class Http<RT> where RT : struct, HasCancel<RT>, Has<IFlurlClient>
{

}
