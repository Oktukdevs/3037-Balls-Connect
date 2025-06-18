using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Core.Infrastructure.Logger;
using Runtime.Core.UI.Popup;
using Runtime.Game.GameStates.Game.Screens;
using Runtime.Game.Services.UI;

namespace Runtime.Game.GameStates.Game.Controllers
{
    public class ModeSelectStateController : StateController
    {
        private readonly IUiService _uiService;

        public ModeSelectStateController(IUiService uiService, ILogger logger) : base(logger)
        {
            _uiService = uiService;
        }

        public override UniTask Enter(CancellationToken cancellationToken = default)
        {
            ModeSelectPopup popup = _uiService.GetPopup<ModeSelectPopup>(ConstPopups.ModeSelectPopup);
            popup.Show(null, cancellationToken).Forget();

            popup.OnCampaignPressed += async () =>
            {
                popup.DestroyPopup();
                await GoTo<LevelSelectionScreenStateController>();
            };
            
            popup.OnEndlessPressed += async () =>
            {
                popup.DestroyPopup();
                await GoTo<EndlessGameScreenStateController>();
            };
            
            return UniTask.CompletedTask;
        }
    }
}