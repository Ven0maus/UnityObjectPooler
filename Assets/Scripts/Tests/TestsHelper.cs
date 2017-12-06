using Assets.Scripts.IProviders;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tests
{
    public static class TestsHelper
    {
        public static class EnemyTests
        {
            public static void SetupEnemies()
            {
                // Add enemy handler
                var eh = new GameObject().AddComponent<EnemyHandler>();

                // Add droid data
                var droidData = new GameObject();
                droidData.AddComponent<Enemy_Droid>();

                // Add reaper data
                var reaperData = new GameObject();
                reaperData.AddComponent<Enemy_Reaper>();

                var enemies = new Enemy[]
                {
                    new Enemy { Prefab = droidData, Type = EnemyType.Droid },
                    new Enemy { Prefab = reaperData, Type = EnemyType.Reaper }
                };

                // Add data
                eh.Enemies = enemies;
            }

            public static IEnumerable<IEnemy> SpawnEnemies(EnemyType type, int amount)
            {
                List<IEnemy> enemies = new List<IEnemy>();
                for (int i = 0; i < amount; i++)
                {
                    var enemy = ObjectPool.Get(type, true);
                    enemy.Reset();

                    enemies.Add(enemy);
                }

                return enemies;
            }

            public static void PreSetupObjectPool(EnemyType type, int amount)
            {
                // Setup data
                SetupEnemies();

                // Create enemies
                var allEnemies = SpawnEnemies(type, amount);

                // Pool all enemies created
                foreach (var enemy in allEnemies)
                    enemy.Kill();
            }

            public static void PreSetupObjectPool(Dictionary<EnemyType, int> data)
            {
                // Setup data
                SetupEnemies();

                foreach (var value in data)
                {
                    // Create enemies
                    var allEnemies = SpawnEnemies(value.Key, value.Value);

                    // Pool all enemies created
                    foreach (var enemy in allEnemies)
                        enemy.Kill();
                }
            }
        }
    }
}
