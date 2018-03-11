using Assets.Scripts.IProviders;
using System;
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
        /// Returns the concrete type of this PoolableType.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enemy"></param>
        /// <returns></returns>
        public T GetConcreteType<T>()
        {
            try
            {
                return (T)Convert.ChangeType(this, GetType());
            }
            catch(InvalidCastException)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Will return the poolable type, must be overriden by it's inheritted class.
        /// </summary>
        /// <returns></returns>
        public string GetPoolableType()
        {
            return GetType().Name;
        }

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
