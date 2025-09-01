using _Game.Scripts.RoundsSystem;
using Game.ServiceLocator;
using UnityEngine;
using CharacterController = _Game.Scripts.CharactersSystem.CharacterController;

namespace _Game.Scripts.CardSystem.EffectVariants
{
    public class AttackEffect : Effect
    {
        private int _damage = 1;
        
        public override void DoEffect(CharacterController characterController, CharacterController _)
        {
            characterController.TakeDamage(_damage);
            G.Get<TurnController>().SwitchWalker();
        }

        public void FactorDamage(int factor)
        {
            _damage *= factor;
        }
    }
}