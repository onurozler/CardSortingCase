using UniRx;
using UnityEngine;

namespace Game.View
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField]
        private ButtonBase _withdrawCards;
        [SerializeField]
        private ButtonBase _777Sort;
        [SerializeField]
        private ButtonBase _123Sort;
        [SerializeField]
        private ButtonBase _smartSort;

        public void Initialize()
        {
            _withdrawCards.OnClickEvent += PublishWithdrawOnClick;
            _777Sort.OnClickEvent += PublishSameNumberSortOnClick;
            _123Sort.OnClickEvent += PublishConsecutiveSortOnClick;
            _smartSort.OnClickEvent += PublishSmartSortOnClick;
        }

        #region Publishers

        private void PublishWithdrawOnClick()
        {
            MessageBroker.Default.Publish(PlayerButtonType.WITDHDRAW);
        }
        
        private void PublishSameNumberSortOnClick()
        {
            MessageBroker.Default.Publish(PlayerButtonType.SAME_NUMBER_SORT);
        }
        
        private void PublishConsecutiveSortOnClick()
        {
            MessageBroker.Default.Publish(PlayerButtonType.CONSECUTIVE_SORT);
        }
        
        private void PublishSmartSortOnClick()
        {
            MessageBroker.Default.Publish(PlayerButtonType.SMARTSORT);
        }
        
        #endregion

        private void OnDestroy()
        {
            _withdrawCards.OnClickEvent -= PublishWithdrawOnClick;
            _123Sort.OnClickEvent -= PublishConsecutiveSortOnClick;
            _777Sort.OnClickEvent -= PublishSameNumberSortOnClick;
            _smartSort.OnClickEvent -= PublishSmartSortOnClick;
        }
    }

    public enum PlayerButtonType
    {
        WITDHDRAW,
        SAME_NUMBER_SORT,
        CONSECUTIVE_SORT,
        SMARTSORT
    }
}
