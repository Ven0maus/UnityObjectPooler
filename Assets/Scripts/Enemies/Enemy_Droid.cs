using Assets.Scripts.IProviders;
using UnityEngine;

namespace Assets.Scripts
{
    public class Enemy_Droid : MonoBehaviour, IEnemy
    {
        public int Health { get; set; }
        public int Shield { get; set; }

        public EnemyType Type { get { return EnemyType.Droid; } }
        public GameObject GameObject { get { return gameObject; } }

        private int? _id;
        public int? Id
        {
            get
            {
                if (_id != null) return _id;
                _id = ObjectPool.GetUniqueId();
                return _id;
            }
        }

        public void Kill()
        {
            ObjectPool.Add(this);
        }

        public void Reset()
        {
            Health = 100;
            Shield = 50;

            gameObject.transform.position = new Vector3(UnityEngine.Random.Range(-3f, 3f), 3, 0);
        }

        void Start()
        {
            Reset();
        }
    }
}
