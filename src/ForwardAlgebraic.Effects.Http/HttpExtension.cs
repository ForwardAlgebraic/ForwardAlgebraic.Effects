using System.Net.Http.Json;
using System.Text.Json;
using Algebraic.Effect.Abstractions;
using LanguageExt;
using LanguageExt.Pipes;
using static LanguageExt.Prelude;

namespace Algebraic.Effect.Http;

public class Http<RT> where RT : struct, Has<RT, HttpClient>
{
    public static JsonSerializerOptions JsonSerializerOptions { get; } = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static Aff<R> DeserialJsonAff<R>(HttpResponseMessage response) =>
        from _1 in Aff(() => response.EnsureSuccessStatusCode()
                                     .Content
                                     .ReadFromJsonAsync<R>(JsonSerializerOptions)
                                     .ToValue())
        select _1;

    public static Eff<RT, Unit> AddHeaderEff(string name, string? value) =>
        from http in Has<RT, HttpClient>.Eff
        from _1 in Eff(fun(() => http.DefaultRequestHeaders.Add(name, value)))
        select unit;

    public static Aff<RT, R> PostAff<R>(string requestUri, object jsonBody) =>
        from http in Has<RT, HttpClient>.Eff
        from _1 in Aff(() => http.PostAsync(requestUri, JsonContent.Create(jsonBody)).ToValue())
        from _2 in DeserialJsonAff<R>(_1)
        select _2;

    public static Aff<RT, R> GetAff<R>(string requestUri) =>
        from http in Has<RT, HttpClient>.Eff
        from _1 in Aff(() => http.GetAsync(requestUri).ToValue())
        from _2 in DeserialJsonAff<R>(_1)
        select _2;

}
