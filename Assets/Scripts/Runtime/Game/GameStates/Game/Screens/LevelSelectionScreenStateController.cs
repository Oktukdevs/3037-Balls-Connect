using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Game.Gameplay;
using Runtime.Game.Services.UI;
using Runtime.Game.UI.Screen;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game.Screens
{
    public class LevelSelectionScreenStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly GameplayData _gameplayData;
        
        private LevelSelectionScreen _screen;
        
        public LevelSelectionScreenStateController(ILogger logger, IUiService uiService,
            GameplayData gameplayData) : base(logger)
        {
            _uiService = uiService;
            _gameplayData = gameplayData;
        }
        
        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            
            return UniTask.CompletedTask;
        }
        
        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.LevelSelectionScreen);
        }
        
        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<LevelSelectionScreen>(ConstScreens.LevelSelectionScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }
        
        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<MenuStateController>();
            _screen.OnLevelSelected += async (level) =>
            {
                _gameplayData.GameLevelId = level;
                await GoTo<LevelGameScreenStateController>();
            };
        }    
    }
}