using _Game.Scripts.SceneManagment;
using Game.ServiceLocator;
using UnityEngine;
using UnityEngine.UI;

public class LoseCanvas : MonoBehaviour
{
   [SerializeField] private Button _restartButton;

   private void Awake()
   {
      _restartButton.onClick.AddListener(RestartGame);
   }

   private void RestartGame()
   {
      G.Get<SceneController>().ChangeScene(SceneController.BOOTSTRAP_SCENE);
   }
}
