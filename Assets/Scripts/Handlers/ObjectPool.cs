using Assets.Scripts.IProviders;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public static class ObjectPool
    {
        private static Dictionary<int?, IEnemy> Pool = new Dictionary<int?, IEnemy>();

        private static EnemyHandler _enemyHandler;
        private static EnemyHandler EnemyHandler
        {
            get
            {
                if (_enemyHandler != null) return _enemyHandler;
                _enemyHandler = UnityEngine.Object.FindObjectOfType<EnemyHandler>();
                return _enemyHandler;
            }
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
        public static int GetAmountInPool(EnemyType type)
        {
            return Pool.Values.Where(f => f.Type == type).Count();
        }

        /// <summary>
        /// Add enemy to the pool.
        /// </summary>
        /// <param name="enemy"></param>
        public static void Add(IEnemy enemy)
        {
            IEnemy data;
            if (!Pool.TryGetValue(enemy.Id, out data))
            {
                // Add to pool and deactivate enemy.
                Pool.Add(enemy.Id, enemy);
                enemy.GameObject.SetActive(false);
            }
            else
            {
                UnityEngine.Debug.LogWarning("Key already present in pool.");
            }
        }

        /// <summary>
        /// Get available enemy from the pool.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnemy Get(EnemyType type, bool allowGrow = false)
        {
            var enemy = Pool.Values.FirstOrDefault(f => f.Type == type);
            if (enemy != null)
            {
                // Get existing enemy data
                Pool.Remove(enemy.Id);
                enemy.GameObject.SetActive(true);
                return enemy;
            }
            else
            {
                if (allowGrow)
                {
                    // Get existing enemy data
                    var data = EnemyHandler.Enemies.FirstOrDefault(f => f.Type == type);

                    // Make a new one
                    var newData = UnityEngine.Object.Instantiate(data.Prefab, new UnityEngine.Vector3(0, 0, 0), UnityEngine.Quaternion.identity);
                    enemy = newData.GetComponent<IEnemy>();

                    return enemy;
                }
            }
            return null;
        }
    }
}
