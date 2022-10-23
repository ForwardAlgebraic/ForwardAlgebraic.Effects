using System.Reactive.Disposables;
using Algebraic.Effect.Abstractions;
using LanguageExt;
using LanguageExt.Effects.Traits;
using static LanguageExt.Prelude;

namespace Algebraic.Effect.Rx;

public interface IDisposable<RT> where RT : struct, HasCancel<RT>, Has<RT, CompositeDisposable>
{
    public static Aff<RT, T> AddEff<T>(T item)
        where T : IDisposable =>
            from cd in Has<RT, CompositeDisposable>.Eff
            from _1 in Eff(fun(() => cd.Add(item)))
            select item;
}
