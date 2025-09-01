using System;
using System.Collections;
using UnityEngine;

namespace _Game.Scripts.CharactersSystem.Enemy
{
    public class EnemyView : CharacterView
    {
        private Coroutine _stopCoroutine;
        
        public override void TakeDamage()
        {
            if (_stopCoroutine != null)
            {
                StopCoroutine(_stopCoroutine);
                _stopCoroutine = null;
            }
            
            _electrodeAudio.Play();
            _damageAudio.Play();
            _electrodeParticles.Play();
            var animState = _unityArmatureComponent.animation.Play("damage");
            _stopCoroutine = StartCoroutine(StopAnimation(animState._duration));
        }

        public override void Death(Action callback)
        {
            if (_stopCoroutine != null)
            {
                StopCoroutine(_stopCoroutine);
                _stopCoroutine = null;
            }
            
            StartCoroutine(WaitDeath(callback));
        }

        private IEnumerator WaitDeath(Action callback)
        {
            yield return new WaitForSeconds(1.5f);
            
            
            var animState = _unityArmatureComponent.animation.Play("death");
            StartCoroutine(DoCallback(callback, animState._duration));
        }

        private IEnumerator DoCallback(Action callback, float duration)
        {
            yield return new WaitForSeconds(duration);
            
            callback?.Invoke();
        }

        private IEnumerator StopAnimation(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            _unityArmatureComponent.animation.Play("idle");
        }
    }
}