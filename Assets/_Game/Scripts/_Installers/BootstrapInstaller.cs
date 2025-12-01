using _Game.Scripts.Fader;
using _Game.Scripts.SceneManagment;
using _Game.Scripts.TranslateSystem;
using Game.ServiceLocator;
using GameAnalyticsSDK;
using UnityEngine;

namespace _Game.Scripts._Installers
{
    public class BootstrapInstaller : MonoBehaviour
    {
        private void Awake()
        {
            Register();
            InitializeServices();
        }

        private void Register()
        {
            GameAnalytics.Initialize();
            GameAnalytics.SetCustomId("myCustomUserId");
            G.Register(new Translator(), ServiceLifetime.Singleton);
            G.InstantiateAndRegisterService<RoutineStarter>(ServiceLifetime.Singleton);
            G.Register(new FadeController(), ServiceLifetime.Singleton);
            G.Register(new SceneController(), ServiceLifetime.Singleton);
        }

        private void InitializeServices()
        {
            G.InitializeServices();
            
            G.Get<SceneController>().ChangeScene(SceneController.CORE_SCENE);
        }
    }
}