using Config;
using Game.CardSystem.Base;
using Game.CardSystem.Managers;
using Game.DeckSystem.Managers;
using Game.Managers;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        private const string CARD_PREFAB_PATH = "Prefabs/CardBase";
        
        [SerializeField] 
        private Transform _poolManager;
        
        public override void InstallBindings()
        {
            Container.Bind<CardManager>().AsSingle().NonLazy();
            Container.Bind<DeckManager>().AsSingle().NonLazy();
            
            Container.BindMemoryPool<CardBase,CardPoolManager>().WithInitialSize(GameConfig.INITIAL_POOLITEM_COUNT).
                FromComponentInNewPrefabResource(CARD_PREFAB_PATH).UnderTransform(_poolManager);
        }
    }
}