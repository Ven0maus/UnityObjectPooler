using Assets.Scripts.IProviders;
using UnityEngine;

namespace Assets.Scripts.Handlers
{
    public abstract class PoolBehaviour : MonoBehaviour, IPoolable
    {
        /// <summary>
        /// Reference to the gameObject for IPoolable interface.
        /// </summary>
        public GameObject GameObject { get { return gameObject; } }

        /// <summary>
        /// The on death event.
        /// </summary>
        public virtual void OnDeath()
        {
            // Add to pool.
            ObjectPool.Add(this);
        }
    }
}
