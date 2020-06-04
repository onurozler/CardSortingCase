using UnityEngine;

namespace Game.CardSystem.Base
{
    public class CardBase : MonoBehaviour
    {
        public TextMesh Number;
        public SpriteRenderer NumberIcon;
        public SpriteRenderer MainIcon;

        public void Initialize()
        {
            Debug.Log("Initialized");
        }
    }
}
