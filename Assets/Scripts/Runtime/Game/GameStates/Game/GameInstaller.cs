using Runtime.Game.DailyLogin;
using Runtime.Game.Gameplay;
using Runtime.Game.GameStates.Game.Controllers;
using Runtime.Game.GameStates.Game.Popups;
using Runtime.Game.GameStates.Game.Screens;
using Runtime.Game.Services.UserData;
using Runtime.Game.Shop;
using UnityEngine;
using Zenject;

namespace Runtime.Game.GameStates.Game
{
    [CreateAssetMenu(fileName = "GameInstaller", menuName = "Installers/GameInstaller")]
    public class GameInstaller : ScriptableObjectInstaller<GameInstaller>
    {
        [SerializeField] private BackgroundActivator _backgroundActivator;
        
        public override void InstallBindings()
        {
            BindStateControllers();
            BindProfile();
            BindControllers();
            BindDaily();
            BindShop();
            BindGameplay();
            Container.BindInterfacesAndSelfTo<LeaderboardRecordFactory>().AsSingle();
        }

        private void BindStateControllers()
        {
            Container.Bind<MenuStateController>().AsSingle();
            Container.Bind<ModeSelectStateController>().AsSingle();
            Container.Bind<EndlessGameScreenStateController>().AsSingle();
            Container.Bind<LevelSelectionScreenStateController>().AsSingle();
            Container.Bind<LevelGameScreenStateController>().AsSingle();
            Container.Bind<EndlessGameOverPopupStateController>().AsSingle();
            Container.Bind<PausePopupStateController>().AsSingle();
            Container.Bind<WinPopupStateController>().AsSingle();
            Container.Bind<LosePopupStateController>().AsSingle();
        }

        private void BindControllers()
        {
            Container.Bind<SettingsPopupController>().AsSingle();
            Container.Bind<InventoryController>().AsSingle();
            Container.Bind<ProfileController>().AsSingle();
            Container.Bind<DailyRewardController>().AsSingle();
        }

        private void BindProfile()
        {
            Container.Bind<UserProfileService>().AsSingle();
            Container.Bind<AvatarSelectionService>().AsSingle();
            Container.Bind<ImageProcessingService>().AsSingle();
        }

        private void BindDaily()
        {
            Container.Bind<DailyRewardService>().AsSingle();
        }

        private void BindShop()
        {
            Container.Bind<UserInventoryService>().AsSingle();
            Container.Bind<ShopService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShopItemsFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<InventoryItemsFactory>().AsSingle();
        }

        private void BindGameplay()
        {
            Container.Bind<BallBoard>().AsSingle();
            Container.Bind<GameSetupController>().AsSingle();
            Container.Bind<BallsTransformController>().AsSingle();
            Container.BindInterfacesAndSelfTo<UserInputService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BallsFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<PathDrawController>().AsSingle().NonLazy();
            Container.Bind<MergeController>().AsSingle().NonLazy();
            Container.Bind<BallsAnimationController>().AsSingle();
            Container.Bind<SpritesProvider>().AsSingle();
            Container.Bind<BallsPool>().AsSingle();
            Container.Bind<BallsPreviewController>().AsSingle();
            Container.Bind<ScoreCalculator>().AsSingle().NonLazy();
            Container.Bind<RewardCalculator>().AsSingle().NonLazy();
            Container.Bind<ComboCalculator>().AsSingle().NonLazy();
            Container.Bind<ClearRatingCalculator>().AsSingle().NonLazy();
            Container.Bind<GameplayData>().AsSingle();
            Container.Bind<UserProgressService>().AsSingle();
            Container.Bind<GameplayTimer>().AsSingle();
            Container.Bind<LevelProgressController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LineDrawer>().AsSingle();
            Container.BindInterfacesAndSelfTo<SwapDrawController>().AsSingle();
            Container.Bind<BackgroundActivator>().FromComponentInNewPrefab(_backgroundActivator).AsSingle();
        }
    }
}