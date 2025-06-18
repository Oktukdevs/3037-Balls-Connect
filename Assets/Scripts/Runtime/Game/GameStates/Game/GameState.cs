using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Game.GameStates.Game.Controllers;
using Runtime.Game.GameStates.Game.Popups;
using Runtime.Game.GameStates.Game.Screens;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game
{
    public class GameState : StateController
    {
        private readonly StateMachine _stateMachine;

        private readonly MenuStateController _menuStateController;
        private readonly ModeSelectStateController _modeSelectStateController;
        private readonly EndlessGameScreenStateController _endlessGameScreenStateController;
        private readonly EndlessGameOverPopupStateController _endlessGameOverPopupStateController;
        private readonly LevelGameScreenStateController _levelGameScreenStateController;
        private readonly LevelSelectionScreenStateController _levelSelectionScreenStateController;
        private readonly PausePopupStateController _pausePopupStateController;
        private readonly WinPopupStateController _winPopupStateController;
        private readonly LosePopupStateController _losePopupStateController;
        private readonly UserDataStateChangeController _userDataStateChangeController;

        public GameState(ILogger logger,
            MenuStateController menuStateController,
            ModeSelectStateController modeSelectStateController,
            EndlessGameScreenStateController endlessGameScreenStateController,
            EndlessGameOverPopupStateController endlessGameOverPopupStateController,
            LevelGameScreenStateController levelGameScreenStateController,
            LevelSelectionScreenStateController levelSelectionScreenStateController,
            PausePopupStateController pausePopupStateController,
            WinPopupStateController winPopupStateController,
            LosePopupStateController losePopupStateController,
            StateMachine stateMachine,
            UserDataStateChangeController userDataStateChangeController) : base(logger)
        {
            _stateMachine = stateMachine;
            _menuStateController = menuStateController;
            _modeSelectStateController = modeSelectStateController;
            _endlessGameScreenStateController = endlessGameScreenStateController;
            _endlessGameOverPopupStateController = endlessGameOverPopupStateController;
            _levelGameScreenStateController = levelGameScreenStateController;
            _levelSelectionScreenStateController = levelSelectionScreenStateController;
            _pausePopupStateController = pausePopupStateController;
            _winPopupStateController = winPopupStateController;
            _losePopupStateController = losePopupStateController;
            _userDataStateChangeController = userDataStateChangeController;
        }

        public override async UniTask Enter(CancellationToken cancellationToken)
        {
            await _userDataStateChangeController.Run(default);

            _stateMachine.Initialize(_menuStateController, _modeSelectStateController, _endlessGameScreenStateController,
                _levelGameScreenStateController, _levelSelectionScreenStateController, _endlessGameOverPopupStateController,
                _pausePopupStateController, _winPopupStateController, _losePopupStateController);
            _stateMachine.GoTo<MenuStateController>().Forget();
        }
    }
}