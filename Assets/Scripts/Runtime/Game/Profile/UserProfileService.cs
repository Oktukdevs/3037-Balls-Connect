using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Services.UserData;
using Runtime.Game.Services.UserData.Data;
using UnityEngine;

public class UserProfileService
{
    private readonly UserDataService _userDataService;
    private readonly ImageProcessingService _imageProcessingService;

    public UserProfileService(UserDataService userDataService,
        ImageProcessingService imageProcessingService)
    {
        _userDataService = userDataService;
        _imageProcessingService = imageProcessingService;
    }

    public UserProfileData GetProfileDataCopy()
    {
        return _userDataService.GetUserData().UserProfileData.Copy();
    }

    public void SaveAccountData(UserProfileData modifiedData)
    {
        var origData = _userDataService.GetUserData().UserProfileData;

        foreach (var field in typeof(UserProfileData).GetFields())
            field.SetValue(origData, field.GetValue(modifiedData));

        _userDataService.SaveUserData();
    }

    public Sprite GetUsedAvatarSprite()
    {
        if (!AvatarExists())
            return null;

        return _imageProcessingService.CreateAvatarSprite(GetAvatarBase64());
    }

    [Tooltip("Pass in the selected avatar and assign the returned string to the account data")]
    public string ConvertToBase64(Sprite sprite, int maxSize = 512)
    {
        return _imageProcessingService.ConvertToBase64(sprite, maxSize);
    }

    private bool AvatarExists() => _userDataService.GetUserData().UserProfileData.AvatarBase64 != string.Empty;

    private string GetAvatarBase64() => _userDataService.GetUserData().UserProfileData.AvatarBase64;
}