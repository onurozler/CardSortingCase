using Game.CardSystem.Base;
using UnityEngine;
using Zenject;

namespace Game.CardSystem.Managers
{
    public class CardPoolManager : MonoMemoryPool<CardBase>
    {
        protected override void OnDespawned(CardBase item)
        {
            base.OnDespawned(item);
            item.transform.position = Vector3.zero;
        }
    }
}
