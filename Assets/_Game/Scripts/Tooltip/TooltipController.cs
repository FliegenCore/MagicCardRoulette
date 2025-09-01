using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.PlayerInput.Tooltip
{
    public class TooltipController : IService
    {
        private TooltipView _tooltipView;
        
        public TooltipView TooltipView => _tooltipView;
        
        public void Initialize()
        {
            Spawn();
        }

        private void Spawn()
        {
            var asset = Resources.Load<TooltipView>("Prefabs/UI/[Canvas] TooltipView");
            _tooltipView = Object.Instantiate(asset);
            _tooltipView.HideTooltip();
        }
    }
}