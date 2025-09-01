using UnityEngine;

namespace _Game.Scripts.RoundsSystem
{
    public class TurnView : MonoBehaviour
    {
        [SerializeField] private AudioSource _lighSound;
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private Sprite _redLightSprite;
        [SerializeField] private Sprite _greenLightSprite;

        public void EnableGreen()
        {
            Debug.Log(name + " is green");
            _lighSound.Play();
            _sprite.sprite = _greenLightSprite;
        }

        public void EnableRed()
        {
            _lighSound.Play();
            _sprite.sprite = _redLightSprite;
        }
    }
}