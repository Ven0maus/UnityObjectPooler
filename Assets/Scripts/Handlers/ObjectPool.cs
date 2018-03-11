using Assets.Scripts.IProviders;
using System;
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
            return Pool.Count(f => f.GetType().Name.Equals(typeof(T).Name));
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
        /// Get available poolable obj from the pool converted to its concrete type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T Get<T>() where T : IPoolable
        {
            var poolableObj = Pool.FirstOrDefault(f => f.GetType().Name.Equals(typeof(T).Name));
            if (poolableObj != null)
            {
                // Get existing enemy data
                Pool.Remove(poolableObj);
                poolableObj.GameObject.SetActive(true);
                return GetConcreteType<T>(poolableObj);
            }
            return default(T);
        }

        public static T GetCustom<T>(Func<T, bool> criteria) where T : IPoolable
        {
            var poolableObjects = Pool.Where(f => f.GetType().Name.Equals(typeof(T).Name));
            foreach (var poolableObj in poolableObjects)
            {
                if (criteria.Invoke((T)poolableObj))
                {
                    // Get existing enemy data
                    Pool.Remove(poolableObj);
                    poolableObj.GameObject.SetActive(true);
                    return GetConcreteType<T>(poolableObj);
                }
            }
            return default(T);
        }

        /// <summary>
        /// Returns the concrete type of this PoolableType.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enemy"></param>
        /// <returns></returns>
        private static T GetConcreteType<T>(IPoolable obj) where T : IPoolable
        {
            try
            {
                return (T)Convert.ChangeType(obj, obj.GetType());
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }
    }
}
