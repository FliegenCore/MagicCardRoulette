using UnityEngine;
using CharacterController = _Game.Scripts.CharactersSystem.CharacterController;

namespace _Game.Scripts.TableSystem
{
    public interface ICardNeeder
    {
        Vector2 NeederPosition { get; }
        CharacterController CharacterController { get; }
    }
}