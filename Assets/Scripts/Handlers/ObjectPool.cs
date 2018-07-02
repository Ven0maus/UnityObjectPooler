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
        private static Dictionary<string, List<IPoolable>> Pool = new Dictionary<string, List<IPoolable>>();

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
            List<IPoolable> poolableObjects;
            if (Pool.TryGetValue(typeof(T).Name, out poolableObjects))
            {
                return poolableObjects.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Add poolable obj to the pool.
        /// </summary>
        /// <param name="enemy"></param>
        public static void Add(IPoolable poolableObj)
        {
            List<IPoolable> poolableObjects;
            string key = poolableObj.GetType().Name;
            if (!Pool.TryGetValue(key, out poolableObjects))
            {
                // Add to pool and deactivate enemy.
                Pool.Add(key, new List<IPoolable> { poolableObj });
                poolableObj.GameObject.SetActive(false);
            }
            else
            {
                poolableObjects.Add(poolableObj);
            }
        }

        /// <summary>
        /// Get available poolable obj from the pool converted to its concrete type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T Get<T>() where T : IPoolable
        {
            List<IPoolable> poolableObjects;
            string key = typeof(T).Name;
            
            if (Pool.TryGetValue(key, out poolableObjects))
            {
                IPoolable poolableObj = poolableObjects.First();
                // Get existing enemy data
                poolableObjects.Remove(poolableObj);
                poolableObj.GameObject.SetActive(true);
                if (poolableObjects.Count == 0)
                    Pool.Remove(key);
                return GetConcreteType<T>(poolableObj);
            }
            return default(T);
        }

        public static T GetCustom<T>(Func<T, bool> criteria) where T : IPoolable
        {
            List<IPoolable> poolableObjects;
            string key = typeof(T).Name;

            if (Pool.TryGetValue(key, out poolableObjects))
            {
                var customObjects = poolableObjects.Where(f => criteria.Invoke((T)f));
                
                foreach (var poolableObj in customObjects)
                {
                    if (criteria.Invoke((T)poolableObj))
                    {
                        // Get existing enemy data
                        poolableObjects.Remove(poolableObj);
                        poolableObj.GameObject.SetActive(true);
                        if (poolableObjects.Count == 0)
                            Pool.Remove(key);
                        return GetConcreteType<T>(poolableObj);
                    }
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
