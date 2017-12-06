using Assets.Scripts.Tests;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Scripts
{
    public class ObjectPoolTests
    {
        [UnityTest]
        public IEnumerator GetNewObjectsFromPool()
        {
            TestsHelper.EnemyTests.SetupEnemies();
            var droids = TestsHelper.EnemyTests.SpawnEnemies(EnemyType.Droid, 4).ToArray();

            // Check if we have 4 droids
            Assert.AreEqual(droids.Length, 4);

            yield return null;
        }

        [UnityTest]
        public IEnumerator PoolNewObjectsIntoPool()
        {
            TestsHelper.EnemyTests.SetupEnemies();
            var droids = TestsHelper.EnemyTests.SpawnEnemies(EnemyType.Droid, 4).ToArray();

            // Pool 2 droids
            for (int i = 0; i < 2; i++)
                droids[i].Kill();

            // Get droids amount in pool
            var amountInPool = ObjectPool.GetAmountInPool(EnemyType.Droid);

            // Check if we have 2 droids in the pool
            Assert.AreEqual(amountInPool, 2);

            yield return null;
        }

        [UnityTest]
        public IEnumerator GetPooledObjectFromPool()
        {
            TestsHelper.EnemyTests.PreSetupObjectPool(EnemyType.Droid, 1);

            // Get a droid from the pool without allowing pool growth
            var droid = ObjectPool.Get(EnemyType.Droid, false);

            // Check if the droid exists
            Assert.IsNotNull(droid);

            yield return null;
        }

        [UnityTest]
        public IEnumerator GetMultipleObjectTypesFromPool()
        {
            var enemiesToSetup = new Dictionary<EnemyType, int> { { EnemyType.Droid, 1 }, { EnemyType.Reaper, 1 } };
            TestsHelper.EnemyTests.PreSetupObjectPool(enemiesToSetup);

            // Get amounts in pool
            var amountInPoolDroids = ObjectPool.GetAmountInPool(EnemyType.Droid);
            var amountInPoolReapers = ObjectPool.GetAmountInPool(EnemyType.Reaper);

            // Check if it is correct
            Assert.AreEqual(amountInPoolDroids, 1);
            Assert.AreEqual(amountInPoolReapers, 1);

            // Get a droid and reaper from the pool respectively
            var droid = ObjectPool.Get(EnemyType.Droid, false);
            var reaper = ObjectPool.Get(EnemyType.Reaper, false);

            // Check if the droid and reaper exists
            Assert.IsNotNull(droid);
            Assert.IsNotNull(reaper);

            // Try get more from the pool and see if object is null without grow.
            var droid2 = ObjectPool.Get(EnemyType.Droid, false);
            var reaper2 = ObjectPool.Get(EnemyType.Reaper, false);

            // Check if the extra one is null
            Assert.IsNull(droid2);
            Assert.IsNull(reaper2);

            yield return null;
        }

        [TearDown]
        public void CleanGameobjects()
        {
            foreach (var obj in Object.FindObjectsOfType<GameObject>())
                Object.Destroy(obj);
        }
    }
}
