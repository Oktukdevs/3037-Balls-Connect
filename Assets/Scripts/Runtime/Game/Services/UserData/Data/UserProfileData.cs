using System;

namespace Runtime.Game.Services.UserData.Data
{
    [Serializable]
    public class UserProfileData
    {
        public string Name = "Player";
        public string AvatarBase64 = string.Empty;

        public UserProfileData Copy()
        {
            return (UserProfileData)MemberwiseClone();
        }
    }
}