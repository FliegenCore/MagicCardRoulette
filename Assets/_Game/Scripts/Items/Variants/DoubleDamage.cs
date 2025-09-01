using _Game.Scripts.CardSystem;
using Game.ServiceLocator;

namespace _Game.Scripts.Items.Variants
{
    public class DoubleDamage : Item
    {
        private bool _canUse = true;
        
        public override void Use()
        {
            if(!_canUse)
                return;

            Card card = G.Get<DeckController>().GetTopCard();
            card.BoostDamage(2);
            card.PlayBoostEffect();
            _canUse = false;
            
            base.Use();
        }
    }
}