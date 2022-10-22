using System.Net.Http.Json;
using System.Text.Json;
using Flurl.Http;
using ForwardAlgebraic.Effects.Abstractions;
using LanguageExt;
using LanguageExt.Effects.Traits;
using LanguageExt.Pipes;
using static LanguageExt.Prelude;

namespace ForwardAlgebraic.Effects.Http;

public static class Http<RT> where RT : struct, HasCancel<RT>, Has<HttpClient>
{
    public static JsonSerializerOptions JsonSerializerOptions { get; } =  new()
    {
        PropertyNameCaseInsensitive = true
    };

public static Aff<RT, R> PostAff<R>(string requestUri, object jsonBody) =>
        from http in Has<RT, HttpClient>.Eff
        from _1 in Aff(() => http.PostAsync(requestUri, JsonContent.Create(jsonBody)).ToValue())
        from _2 in Aff(() => _1.EnsureSuccessStatusCode()
                               .Content
                               .ReadFromJsonAsync<R>(JsonSerializerOptions)
                               .ToValue())
        select _2;
}
