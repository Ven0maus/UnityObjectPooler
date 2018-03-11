using Assets.Scripts.IProviders;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public static class ObjectPool
    {
        /// <summary>
        /// The pool collection.
        /// </summary>
        private static HashSet<IPoolable> Pool = new HashSet<IPoolable>();

        /// <summary>
        /// Clear's the entire pool of objects, must be called on new scene load.
        /// </summary>
        public static void Clear()
        {
            Pool.Clear();
        }

        /// <summary>
        /// Returns the amount of objects in the pool of this given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetAmountInPool<T>() where T : IPoolable
        {
            return Pool.Count(f => f.GetPoolableType().Equals(typeof(T).Name));
        }

        /// <summary>
        /// Add poolable obj to the pool.
        /// </summary>
        /// <param name="enemy"></param>
        public static void Add(IPoolable poolableObj)
        {
            if (!Pool.Contains(poolableObj))
            {
                // Add to pool and deactivate enemy.
                Pool.Add(poolableObj);
                poolableObj.GameObject.SetActive(false);
            }
            else
            {
                UnityEngine.Debug.LogWarning("Key already present in pool.");
            }
        }

        /// <summary>
        /// Get available poolable obj from the pool.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IPoolable Get<T>()
        {
            var poolableObj = Pool.FirstOrDefault(f => f.GetPoolableType().Equals(typeof(T).Name));
            if (poolableObj != null)
            {
                // Get existing enemy data
                Pool.Remove(poolableObj);
                poolableObj.GameObject.SetActive(true);
                return poolableObj;
            }
            return null;
        }

        /// <summary>
        /// Get available poolable obj from the pool converted to its concrete type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetCast<T>()
        {
            var poolableObj = Pool.FirstOrDefault(f => f.GetPoolableType().Equals(typeof(T).Name));
            if (poolableObj != null)
            {
                // Get existing enemy data
                Pool.Remove(poolableObj);
                poolableObj.GameObject.SetActive(true);
                return poolableObj.GetConcreteType<T>();
            }
            return default(T);
        }
    }
}
