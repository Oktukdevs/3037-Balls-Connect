using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Game.GameStates.Game.Controllers;
using Runtime.Game.Services.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HomeScreen : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _htpButton;
    [SerializeField] private Button _profileButton;
    [SerializeField] private Button _dailyButton;
    [SerializeField] private Image _avatarImage;
    

    private IUiService _uiService;
    private SettingsPopupController _settingsPopupController;
    private ModeSelectStateController _modeSelectStateController;
    private InventoryController _inventoryController;
    private ProfileController _profileController;
    private UserProfileService _userProfileService;
    private DailyRewardController _dailyRewardController;
    
    [Inject]
    private void Construct(IUiService uiService, ModeSelectStateController modeSelectStateController, 
        InventoryController inventoryController, SettingsPopupController settingsPopupController,
        ProfileController profileController, DailyRewardController dailyRewardController, UserProfileService userProfileService)
    {
        _uiService = uiService;
        _modeSelectStateController = modeSelectStateController;
        _inventoryController = inventoryController;
        _settingsPopupController = settingsPopupController;
        _profileController = profileController;
        _dailyRewardController = dailyRewardController;
        _userProfileService = userProfileService;
        
        _profileController.OnSaved += UpdateUsedAvatar;
    }

    private void OnDestroy()
    {
        _profileController.OnSaved -= UpdateUsedAvatar;
    }

    private void Awake()
    {
        _htpButton.onClick.AddListener(async () => await _uiService.ShowPopup(ConstPopups.HowToPlayPopup));
        _playButton.onClick.AddListener(() => _modeSelectStateController.Enter(CancellationToken.None).Forget());
        _settingsButton.onClick.AddListener(() => _settingsPopupController.Run(CancellationToken.None).Forget());
        _inventoryButton.onClick.AddListener(() => _inventoryController.Run(CancellationToken.None).Forget());
        _profileButton.onClick.AddListener(() => _profileController.Run(CancellationToken.None).Forget());
        _dailyButton.onClick.AddListener(() => _dailyRewardController.Run(CancellationToken.None).Forget());
    }

    private void Start()
    {
        UpdateUsedAvatar();
    }

    private void UpdateUsedAvatar()
    {
        var avatar = _userProfileService.GetUsedAvatarSprite();
        if(avatar)
            _avatarImage.sprite = avatar;
    }
}
