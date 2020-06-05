using UniRx;
using UnityEngine;

namespace Game.View
{
    public class PlayerView : MonoBehaviour
    {
        public ButtonBase WithdrawCards;

        public void Initialize()
        {
            WithdrawCards.OnClickEvent += PublishWithdrawOnClick;
        }

        private void PublishWithdrawOnClick()
        {
            MessageBroker.Default.Publish(PlayerButtonType.WITDHDRAW);
        }

        private void OnDestroy()
        {
            WithdrawCards.OnClickEvent -= PublishWithdrawOnClick;
        }
    }

    public enum PlayerButtonType
    {
        WITDHDRAW,
        SMARTSORT
    }
}
