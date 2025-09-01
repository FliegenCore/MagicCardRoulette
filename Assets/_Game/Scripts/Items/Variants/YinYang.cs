using _Game.Scripts.CardSystem;
using Game.ServiceLocator;

namespace _Game.Scripts.Items.Variants
{
    public class YinYang : Item
    {
        private bool _canUse = true;

        public override void Use()
        {
            if(!_canUse)
                return;

            Card card = G.Get<DeckController>().GetTopCard();
            
            G.Get<DeckController>().SwitchCardEffect(card);
            card.EnableCollider();
            card.PlayYinYanEffect();
            _canUse = false;
            
            base.Use();
        }
    }
}