using UnityEngine;

namespace Assets.Scripts.IProviders
{
    public enum PoolableType
    {
        None,
        Droid
    }

    public interface IPoolable
    {
        int Id { get; }
        GameObject GameObject { get; }
        PoolableType GetPoolableType();
        T GetConcreteType<T>();
    }
}
