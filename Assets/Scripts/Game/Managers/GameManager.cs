using UnityEngine;
using Zenject;

namespace Game.Managers
{
    public class GameManager : MonoBehaviour
    {
        private CardPoolManager _cardPoolManager;
        
        [Inject]
        private void OnInstaller(CardPoolManager cardPoolManager)
        {
            _cardPoolManager = cardPoolManager;
        }
        
        private void Start()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
        }
    }
}
