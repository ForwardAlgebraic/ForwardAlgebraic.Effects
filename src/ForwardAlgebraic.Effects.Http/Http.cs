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
    public static Aff<RT, R> ToDeserialJsonAff<RT, R>(this HttpResponseMessage response)
        where RT : struct, HasCancel<RT>, Has<RT, HttpClient> =>
            from _1 in Aff(() => response.EnsureSuccessStatusCode()
                                         .Content
                                         .ReadFromJsonAsync<R>(Http<RT>.JsonSerializerOptions)
                                         .ToValue())
            select _1;
}


public interface Http<RT> where RT : struct, HasCancel<RT>, Has<RT, HttpClient>
{
    public static JsonSerializerOptions JsonSerializerOptions { get; } = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static Aff<RT, R> PostAff<R>(string requestUri, object jsonBody) =>
        from http in Has<RT, HttpClient>.Eff
        from _1 in Aff(() => http.PostAsync(requestUri, JsonContent.Create(jsonBody)).ToValue())
        from _2 in _1.ToDeserialJsonAff<RT, R>()
        select _2;

    public static Aff<RT, R> GetAff<R>(string requestUri) =>
        from http in Has<RT, HttpClient>.Eff
        from _1 in Aff(() => http.GetAsync(requestUri).ToValue())
        from _2 in _1.ToDeserialJsonAff<RT, R>()
        select _2;

    public static Eff<RT, Unit> AddHeaderEff(string name, string? value) =>
        from http in Has<RT, HttpClient>.Eff
        from _1 in Eff(fun(() => http.DefaultRequestHeaders.Add(name, value)))
        select unit;
}
