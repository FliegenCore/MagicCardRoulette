using _Game.Scripts.CharactersSystem;
using _Game.Scripts.CharactersSystem.Enemy;
using _Game.Scripts.RoundsSystem;
using Game.ServiceLocator;

namespace _Game.Scripts.CardSystem.EffectVariants
{
    public class EmptyEffect : Effect
    {
        public override void DoEffect(CharacterController characterController, CharacterController user)
        {
            if (characterController != user)
            {
                G.Get<TurnController>().SwitchWalker();
            }
            else
            {
                if (user is EnemyController enemyController)
                {
                    enemyController.DoTurn();
                }
            }
        }
    }
}