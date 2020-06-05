using Config;
using Game.CardSystem.Base;
using Game.CardSystem.Controllers;
using Game.CardSystem.Managers;
using Game.DeckSystem.Managers;
using Game.Managers;
using Game.View;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        private const string CARD_PREFAB_PATH = "Prefabs/CardBase";

        [SerializeField] 
        private CardController _cardController;
        
        [SerializeField] 
        private Transform _poolManager;

        public override void InstallBindings()
        {
            Container.Bind<CardManager>().AsSingle().NonLazy();
            Container.Bind<DeckManager>().AsSingle().NonLazy();
            Container.Bind<CardCurveManager>().AsSingle().NonLazy();
            Container.Bind<CardInputController>().AsSingle().NonLazy();
            
            Container.Bind<AssetManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Camera>().FromComponentInHierarchy().AsSingle();

            Container.BindMemoryPool<CardBase,CardPoolManager>().WithInitialSize(GameConfig.INITIAL_POOLITEM_COUNT).
                FromComponentInNewPrefabResource(CARD_PREFAB_PATH).UnderTransform(_poolManager);
            
            Container.BindInstance(_cardController);
        }
    }
}