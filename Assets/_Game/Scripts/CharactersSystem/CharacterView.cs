using System;
using DG.Tweening;
using DragonBones;
using UnityEngine;

namespace _Game.Scripts.CharactersSystem
{
    public abstract class CharacterView : MonoBehaviour
    {
        protected UnityArmatureComponent _unityArmatureComponent;
        [SerializeField] protected AudioSource _electrodeAudio;
        [SerializeField] protected AudioSource _damageAudio;
        [SerializeField] protected ParticleSystem _electrodeParticles;
        [SerializeField] private SpriteRenderer _lock;

        private void Start()
        {
            _unityArmatureComponent = GetComponent<UnityArmatureComponent>();
        }
        
        public void EnableLock()
        {
            _lock.DOFade(1, 0.25f);
        }

        public void DisableLock()
        {
            _lock.DOFade(0, 0.25f);
        }
        
        public abstract void TakeDamage();
        public abstract void Death(Action callback);
    }
}