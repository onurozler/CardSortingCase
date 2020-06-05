using System.Collections.Generic;
using System.Linq;
using Game.CardSystem.Model;
using UnityEngine;

namespace Game.Managers
{
    public class AssetManager : MonoBehaviour
    {
        private const string SUFFIX = "_Icon";
        
        [SerializeField]
        private List<Sprite> _cardIcons;
        [SerializeField]
        private List<Sprite> _portraitIcons;

        public Sprite GetCardIcon(CardType cardType)
        {
            return _cardIcons.FirstOrDefault(x=> x.name == cardType+SUFFIX);
        }

        public Sprite GetPortraitIcon(string portName)
        {
            return _cardIcons.FirstOrDefault(x => x.name == portName);
        }
    }
}
