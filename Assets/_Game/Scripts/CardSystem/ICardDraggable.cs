using _Game.Scripts.TableSystem;
using UnityEngine;
using CharacterController = _Game.Scripts.CharactersSystem.CharacterController;

namespace _Game.Scripts.CardSystem
{
    public interface ICardDraggable
    {
        Transform Transform { get; }
        void OnPlace(ICardNeeder takeCardZone, CharacterController user);
    }
}