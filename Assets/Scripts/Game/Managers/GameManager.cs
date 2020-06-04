using UnityEngine;
using Zenject;

namespace Game.Managers
{
    public class GameManager : MonoBehaviour
    {
        private PoolManager _poolManager;
        
        [Inject]
        private void OnInstaller(PoolManager poolManager)
        {
            _poolManager = poolManager;
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
