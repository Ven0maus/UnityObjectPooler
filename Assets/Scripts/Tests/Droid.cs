using Assets.Scripts.Handlers;
using Assets.Scripts.IProviders;

namespace Assets.Scripts
{
    public class Droid : PoolBehaviour
    {
        public override PoolableType GetPoolableType()
        {
            return PoolableType.Droid;
        }
    }
}
