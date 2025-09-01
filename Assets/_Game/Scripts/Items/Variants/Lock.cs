using _Game.Scripts.CardSystem;
using Game.ServiceLocator;

namespace _Game.Scripts.Items.Variants
{
    public class Lock : Item
    {
        private bool _canUse = true;
        
        public override void Use()
        {
            if(!_canUse)
                return;

            _opponent.LockNextTurn();
            
            _canUse = false;
            
            base.Use();
        }
    }
}