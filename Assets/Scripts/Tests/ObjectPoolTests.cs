using NUnit.Framework;
using UnityEngine;

namespace Assets.Scripts.Tests
{
    public class ObjectPoolTests
    {
        [Test]
        public void Test_AddToPool()
        {
            var droid = new GameObject().AddComponent<Droid>();
            ObjectPool.Add(droid);

            var value = ObjectPool.GetAmountInPool<Droid>();
            Assert.AreEqual(1, value);
        }

        [Test]
        public void Test_AddMultipleToPool()
        {
            for (int i = 0; i < 5; i++)
            {
                var droid = new GameObject().AddComponent<Droid>();
                ObjectPool.Add(droid);
            }

            var value = ObjectPool.GetAmountInPool<Droid>();
            Assert.AreEqual(5, value);
        }

        [Test]
        public void Test_GrabFromPool()
        {
            var droid = new GameObject().AddComponent<Droid>();
            ObjectPool.Add(droid);

            var droidData = ObjectPool.Get<Droid>();
            Assert.IsNotNull(droidData);
        }

        [Test]
        public void Test_GrabMultipleFromPool()
        {
            for (int i = 0; i < 5; i++)
            {
                var droid = new GameObject().AddComponent<Droid>();
                ObjectPool.Add(droid);

                var droidData = ObjectPool.Get<Droid>();
            }

            Assert.AreEqual(0, ObjectPool.GetAmountInPool<Droid>());
        }

        [Test]
        public void Test_ConvertToConcreteType()
        {
            // Add droid to pool
            var droid = new GameObject().AddComponent<Droid>();
            ObjectPool.Add(droid);

            // Convert to concrete type
            var newDroid = ObjectPool.GetCast<Droid>();
            Assert.IsTrue(newDroid != default(Droid));
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
