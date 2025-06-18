using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Core.UI.Popup;
using Runtime.Game.GameStates.Game.Screens;
using Runtime.Game.Services.UI;
using UnityEngine;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game.Popups
{
    public class PausePopupStateController : StateController
    {
        private readonly IUiService _uiService;
        
        public PausePopupStateController(ILogger logger, IUiService uiService) : base(logger)
        {
            _uiService = uiService;
        }

        public override async UniTask Enter(CancellationToken cancellationToken = default)
        {
            Time.timeScale = 0;
            
            PausePopup popup = await _uiService.ShowPopup(ConstPopups.PausePopup) as PausePopup;

            popup.OnContinueButtonPressed += () =>
            {
                popup.DestroyPopup();
                Time.timeScale = 1;
            };
            
            popup.OnLeaveButtonPressed += async () =>
            {
                popup.DestroyPopup();
                Time.timeScale = 1;
                await GoTo<MenuStateController>();
            };
        }
    }
}