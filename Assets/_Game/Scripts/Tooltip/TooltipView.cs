using TMPro;
using UnityEngine;

namespace _Game.Scripts.PlayerInput.Tooltip
{
    public class TooltipView : MonoBehaviour
    {
        [SerializeField] private GameObject _frame;
        [SerializeField] private TMP_Text _tooltipText;

        public void SetTooltipText(string tooltip)
        {
            if(!_frame.activeInHierarchy)
                _frame.gameObject.SetActive(true);
            _tooltipText.text = tooltip;
        }

        public void HideTooltip()
        {
            if(_frame.activeInHierarchy)
                _frame.gameObject.SetActive(false);
            
            _tooltipText.text = "";
        }
    }
}