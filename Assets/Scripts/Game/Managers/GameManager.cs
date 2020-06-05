using Game.CardSystem.Controllers;
using Game.View;
using UnityEngine;
using Zenject;

namespace Game.Managers
{
    public class GameManager : MonoBehaviour
    {
        private PlayerView _playerView;
        private CardController _cardController;
        
        [Inject]
        private void OnInstaller(CardController cardController,PlayerView playerView)
        {
            _playerView = playerView;
            _cardController = cardController;
        }
        
        private void Start()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            _playerView.Initialize();
            _cardController.Initialize();
        }
    }
}
