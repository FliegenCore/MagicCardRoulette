using _Game.Scripts.Items;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts
{
    public class Test : MonoBehaviour
    {
        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space))
            {
                G.Get<ItemsController>().GiveItemToPlayer();
            }
#endif
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}