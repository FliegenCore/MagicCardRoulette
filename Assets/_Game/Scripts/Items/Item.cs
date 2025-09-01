using System;
using System.Collections;
using _Game.Scripts.CharactersSystem;
using _Game.Scripts.CharactersSystem.Player;
using _Game.Scripts.PlayerInput.Tooltip;
using DragonBones;
using UnityEngine;
using CharacterController = _Game.Scripts.CharactersSystem.CharacterController;

namespace _Game.Scripts.Items
{
    public abstract class Item : MonoBehaviour, IHasDescription, IClickble, IUsble
    {
        public event Action Callback;
        
        [SerializeField] private string _description;

        protected CharacterController _user;
        protected CharacterController _opponent;
        protected UnityArmatureComponent _unityArmatureComponent;
        
        public string Description => _description;

        private void Awake()
        {
            _unityArmatureComponent = GetComponent<UnityArmatureComponent>();
            var animationState = _unityArmatureComponent.animation.Play("plus");
            
            StartCoroutine(EnterInIdle(animationState._duration));
        }

        public void SetOwnerAndOpponent(CharacterController characterController, CharacterController opponent)
        {
            _opponent = opponent;
            _user = characterController;
        }

        protected bool PlayerIsOwner()
        {
            if (_user is PlayerController)
            {
                return true;
            }
            
            return false;
        }
        
        public void OnClick()
        {
            if (PlayerIsOwner())
            {
                Use();
            }
        }
        
        public virtual void Use()
        {
            OnUse();
        }

        private void OnUse()
        {
            var animation = _unityArmatureComponent.animation.Play("use");
            Destroy(gameObject, animation._duration);
        }

        private IEnumerator EnterInIdle(float time)
        {
            yield return new WaitForSeconds(time);
            _unityArmatureComponent.animation.Play("idle");
        }
        
        private void OnDestroy()
        {
            Callback?.Invoke();
        }
    }
}