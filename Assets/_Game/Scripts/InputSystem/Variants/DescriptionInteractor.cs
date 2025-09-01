using _Game.Scripts.PlayerInput.Tooltip;
using _Game.Scripts.TranslateSystem;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.PlayerInput.Variants
{
    public class DescriptionInteractor
    {
        private Raycaster _raycaster;
        
        public DescriptionInteractor(Raycaster raycaster)
        {
            _raycaster = raycaster;
        }

        public void SelfUpdate()
        {
            if (_raycaster.TryGetHasDescription(Input.mousePosition, out IHasDescription result, out Vector2 worldPos))
            {
                string text = Translator.Translate(result.Description);
                G.Get<TooltipController>().TooltipView.SetTooltipText(text);
            }
            else
            {
                G.Get<TooltipController>().TooltipView.HideTooltip();
            }
        }
        
    }
}