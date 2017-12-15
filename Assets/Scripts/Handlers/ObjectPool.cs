using Assets.Scripts.IProviders;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public static class ObjectPool
    {
        private static Dictionary<int, IPoolable> Pool = new Dictionary<int, IPoolable>();

        /// <summary>
        /// Clear's the entire pool of objects, must be called on new scene load.
        /// </summary>
        public static void Clear()
        {
            Pool.Clear();
        }

        private static int CurrentId = 0;
        public static int? GetUniqueId()
        {
            CurrentId++;
            return CurrentId;
        }

        /// <summary>
        /// Returns the amount of objects in the pool of this given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetAmountInPool(PoolableType type)
        {
            return Pool.Values.Count(f => f.GetPoolableType() == type);
        }

        /// <summary>
        /// Add poolable obj to the pool.
        /// </summary>
        /// <param name="enemy"></param>
        public static void Add(IPoolable poolableObj)
        {
            IPoolable data;
            if (!Pool.TryGetValue(poolableObj.Id, out data))
            {
                // Add to pool and deactivate enemy.
                Pool.Add(poolableObj.Id, poolableObj);
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
        public static IPoolable Get(PoolableType type)
        {
            var poolableObj = Pool.Values.FirstOrDefault(f => f.GetPoolableType() == type);
            if (poolableObj != null)
            {
                // Get existing enemy data
                Pool.Remove(poolableObj.Id);
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
        public static T Get<T>(PoolableType type)
        {
            var poolableObj = Pool.Values.FirstOrDefault(f => f.GetPoolableType() == type);
            if (poolableObj != null)
            {
                // Get existing enemy data
                Pool.Remove(poolableObj.Id);
                poolableObj.GameObject.SetActive(true);
                return poolableObj.GetConcreteType<T>();
            }
            return default(T);
        }
    }
}
