using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class Enemy
    {
        public GameObject Prefab;
        public EnemyType Type;
    }

    public enum EnemyType
    {
        Droid,
        Reaper
    }

    public class EnemyHandler : MonoBehaviour
    {
        public Enemy[] Enemies;
    }
}
