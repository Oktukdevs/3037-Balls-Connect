using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Controllers;
using Runtime.Core.UI.Popup;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData.Data;

namespace Runtime.Game.GameStates.Game.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IUiService _uiService;
        private readonly UserProfileService _userProfileService;
        private AvatarSelectionService _avatarSelectionService;

        private UserProfileData _modifiedData;

        public event Action OnSaved;
        
        public ProfileController(IUiService uiService, UserProfileService userProfileService, AvatarSelectionService avatarSelectionService)
        {
            _uiService = uiService;
            _userProfileService = userProfileService;
            _avatarSelectionService = avatarSelectionService;
        }

        public override UniTask Run(CancellationToken cancellationToken)
        {
            base.Run(cancellationToken);

            _modifiedData = _userProfileService.GetProfileDataCopy();
            
            ProfilePopup popup = _uiService.GetPopup<ProfilePopup>(ConstPopups.ProfilePopup);
            popup.Show(null, cancellationToken).Forget();
            
            popup.SetName(_modifiedData.Name);
            popup.SetAvatar(_userProfileService.GetUsedAvatarSprite());

            popup.OnNameFieldChanged += (value) =>
            {
                if (value.Length < 2 || !Char.IsLetter(value[0]))
                    popup.SetName(_modifiedData.Name);
                else
                    _modifiedData.Name = value;
            };

            popup.OnAvatarSelectPressed += async () =>
            {
                var avatar = await _avatarSelectionService.PickImage(512);
                if (avatar)
                {
                    _modifiedData.AvatarBase64 = _userProfileService.ConvertToBase64(avatar);
                    popup.SetAvatar(avatar);
                }
            };
            
            popup.OnSavePressed += () =>
            {
                _userProfileService.SaveAccountData(_modifiedData);
                OnSaved?.Invoke();
                popup.DestroyPopup();
            };
            
            CurrentState = ControllerState.Complete;
            return UniTask.CompletedTask;
        }
    }
}