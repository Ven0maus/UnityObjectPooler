using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public static class ObjectPool
    {
        /// <summary>
        /// The pool collection.
        /// </summary>
        private static Dictionary<string, List<Component>> Pool = new Dictionary<string, List<Component>>();

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
        public static int GetAmountInPool<T>() where T : Component
        {
            List<Component> poolableObjects;
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
        public static void Add(Component poolableObj)
        {
            List<Component> poolableObjects;
            string key = poolableObj.GetType().Name;
            if (!Pool.TryGetValue(key, out poolableObjects))
            {
                // Add to pool and deactivate enemy.
                Pool.Add(key, new List<Component> { poolableObj });
                poolableObj.gameObject.SetActive(false);
            }
            else
            {
                poolableObjects.Add(poolableObj);
                poolableObj.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Get available poolable obj from the pool converted to its concrete type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T Get<T>() where T : Component
        {
            List<Component> poolableObjects;
            string key = typeof(T).Name;
            
            if (Pool.TryGetValue(key, out poolableObjects))
            {
                Component poolableObj = poolableObjects.First();
                // Get existing enemy data
                poolableObjects.Remove(poolableObj);
                poolableObj.gameObject.SetActive(true);
                if (poolableObjects.Count == 0)
                    Pool.Remove(key);
                return GetConcreteType<T>(poolableObj);
            }
            return default(T);
        }

        /// <summary>
        /// Get an object in the pool by a custom lambda.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static T GetCustom<T>(Func<T, bool> criteria) where T : Component
        {
            List<Component> poolableObjects;
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
                        poolableObj.gameObject.SetActive(true);
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
        private static T GetConcreteType<T>(Component obj) where T : Component
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
