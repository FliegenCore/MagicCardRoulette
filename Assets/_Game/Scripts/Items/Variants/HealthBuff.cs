namespace _Game.Scripts.Items.Variants
{
    public class HealthBuff : Item
    {
        private bool _canUse = true;
        
        public override void Use()
        {
            if(!_canUse)
                return;

            _user.AddHealth();
            _canUse = false;
            
            base.Use();
        }
    }
}