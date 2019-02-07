using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ObjectPool
{
    /// <summary>
    /// The pool collection.
    /// </summary>
    private static readonly Dictionary<Type, List<Component>> Pool = new Dictionary<Type, List<Component>>();

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
    /// <returns></returns>
    public static int GetAmountInPool<T>() where T : Component
    {
        List<Component> poolableObjects;
        return Pool.TryGetValue(typeof(T), out poolableObjects) ? poolableObjects.Count : 0;
    }

    /// <summary>
    /// Add poolable obj to the pool.
    /// </summary>
    /// <param name="poolableObj"></param>
    public static void Add(Component poolableObj)
    {
        List<Component> poolableObjects;
        var key = poolableObj.GetType();
        if (!Pool.TryGetValue(key, out poolableObjects))
        {
            // Add to pool and deactivate enemy.
            Pool.Add(key, new List<Component> {poolableObj});
            poolableObj.gameObject.SetActive(false);
        }
        else
        {
            poolableObjects.Add(poolableObj);
            poolableObj.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Add poolable obj to the pool as a base class instead of the subclass.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="poolableObj"></param>
    public static void Add<T>(Component poolableObj) where T : Component
    {
        var otherType = typeof(T);
        var originalType = poolableObj.GetType();
        if (!originalType.IsSubclassOf(otherType))
        {
            // Call normal add when its not a subclass but same class as component.
            if (originalType.Equals(otherType))
            {
                Add(poolableObj);
                return;
            }
            throw new Exception("Generic type: " + originalType.Name + " is not a subclass of type: " + otherType.Name);
        }

        List<Component> poolableObjects;
        Type key = otherType;
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
    /// <returns></returns>
    public static T Get<T>() where T : Component
    {
        List<Component> poolableObjects;
        var key = typeof(T);

        if (Pool.TryGetValue(key, out poolableObjects))
        {
            var poolableObj = poolableObjects.First();
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
        var key = typeof(T);

        if (!Pool.TryGetValue(key, out poolableObjects)) return default(T);
        var customObjects = poolableObjects.Where(f => criteria.Invoke((T) f));
        var poolableObj = customObjects.FirstOrDefault();
        if (poolableObj == null) return default(T);

        // Get existing enemy data
        poolableObjects.Remove(poolableObj);
        poolableObj.gameObject.SetActive(true);
        if (poolableObjects.Count == 0)
            Pool.Remove(key);

        return GetConcreteType<T>(poolableObj);
    }

    /// <summary>
    /// Returns the concrete type of this PoolableType.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    private static T GetConcreteType<T>(Component obj) where T : Component
    {
        try
        {
            return (T) Convert.ChangeType(obj, obj.GetType());
        }
        catch (InvalidCastException)
        {
            return default(T);
        }
    }
}