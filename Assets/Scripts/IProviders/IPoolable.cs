using UnityEngine;

namespace Assets.Scripts.IProviders
{
    public interface IPoolable
    {
        GameObject GameObject { get; }
        string GetPoolableType();
        T GetConcreteType<T>();
    }
}
