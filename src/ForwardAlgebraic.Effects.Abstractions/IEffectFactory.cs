namespace ForwardAlgebraic.Effects.Abstractions;

public interface IEffectFactory<TA>
{
    static IEffectFactory()
    {
        if (!typeof(TA).IsInterface)
        {
            throw new Exception("IEffectFactory not support concreate type");
        }
    }

    TA Create(params object[] args);
}
