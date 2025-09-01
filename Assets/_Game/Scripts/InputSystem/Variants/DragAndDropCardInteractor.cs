using _Game.Scripts.CardSystem;
using _Game.Scripts.CharactersSystem.Player;
using _Game.Scripts.TableSystem;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.PlayerInput.Variants
{
    public class DragAndDropCardInteractor : Interactor
    {
        private Raycaster _raycaster;
        private ICardDraggable _currentCardDraggable;

        public DragAndDropCardInteractor(Raycaster raycaster)
        {
            _raycaster = raycaster;
        }
        
        public override bool TryInteract()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(_raycaster.TryGetDragAndDropObject(Input.mousePosition,
                       out ICardDraggable draggable, 
                       out Vector2 worldPos))
                {
                    _currentCardDraggable = draggable;
                    return true;
                }
            }
            
            return false;
        }

        public override void SelfUpdate()
        {
            Drag();
            
            if (OnClickUp())
            {
                StopDrag();
            }
        }

        private void Drag()
        {
            if (_currentCardDraggable == null)
            {
                return;
            }
            
            Vector2 mousePos = Input.mousePosition;
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            Vector3 pos = new Vector3(worldPos.x, worldPos.y, _currentCardDraggable.Transform.position.z);
            
            _currentCardDraggable.Transform.position = pos;
        }

        private void StopDrag()
        {
            if (_currentCardDraggable == null)
            {
                return;
            }
            
            if(_raycaster.TryGetItemNeederObject(Input.mousePosition, 
                       out ICardNeeder cardPlacement,
                       out Vector2 worldClickPosition))
            {
                _currentCardDraggable.OnPlace(cardPlacement, G.Get<PlayerController>());
            }
            
            _currentCardDraggable = null;
        }
    }
}