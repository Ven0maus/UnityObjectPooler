using Assets.Scripts.Handlers;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Scripts.Tests
{
    public class ObjectPoolTests
    {
        public class Droid : PoolBehaviour
        {

        }

        public class DroidPlane : PoolBehaviour
        {

        }

        [Test]
        public void Test_AddToPool()
        {
            var droid = new GameObject().AddComponent<Droid>();
            ObjectPool.Add(droid);

            var droid2 = new GameObject().AddComponent<DroidPlane>();
            ObjectPool.Add(droid2);

            var value = ObjectPool.GetAmountInPool<Droid>();
            var value2 = ObjectPool.GetAmountInPool<DroidPlane>();
            Assert.AreEqual(1, value);
            Assert.AreEqual(1, value2);
        }

        [Test]
        public void Test_AddMultipleToPool()
        {
            for (int i = 0; i < 5; i++)
            {
                var droid = new GameObject().AddComponent<Droid>();
                ObjectPool.Add(droid);

                var droid2 = new GameObject().AddComponent<DroidPlane>();
                ObjectPool.Add(droid2);
            }

            var value = ObjectPool.GetAmountInPool<Droid>();
            var value2 = ObjectPool.GetAmountInPool<DroidPlane>();
            Assert.AreEqual(5, value);
            Assert.AreEqual(5, value2);
        }

        [Test]
        public void Test_GrabFromPool()
        {
            var droid = new GameObject().AddComponent<Droid>();
            ObjectPool.Add(droid);

            var droid2 = new GameObject().AddComponent<DroidPlane>();
            ObjectPool.Add(droid2);

            var droidData = ObjectPool.Get<Droid>();
            Assert.IsNotNull(droidData);

            var droidData2 = ObjectPool.Get<DroidPlane>();
            Assert.IsNotNull(droidData2);
        }

        [Test]
        public void Test_GrabMultipleFromPool()
        {
            for (int i = 0; i < 5; i++)
            {
                var droid = new GameObject().AddComponent<Droid>();
                ObjectPool.Add(droid);

                var droid2 = new GameObject().AddComponent<DroidPlane>();
                ObjectPool.Add(droid2);

                var droidObj1 = ObjectPool.Get<Droid>();
                var droidObj2 = ObjectPool.Get<DroidPlane>();

                Assert.IsNotNull(droidObj1);
                Assert.IsNotNull(droidObj2);
            }

            Assert.AreEqual(0, ObjectPool.GetAmountInPool<Droid>());
            Assert.AreEqual(0, ObjectPool.GetAmountInPool<DroidPlane>());
        }

        [TearDown]
        public void Test_Cleanup()
        {
            // Clean all objects from the scene
            foreach (var obj in Object.FindObjectsOfType<GameObject>())
                Object.Destroy(obj);

            // Clean the pool references
            ObjectPool.Clear();
        }
    }
}
