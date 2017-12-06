using UnityEngine;

namespace Assets.Scripts.IProviders
{
    public interface IEnemy
    {
        int? Id { get; }
        int Health { get; set; }
        int Shield { get; set; }
        EnemyType Type { get; }
        GameObject GameObject { get; }

        void Kill();
        void Reset();
    }
}
