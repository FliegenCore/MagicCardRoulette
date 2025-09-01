using System.Collections;
using _Game.Scripts.CardSystem;
using _Game.Scripts.CardSystem.EffectVariants;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.Items.Variants
{
    public class Angel : Item
    {
        private bool _canUse = true;
        
        public override void Use()
        {
            if(!_canUse)
                return;

            Card card = G.Get<DeckController>().GetTopCard();

            if (card.Effect is AttackEffect)
            {
                var animationState = _unityArmatureComponent.animation.Play("use_bad");
                StartCoroutine(AfterAnimation(animationState._duration));
            }
            else
            {
                var animationState = _unityArmatureComponent.animation.Play("use_good");
                StartCoroutine(AfterAnimation(animationState._duration));
            }
            
            _canUse = false;
        }

        private IEnumerator AfterAnimation(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}