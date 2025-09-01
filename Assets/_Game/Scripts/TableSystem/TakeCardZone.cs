using UnityEngine;

namespace _Game.Scripts.TableSystem
{
    public class TakeCardZone : MonoBehaviour, ICardNeeder
    {
        public Vector2 NeederPosition => transform.position;
        public CharactersSystem.CharacterController CharacterController => _currentCharacterView;
        
        private CharactersSystem.CharacterController _currentCharacterView;

        public void Initialize(CharactersSystem.CharacterController characterView)
        {
            _currentCharacterView = characterView;   
        }
    }
}
