using _Game.Scripts.Items;

namespace _Game.Scripts.CharactersSystem
{
    public abstract class CharacterController
    {
        protected bool _canDoTurn = true;
        
        public bool CanDoTurn => _canDoTurn;
        
        public abstract CharacterController Opponent{ get; }
        public abstract void TakeDamage(int damage);
        public abstract void AddHealth();
        public abstract void AddItem(Item item);
        public abstract void LockNextTurn();
        public abstract void TryUnlockTurn();
        public abstract void EnablePanickAnimation();
        public abstract void DisablePanickAnimation();
    }
}