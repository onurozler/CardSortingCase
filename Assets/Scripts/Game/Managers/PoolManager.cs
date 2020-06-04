using Game.CardSystem.Base;
using Zenject;

namespace Game.Managers
{
    public class PoolManager : MonoMemoryPool<CardBase>
    {
        protected override void OnSpawned(CardBase item)
        {
            base.OnSpawned(item);
            item.Initialize();
        }
    }
}
