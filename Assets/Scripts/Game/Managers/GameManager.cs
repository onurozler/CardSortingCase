using Game.CardSystem.Managers;
using Game.View;
using UnityEngine;
using Zenject;

namespace Game.Managers
{
    public class GameManager : MonoBehaviour
    {
        private PlayerView _playerView;
        private CardPoolManager _cardPoolManager;
        
        [Inject]
        private void OnInstaller(CardPoolManager cardPoolManager,PlayerView playerView)
        {
            _playerView = playerView;
            _cardPoolManager = cardPoolManager;
        }
        
        private void Start()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            _playerView.Initialize();
        }
    }
}
