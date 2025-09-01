using _Game.Scripts.AI;
using _Game.Scripts.CardSystem;
using _Game.Scripts.CharactersSystem.Enemy;
using _Game.Scripts.CharactersSystem.Player;
using _Game.Scripts.Fader;
using _Game.Scripts.Items;
using _Game.Scripts.LevelSystem;
using _Game.Scripts.PlayerInput;
using _Game.Scripts.PlayerInput.Tooltip;
using _Game.Scripts.RoundsSystem;
using _Game.Scripts.SceneManagment;
using _Game.Scripts.TranslateSystem;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts._Installers
{
    public class CoreInstaller : MonoBehaviour
    {
        private void Start()
        {
            Register();
            InitializeServices();
        }

        private void Register()
        {
            G.InstantiateAndRegisterService<InputRoot>(ServiceLifetime.Transient);
            G.Register(new TooltipController(), ServiceLifetime.Transient);
            G.Register(new AIController(), ServiceLifetime.Transient);
            G.Register(new TurnController(), ServiceLifetime.Transient);
            G.Register(new PlayerController(), ServiceLifetime.Transient);
            G.Register(new EnemyController(), ServiceLifetime.Transient);
            G.Register(new RoundController(), ServiceLifetime.Transient);
            G.Register(new LevelInitializer(), ServiceLifetime.Transient);
            G.Register(new DeckController(), ServiceLifetime.Transient);
            G.Register(new ItemsController(), ServiceLifetime.Transient);
        }

        private void InitializeServices()
        {
            G.InitializeServices();
        }
    }
}