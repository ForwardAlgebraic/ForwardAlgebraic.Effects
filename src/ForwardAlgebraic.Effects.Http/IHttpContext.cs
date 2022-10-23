using System.Net.Http.Json;
using System.Text.Json;
using Algebraic.Effect.Abstractions;
using LanguageExt;
using LanguageExt.Pipes;
using Microsoft.AspNetCore.Http;
using static LanguageExt.Prelude;

namespace Algebraic.Effect.Http;

public interface IHttpContext<RT> : IHas<RT, HttpContext>
    where RT : struct, IHas<RT, HttpContext>
{
    public static JsonSerializerOptions JsonSerializerOptions { get; } = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static Aff<RT, R> ReadRequestAff<R>() =>
        from ct in cancelToken<RT>()
        from context in Eff
        from _1 in Aff(() => context.Request.ReadFromJsonAsync<R>(JsonSerializerOptions, ct))
        select _1;

    public static Aff<RT, Unit> WriteResponseAff(IResult result) =>
        from context in Eff
        from _1 in Aff(() => result.ExecuteAsync(context).ToUnit().ToValue())
        select unit;
}
