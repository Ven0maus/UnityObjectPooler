using UnityEngine;

namespace Assets.Scripts.Handlers
{
    public abstract class PoolBehaviour : MonoBehaviour
    {
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
