using System;
using Runtime.Core.UI.Data;

namespace Runtime.Game.UI.Popup.Data
{
    public class SimpleDecisionPopupData : BasePopupData
    {
        public Action PressOkEvent;
        public string Message;
    }
}