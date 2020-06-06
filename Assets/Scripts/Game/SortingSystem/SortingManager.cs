using Game.CardSystem.Managers;
using Zenject;

namespace Game.SortingSystem
{
    public class SortingManager
    {
        private CardCurveManager _cardCurveManager;
        
        [Inject]
        private void OnInstaller(CardCurveManager cardCurveManager)
        {
            _cardCurveManager = cardCurveManager;
        }
        
        public SortingManager()
        {
        }

        public void ConsecutiveSort()
        {
            
        }
    }
}
