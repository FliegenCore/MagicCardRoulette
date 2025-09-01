using _Game.Scripts.CardSystem;
using _Game.Scripts.Items;
using _Game.Scripts.PlayerInput.Tooltip;
using _Game.Scripts.TableSystem;
using UnityEngine;

namespace _Game.Scripts.PlayerInput
{
    public class Raycaster
    {
        private Camera _camera;
        private LayerMask _placementMask;
        private LayerMask _cardMask;
        
        public Raycaster(Camera camera)
        {
            _placementMask = LayerMask.GetMask("Placement");
            _cardMask = LayerMask.GetMask("Card");
            _camera = camera;
        }

        public bool TryGetDragAndDropObject(Vector2 screenPosition, out ICardDraggable interactiveObject, out Vector2 worldClickPosition)
        {
            interactiveObject = null;
            worldClickPosition = Vector2.zero;
            
            worldClickPosition = _camera.ScreenToWorldPoint(screenPosition);
            
            RaycastHit2D hit = Physics2D.Raycast(worldClickPosition, Vector2.zero, 0f, _cardMask);
            
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out interactiveObject))
                {
                    worldClickPosition = hit.point;
                    return true;
                }
            }
            
            return false;
        }
        
        public bool TryGetHasDescription(Vector2 screenPosition, out IHasDescription interactiveObject, out Vector2 worldClickPosition)
        {
            interactiveObject = null;
            worldClickPosition = Vector2.zero;
            
            worldClickPosition = _camera.ScreenToWorldPoint(screenPosition);
            
            RaycastHit2D hit = Physics2D.Raycast(worldClickPosition, Vector2.zero, 0f);
            
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out interactiveObject))
                {
                    worldClickPosition = hit.point;
                    return true;
                }
            }
            
            return false;
        }

        public bool TryGetClickableObject(Vector2 screenPosition, out IClickble interactiveObject, out Vector2 worldClickPosition)
        {
            interactiveObject = null;
            worldClickPosition = Vector2.zero;
            
            worldClickPosition = _camera.ScreenToWorldPoint(screenPosition);
            
            RaycastHit2D hit = Physics2D.Raycast(worldClickPosition, Vector2.zero, 0f);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out interactiveObject))
                {
                    worldClickPosition = hit.point;
                    return true;
                }
            }
            
            return false;
        }
        
        public bool TryGetItemNeederObject(Vector2 screenPosition, out ICardNeeder interactiveObject, out Vector2 worldClickPosition)
        {
            interactiveObject = null;
            worldClickPosition = Vector2.zero;
            
            worldClickPosition = _camera.ScreenToWorldPoint(screenPosition);
            
            RaycastHit2D hit = Physics2D.Raycast(worldClickPosition, Vector2.zero, 0f, _placementMask);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out interactiveObject))
                {
                    worldClickPosition = hit.point;
                    return true;
                }
            }
            
            return false;
        }
    }
}