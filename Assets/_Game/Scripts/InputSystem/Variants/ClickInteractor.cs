using _Game.Scripts.Items;
using UnityEngine;

namespace _Game.Scripts.PlayerInput.Variants
{
    public class ClickInteractor : Interactor
    {
        private Raycaster _raycaster;
        
        private IClickble _clickble;
        
        public ClickInteractor(Raycaster raycaster)
        {
            _raycaster = raycaster;
        }
        
        public override bool TryInteract()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_raycaster.TryGetClickableObject(Input.mousePosition, 
                        out IClickble clickble, out Vector2 worldPos))
                {
                    _clickble = clickble;

                    return true;

                }
            }
            
            return false;
        }

        public override void SelfUpdate()
        {
            if (_clickble == null)
            {
                return;
            }
            
            if(OnClickUp())
            {
                _clickble.OnClick();
                _clickble = null;
            }
        }
    }
}