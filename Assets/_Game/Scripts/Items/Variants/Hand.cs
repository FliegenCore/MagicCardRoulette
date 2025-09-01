using _Game.Scripts.CardSystem;
using Game.ServiceLocator;

namespace _Game.Scripts.Items.Variants
{
    public class Hand : Item
    {
        private bool _canUse = true;
        
        public override void Use()
        {
            if(!_canUse)
                return;

            Card card = G.Get<DeckController>().GetTopCard();
            card.DisableCollider();
            card.JustOpenAndDissolve();
            _canUse = false;
            
            base.Use();
        }
    }
}