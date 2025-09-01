using UnityEngine;

namespace _Game.Scripts.Items
{
    public class ItemZone : MonoBehaviour
    {
        private Item _currentItem;
        
        public bool CanSetItem()
        {
            if (_currentItem == null)
            {
                return true;
            }

            return false;
        }
        
        public void SetItem(Item item)
        {
            _currentItem = item;
        }

        private void ClearZone()
        {
            _currentItem = null;
        }
    }
}