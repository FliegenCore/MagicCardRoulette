using System;
using System.Collections;
using _Game.Scripts.CardSystem.EffectVariants;
using _Game.Scripts.CharactersSystem;
using _Game.Scripts.CharactersSystem.Player;
using _Game.Scripts.TableSystem;
using DG.Tweening;
using Game.ServiceLocator;
using UnityEngine;
using CharacterController = _Game.Scripts.CharactersSystem.CharacterController;

namespace _Game.Scripts.CardSystem
{
    public class Card : MonoBehaviour, ICardDraggable
    {
        public event Action<Card> OnCardDissolved;

        
        [SerializeField] private ParticleSystem _boostParticle;
        [SerializeField] private ParticleSystem _switchYinYainParticle;
        [SerializeField] private AudioSource _dealSound;
        [SerializeField] private AudioSource _fireSound;
        [SerializeField] private Transform _sides;
        [SerializeField] private SpriteRenderer _frontSprite;
        [SerializeField] private SpriteRenderer _backSprite;
        
        private Material _frontMaterial;
        private Material _backMaterial;

        private Effect _effect;

        private float _alpha = 1;
        
        private Collider2D _collider;
        public Transform Transform => transform;
        public Effect Effect => _effect;

        public void Initialize(Effect effect, Sprite sprite)
        {
            _frontMaterial = _frontSprite.material;
            _backMaterial = _backSprite.material;
            _effect = effect;
            _frontSprite.sprite = sprite;
            _collider = GetComponent<Collider2D>();
            _collider.enabled = false;
        }

        public void PlayDealSound()
        {
            _dealSound.Play();
        }

        public void PlayYinYanEffect()
        {
            _switchYinYainParticle.Play();
        }
        
        public void PlayBoostEffect()
        {
            _boostParticle.Play();
        }
        
        public void EnableCollider()
        {
            _collider.enabled = true;
        }

        public void BoostDamage(int factor)
        {
            if (_effect is AttackEffect attackEffect)
            {
                attackEffect.FactorDamage(factor);
            }
        }

        public void DisableCollider()
        {
            _collider.enabled = false;
        }

        public IEnumerator JustDissolve()
        {
            yield return new WaitForSeconds(0.25f);
            _fireSound.Play();
            while (_alpha > 0)
            {
                _frontMaterial.SetFloat("_Fade", _alpha);
                _backMaterial.SetFloat("_Fade", _alpha);
                _alpha -= Time.deltaTime;
                yield return null;
            }
        }
        
        public IEnumerator Dissolve()
        {
            yield return new WaitForSeconds(0.25f);
            _fireSound.Play();

            while (_alpha > 0)
            {
                _frontMaterial.SetFloat("_Fade", _alpha);
                _backMaterial.SetFloat("_Fade", _alpha);
                _alpha -= Time.deltaTime;
                yield return null;
            }
            
            OnCardDissolved?.Invoke(this);
        }

        public void Hide()
        {
            _alpha = 0;
            _frontMaterial.SetFloat("_Fade", _alpha);
            _backMaterial.SetFloat("_Fade", _alpha);
        }

        public void JustOpenAndDissolve()
        {
            _sides.DORotate(new Vector3(0, 180, 0), 0.25f).OnComplete(() =>
            {
                StartCoroutine(Dissolve());
            });
        }
        
        public IEnumerator SpawnCardAnimation()
        {
            while (_alpha < 1)
            {
                _frontMaterial.SetFloat("_Fade", _alpha);
                _backMaterial.SetFloat("_Fade", _alpha);
                _alpha += Time.deltaTime;
                yield return null;
            }

        }
        
        public void OnPlace(ICardNeeder cardNeeder, CharacterController user)
        {
            transform.DOShakePosition(0.25f,0.5f,100,15);
            DisableCollider();
            ShowFace(cardNeeder.CharacterController, user);
        }

        public void ShowBack()
        {
            _sides.DORotate(new Vector3(0, 0, 0), 0.25f);
        }
        
        private void ShowFace(CharacterController characterController, CharacterController user)
        {
            _sides.DORotate(new Vector3(0, 180, 0), 0.25f).OnComplete(() =>
            {
                _effect.DoEffect(characterController, user);
                StartCoroutine(Dissolve());
            });
        }

    }
}