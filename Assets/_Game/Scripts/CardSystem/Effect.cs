using _Game.Scripts.CharactersSystem;

namespace _Game.Scripts.CardSystem.EffectVariants
{
    public abstract class Effect
    {
        public abstract void DoEffect(CharacterController characterController, CharacterController user);
    }
}