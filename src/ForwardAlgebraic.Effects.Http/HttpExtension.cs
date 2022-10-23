using System.Net.Http.Json;
using System.Text.Json;
using Algebraic.Effect.Abstractions;
using Algebraic.Effect.Http;
using LanguageExt;
using LanguageExt.Effects.Traits;
using LanguageExt.Pipes;
using static LanguageExt.Prelude;

namespace Algebraic.Effect.Http;

public static class HttpExtension
{
    public static JsonSerializerOptions JsonSerializerOptions { get; } = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static Aff<R> ToDeserialJsonAff<R>(this HttpResponseMessage response) => 
        from _1 in Aff(() => response.EnsureSuccessStatusCode()
                                     .Content
                                     .ReadFromJsonAsync<R>(JsonSerializerOptions)
                                     .ToValue())
        select _1;

    public static Eff<Unit> EffAddHeader(this HttpClient http, string name, string? value) =>
       Eff(fun(() => http.DefaultRequestHeaders.Add(name, value)));

    public static Aff<R> AffPost<R>(this HttpClient http, string requestUri, object jsonBody) =>
        from _1 in Aff(() => http.PostAsync(requestUri, JsonContent.Create(jsonBody)).ToValue())
        from _2 in _1.ToDeserialJsonAff<R>()
        select _2;

    public static Aff<R> GetAff<R>(this HttpClient http, string requestUri) =>
        from _1 in Aff(() => http.GetAsync(requestUri).ToValue())
        from _2 in _1.ToDeserialJsonAff<R>()
        select _2;

}
