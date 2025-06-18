using System;

namespace Tools.AssetCreation.PopupCreator
{
    [Serializable]
    public class PopupData
    {
        public string Name;
        public bool CreateData;
        public bool CreateStateController;

        public PopupData()
        {
            Name = "";
            CreateData = false;
            CreateStateController = false;
        }
    }
}