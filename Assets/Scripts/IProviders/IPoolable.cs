using UnityEngine;

namespace Assets.Scripts.IProviders
{
    public interface IPoolable
    {
        GameObject gameObject { get; }
    }
}
